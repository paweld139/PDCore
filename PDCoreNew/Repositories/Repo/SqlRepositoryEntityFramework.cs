using PDCore.Extensions;
using PDCore.Helpers.Wrappers;
using PDCore.Interfaces;
using PDCore.Repositories.IRepo;
using PDCore.Repositories.Repo;
using PDCore.Utils;
using PDCoreNew.Context.IContext;
using PDCoreNew.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PDCoreNew.Repositories.Repo
{
    public class SqlRepositoryEntityFramework<T> : SqlRepository<T>, ISqlRepositoryEntityFramework<T> where T : class, IModificationHistory
    {
        protected readonly IEntityFrameworkDbContext ctx;
        protected readonly DbSet<T> set;

        protected override string ConnectionString => ctx.Database.Connection.ConnectionString;

        public SqlRepositoryEntityFramework(IEntityFrameworkDbContext ctx, ILogger logger) : base(ctx, logger)
        {
            this.ctx = ctx;
            set = this.ctx.Set<T>();
        }

        public override IQueryable<T> FindAll()
        {
            return FindAll(false);
        }

        public IQueryable<T> FindAll(bool asNoTracking)
        {
            if (asNoTracking)
                return set.AsNoTracking();

            return set;
        }

        public IQueryable<KeyValuePair<TKey, TValue>> FindKeyValuePairs<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TValue> valueSelector, bool sortByValue = true) where TValue : IComparable<TValue>
        {
            var query = FindAll().GetKVP(keySelector, valueSelector);

            if (sortByValue)
                query = query.OrderBy(e => e.Value);

            return query;
        }

        public IQueryable<T> FindByFilter(Converter<T, string> converter, string substring)
        {
            var query = FindAll();

            if (!string.IsNullOrWhiteSpace(substring))
                query = query.Filter(substring, converter);

            return query;
        }

        public IQueryable<T> FindPage(int page, int pageSize)
        {
            var query = FindAll();

            if (page > 0 && pageSize > 0)
            {
                query = query.GetPage(page, pageSize);
            }

            return query;
        }

        public IQueryable<T> FindByDateCreated(IQueryable<T> source, string dateF, string dateT)
        {
            return FindAll().FindByDateCreated(dateF, dateT);
        }

        public IQueryable<T> FindByDateCreated(IQueryable<T> source, DateTime? dateF, DateTime? dateT)
        {
            return FindAll().FindByDateCreated(dateF, dateT);
        }

        public IQueryable<T> FindByDateModified(IQueryable<T> source, string dateF, string dateT)
        {
            return FindAll().FindByDateModified(dateF, dateT);
        }

        public IQueryable<T> FindByDateModified(IQueryable<T> source, DateTime? dateF, DateTime? dateT)
        {
            return FindAll().FindByDateModified(dateF, dateT);
        }


        public List<T> GetAll(bool asNoTracking)
        {
            return FindAll(asNoTracking).ToList();
        }

        public List<KeyValuePair<TKey, TValue>> GetKeyValuePairs<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TValue> valueSelector, bool sortByValue = true) where TValue : IComparable<TValue>
        {
            return FindKeyValuePairs(keySelector, valueSelector, sortByValue).ToList();
        }

        public List<T> GetByFilter(Converter<T, string> converter, string substring)
        {
            return FindByFilter(converter, substring).ToList();
        }

        public List<T> GetPage(int page, int pageSize)
        {
            return FindPage(page, pageSize).ToList();
        }

        public override int GetCount()
        {
            return FindAll().Count();
        }

        public int GetCount(Expression<Func<T, bool>> predicate)
        {
            return FindAll().Count(predicate);
        }


        public void Attach(T obj)
        {
            set.Attach(obj);
        }

        public override void Add(T newEntity)
        {
            set.Add(newEntity);
        }

        public override void AddRange(IEnumerable<T> newEntities)
        {
            set.AddRange(newEntities);
        }


        public override void Delete(T entity)
        {
            set.Remove(entity);
        }

        public override void DeleteRange(IEnumerable<T> entities)
        {
            set.RemoveRange(entities);
        }


        public override T FindById(int id)
        {
            return FindByKeyValues(id);
        }

        public T FindByKeyValues(params object[] keyValues)
        {
            return set.Find(keyValues);
        }

        public override List<T> GetAll()
        {
            return FindAll().ToList();
        }

        public override string GetQuery()
        {
            return ctx.GetQuery<T>();
        }

        public override List<T> GetByQuery(string query)
        {
            return set.SqlQuery(query).ToList();
        }

        public override DataTable GetDataTableByWhere(string where)
        {
            var list = GetByWhere(where);

            return ReflectionUtils.CreateDataTable(list);
        }

        public override DataTable GetDataTableByQuery(string query)
        {
            return DbLogWrapper.Execute(ctx.DataTable, query, ConnectionString, logger, IsLoggingEnabled);
        }


        public override int Commit()
        {
            return ctx.SaveChangesWithModificationHistory(); //Zwraca ilość wierszy wziętych po uwagę
        }
    }
}
