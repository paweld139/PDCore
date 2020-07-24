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
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Repositories.Repo
{
    public class SqlRepositoryEntityFramework<T> : SqlRepository<T>, ISqlRepositoryEntityFramework<T> where T : class
    {
        private readonly IEntityFrameworkDbContext ctx;
        private readonly DbSet<T> set;

        protected override string ConnectionString => ctx.Database.Connection.ConnectionString;

        public SqlRepositoryEntityFramework(IEntityFrameworkDbContext ctx, ILogger logger) : base(ctx, logger)
        {
            this.ctx = ctx;
            set = this.ctx.Set<T>();
        }

        public override void Add(T newEntity)
        {
            set.Add(newEntity);
        }

        public override void AddRange(IEnumerable<T> newEntities)
        {
            set.AddRange(newEntities);
        }

        public void Attach(T obj)
        {
            set.Attach(obj);
        }

        public override int Commit()
        {
            return ctx.SaveChangesWithModificationHistory(); //Zwraca ilość wierszy wziętych po uwagę
        }

        public Task<int> CommitAsync()
        {
            return ctx.SaveChangesWithModificationHistoryAsync();
        }

        public override void Delete(T entity)
        {
            set.Remove(entity);
        }

        public override void DeleteRange(IEnumerable<T> entities)
        {
            set.RemoveRange(entities);
        }

        public IQueryable<T> FindAll(bool asNoTracking)
        {
            if (asNoTracking)
                return set.AsNoTracking();

            return set;
        }

        public override IQueryable<T> FindAll()
        {
            return FindAll(true);
        }

        public override T FindById(int id)
        {
            return set.Find(id);
        }

        public Task<T> FindByIdAsync(int id)
        {
            return set.FindAsync(id);
        }

        public Task<List<T>> GetAllAsync(bool asNoTracking = true)
        {
            return FindAll(asNoTracking).ToListAsync();
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

        public Task<int> GetCountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
                return FindAll().CountAsync(predicate);
            else
                return FindAll().CountAsync();
        }

        public override string GetQuery()
        {
            return ctx.GetQuery<T>();
        }

        public int GetCount(Expression<Func<T, bool>> predicate)
        {
            return FindAll().Count(predicate);
        }

        public override int GetCount()
        {
            return FindAll().Count();
        }
    }
}
