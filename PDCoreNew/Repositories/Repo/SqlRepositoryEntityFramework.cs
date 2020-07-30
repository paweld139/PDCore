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
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

        public override IEnumerable<T> GetAll()
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

        protected async Task<int> DoCommitAsClientWins(bool sync, Func<Task<int>> commitAsync)
        {
            bool saved = false;

            int result = 0;

            do
            {
                try
                {
                    if (sync)
                        result = Commit(); //Zwraca ilość wierszy wziętych po uwagę
                    else
                        result = await commitAsync();

                    saved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        var databaseValues = entry.GetDatabaseValues();

                        entry.OriginalValues.SetValues(databaseValues);
                    }
                }
            }
            while (!saved);

            return result;
        }

        public int CommitAsClientWins()
        {
            return DoCommitAsClientWins(true, null).Result;
        }

        public async Task<int> DoCommitAsDatabaseWins(bool sync, Func<Task<int>> commitAsync)
        {
            bool saved = false;

            int result = 0;

            do
            {
                try
                {
                    if (sync)
                        result = Commit(); //Zwraca ilość wierszy wziętych po uwagę
                    else
                        result = await commitAsync();

                    saved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (sync)
                            entry.Reload();
                        else
                            await entry.ReloadAsync();
                    }
                }
            }
            while (!saved);

            return result;
        }

        public int CommitAsDatabaseWins()
        {
            return DoCommitAsDatabaseWins(true, null).Result;
        }

        protected async Task<int> DoCommitWithOptimisticConcurrency(bool sync, Func<Task<int>> commitAsync)
        {
            bool saved = false;

            int result = 0;

            do
            {
                try
                {
                    if (sync)
                        result = Commit(); //Zwraca ilość wierszy wziętych po uwagę
                    else
                        result = await commitAsync();

                    saved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        var currentValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        // Choose an initial set of resolved values. In this case we
                        // make the default be the values currently in the database.
                        var resolvedValues = currentValues.Clone();

                        // Have the user choose what the resolved values should be
                        HaveUserResolveConcurrency?.Invoke(currentValues, databaseValues, resolvedValues);

                        // Update the original values with the database values and
                        // the current values with whatever the user choose.
                        entry.OriginalValues.SetValues(databaseValues);
                        entry.CurrentValues.SetValues(resolvedValues);
                    }
                }
            }
            while (!saved);

            return result;
        }

        public int CommitWithOptimisticConcurrency()
        {
            return DoCommitWithOptimisticConcurrency(true, null).Result;
        }

        /// <summary>
        /// DbPropertyValues currentValues, DbPropertyValues databaseValues, DbPropertyValues resolvedValues
        /// </summary>
        public static event Action<DbPropertyValues, DbPropertyValues, DbPropertyValues> HaveUserResolveConcurrency;

        public virtual void DeleteAndCommit(T entity)
        {
            Delete(entity);

            Commit();
        }

        protected async Task<bool> DoDeleteAndCommitWithOptimisticConcurrency(T entity, Action<string, string> writeError, bool sync, Func<Task<int>> commitAsync)
        {
            Delete(entity);

            bool result = false;

            try
            {
                if (sync)
                    Commit();
                else
                    await commitAsync();

                result = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();

                var databaseEntry = entry.GetDatabaseValues();

                if (databaseEntry == null)
                {
                    result = true;
                }
                else
                {
                    writeError(string.Empty, "The record you attempted to delete "
                        + "was modified by another user after you got the original values. "
                        + "The delete operation was canceled and the current values in the "
                        + "database have been displayed. If you still want to delete this "
                        + "record, click the Delete button again. Otherwise "
                        + "click the Back to List hyperlink.");

                    if (sync)
                        entry.Reload();
                    else
                        await entry.ReloadAsync();
                }
            }
            catch (DataException dex)
            {
                logger.Error("An error occurred while trying to delete the entity", dex);

                writeError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
            }

            return result;
        }

        public bool DeleteAndCommitWithOptimisticConcurrency(T entity, Action<string, string> writeError)
        {
            return DoDeleteAndCommitWithOptimisticConcurrency(entity, writeError, true, null).Result;
        }
    }
}
