using AutoMapper;
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
using System.Windows.Navigation;

namespace PDCoreNew.Repositories.Repo
{
    public class SqlRepositoryEntityFramework<T> : SqlRepository<T>, ISqlRepositoryEntityFramework<T> where T : class, IModificationHistory
    {
        protected readonly IEntityFrameworkDbContext ctx;
        protected readonly IMapper mapper;
        protected readonly DbSet<T> set;

        protected override string ConnectionString => ctx.Database.Connection.ConnectionString;

        public SqlRepositoryEntityFramework(IEntityFrameworkDbContext ctx, ILogger logger, IMapper mapper) : base(ctx, logger)
        {
            this.ctx = ctx;
            this.mapper = mapper;
            set = this.ctx.Set<T>();
        }

        public override IQueryable<T> FindAll()
        {
            return FindAll(false);
        }

        public virtual IQueryable<T> FindAll(bool asNoTracking)
        {
            if (asNoTracking)
                return set.AsNoTracking();

            return set;
        }

        //public virtual IQueryable<KeyValuePair<TKey, TValue>> FindKeyValuePairs<TKey, TValue>(Expression<Func<T, TKey>> keySelector, Expression<Func<T, TValue>> valueSelector, bool sortByValue = true) where TValue : IComparable<TValue>
        //{
        //    var query = FindAll().GetKVP(keySelector, valueSelector);

        //    if (sortByValue)
        //        query = query.OrderBy(e => e.Value);

        //    return query;
        //}

        public virtual IQueryable<T> FindByFilter(Expression<Func<T, string>> propertySelector, string substring)
        {
            var query = FindAll();

            if (!string.IsNullOrWhiteSpace(substring))
                query = query.Filter(substring, propertySelector);

            return query;
        }

        public virtual IQueryable<TOutput> FindByFilter<TOutput>(Expression<Func<T, string>> propertySelector, string substring)
        {
            return mapper.ProjectTo<TOutput>(FindByFilter(propertySelector, substring));
        }

        public virtual IQueryable<T> FindPage(int page, int pageSize)
        {
            var query = FindAll();

            if (page > 0 && pageSize > 0)
            {
                query = query.OrderByDescending(e => e.DateCreated).GetPage(page, pageSize);
            }

            return query;
        }

        public virtual IQueryable<TOutput> FindPage<TOutput>(int page, int pageSize)
        {
            return mapper.ProjectTo<TOutput>(FindPage(page, pageSize));
        }

        public virtual IQueryable<T> FindByDateCreated(string dateF, string dateT)
        {
            return FindAll().FindByDateCreated(dateF, dateT);
        }

        public virtual IQueryable<T> FindByDateCreated(DateTime? dateF, DateTime? dateT)
        {
            return FindAll().FindByDateCreated(dateF, dateT);
        }

        public virtual IQueryable<TOutput> FindByDateCreated<TOutput>(DateTime? dateF, DateTime? dateT)
        {
            return mapper.ProjectTo<TOutput>(FindByDateCreated(dateF, dateT));
        }

        public virtual IQueryable<T> FindByDateModified(string dateF, string dateT)
        {
            return FindAll().FindByDateModified(dateF, dateT);
        }

        public virtual IQueryable<T> FindByDateModified(DateTime? dateF, DateTime? dateT)
        {
            return FindAll().FindByDateModified(dateF, dateT);
        }


        public virtual IEnumerable<T> GetAll(bool asNoTracking)
        {
            return FindAll(asNoTracking).ToList();
        }

        public virtual IEnumerable<KeyValuePair<TKey, TValue>> GetKeyValuePairs<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TValue> valueSelector, bool sortByValue = true) where TValue : IComparable<TValue>
        {
            var query = GetAll().GetKVP(keySelector, valueSelector);

            if (sortByValue)
            {
                query = query.OrderBy(e => e.Value);
            }

            return query;
        }

        public virtual IEnumerable<T> GetByFilter(Expression<Func<T, string>> propertySelector, string substring)
        {
            return FindByFilter(propertySelector, substring).ToList();
        }

        public virtual IEnumerable<T> GetPage(int page, int pageSize)
        {
            return FindPage(page, pageSize).ToList();
        }

        public override int GetCount()
        {
            return FindAll().Count();
        }

        public virtual int GetCount(Expression<Func<T, bool>> predicate)
        {
            return FindAll().Count(predicate);
        }


        public virtual void Attach(T obj)
        {
            set.Attach(obj);
        }

        public override void Add(T newEntity)
        {
            //DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            //if (dbEntityEntry.State != EntityState.Detached)
            //{
            //    dbEntityEntry.State = EntityState.Added;
            //}
            //else
            //{
            //    DbSet.Add(entity);
            //}

            set.Add(newEntity);
        }

        public override void AddRange(IEnumerable<T> newEntities)
        {
            set.AddRange(newEntities);
        }

        public override void Update(T entity)
        {
            //DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            //if (dbEntityEntry.State == EntityState.Detached)
            //{
            //    DbSet.Attach(entity);
            //}
            //dbEntityEntry.State = EntityState.Modified;

            ctx.Entry(entity).State = EntityState.Modified;
            ctx.Entry(entity).Property(e => e.RowVersion).IsModified = false;
        }


        public override void Delete(T entity)
        {
            //DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            //if (dbEntityEntry.State != EntityState.Deleted)
            //{
            //    dbEntityEntry.State = EntityState.Deleted;
            //}
            //else
            //{
            //    DbSet.Attach(entity);
            //    DbSet.Remove(entity);
            //}

            set.Remove(entity);
        }

        public override void DeleteRange(IEnumerable<T> entities)
        {
            set.RemoveRange(entities);
        }


        public override T FindById(int id)
        {
            //return DbSet.FirstOrDefault(PredicateBuilder.GetByIdPredicate<T>(id));
            return FindByKeyValues(id);
        }

        public virtual T FindByKeyValues(params object[] keyValues)
        {
            return set.Find(keyValues);
        }

        public override IEnumerable<T> GetAll()
        {
            return FindAll().ToList();
        }

        public virtual IEnumerable<TOutput> GetAll<TOutput>()
        {
            return FindAll<TOutput>().ToList();
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

        public virtual int CommitWithoutValidation()
        {
            bool validate = ctx.Configuration.ValidateOnSaveEnabled;

            if (validate)
                ctx.Configuration.ValidateOnSaveEnabled = false;

            int result = Commit();

            if (validate)
                ctx.Configuration.ValidateOnSaveEnabled = validate;

            return result;
        }

        protected virtual async Task<int> DoCommitAsClientWins(bool sync, Func<Task<int>> commitAsync)
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

        public virtual int CommitAsClientWins()
        {
            return DoCommitAsClientWins(true, null).Result;
        }

        protected virtual async Task<int> DoCommitAsDatabaseWins(bool sync, Func<Task<int>> commitAsync)
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

        public virtual int CommitAsDatabaseWins()
        {
            return DoCommitAsDatabaseWins(true, null).Result;
        }

        protected virtual async Task<int> DoCommitWithOptimisticConcurrency(bool sync, Func<Task<int>> commitAsync)
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

        public virtual int CommitWithOptimisticConcurrency()
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

        protected virtual async Task<bool> DoDeleteAndCommitWithOptimisticConcurrency(T entity, Action<string, string> writeError, bool sync, Func<Task<int>> commitAsync)
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
                        + "record, delete again.");

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

        public virtual bool DeleteAndCommitWithOptimisticConcurrency(T entity, Action<string, string> writeError)
        {
            return DoDeleteAndCommitWithOptimisticConcurrency(entity, writeError, true, null).Result;
        }

        public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return FindAll().Where(predicate);
        }

        public virtual IQueryable<TOutput> FindBy<TOutput>(Expression<Func<T, bool>> predicate, Expression<Func<T, TOutput>> columns)
        {
            return Find(predicate).Select(columns);
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return Find(predicate).ToList();
        }

        public virtual bool Exists(int id)
        {
            var predicate = RepositoryUtils.GetByIdPredicate<T>(id);

            return FindAll().Any(predicate);
        }

        public virtual IQueryable<TOutput> FindAll<TOutput>()
        {
            return mapper.ProjectTo<TOutput>(FindAll());
        }

        public virtual IQueryable<TOutput> FindAll<TOutput>(bool asNoTracking)
        {
            return mapper.ProjectTo<TOutput>(FindAll(asNoTracking));
        }

        public virtual IQueryable<TOutput> Find<TOutput>(Expression<Func<T, bool>> predicate)
        {
            return mapper.ProjectTo<TOutput>(Find(predicate));
        }

        public Expression<Func<T, bool>> GetByIdPredicate(int id) => RepositoryUtils.GetByIdPredicate<T>(id);

        public virtual bool UpdateWithIncludeOrExcludeProperties(T item, bool include, IEnumerable<string> propertyNames)
        {
            if (include)
            {
                Attach(item);
            }
            else
            {
                Update(item);
            }

            var entry = ctx.Entry(item);

            foreach (var propertyName in propertyNames)
            {
                // If we can't find the property, this line wil throw an exception, 
                //which is good as we want to know about it
                entry.Property(propertyName).IsModified = include;
            }

            return true;
        }

        public virtual bool UpdateWithIncludeOrExcludeProperties(T item, bool include, params string[] propertyNames)
        {
            return UpdateWithIncludeOrExcludeProperties(item, include, propertyNames.AsEnumerable());
        }

        public virtual bool UpdateWithIncludeOrExcludeProperties(T item, bool include, IEnumerable<Expression<Func<T, object>>> properties)
        {
            var changedPropertyNames = properties.ToArray(ReflectionUtils.GetNameOf);

            return UpdateWithIncludeOrExcludeProperties(item, include, changedPropertyNames);
        }

        public virtual bool UpdateWithIncludeOrExcludeProperties(T item, bool include, params Expression<Func<T, object>>[] properties)
        {
            return UpdateWithIncludeOrExcludeProperties(item, include, properties.AsEnumerable());
        }

        public virtual bool UpdateWithIncludeOrExcludeProperties(IHasRowVersion source, T destination, bool include, ICollection<string> propertyNames)
        {
            var destinationEntry = ctx.Entry(destination);

            destinationEntry.Property(e => e.RowVersion).OriginalValue = source.RowVersion;

            var sourceProperties = source.GetProperties();

            foreach (var property in sourceProperties)
            {
                string propertyName = property.Name;

                bool contains = propertyNames.Contains(propertyName);

                if ((contains && include) || (!contains && !include))
                {
                    var sourceValue = property.GetPropertyValue(source);

                    destination.SetPropertyValue(propertyName, sourceValue);
                }
            }

            return true;
        }

        public virtual bool UpdateWithIncludeOrExcludeProperties(IHasRowVersion source, T destination, bool include, params Expression<Func<T, object>>[] properties)
        {
            var propertyNames = properties.ConvertArray(ReflectionUtils.GetNameOf);

            return UpdateWithIncludeOrExcludeProperties(source, destination, include, propertyNames);
        }
    }
}
