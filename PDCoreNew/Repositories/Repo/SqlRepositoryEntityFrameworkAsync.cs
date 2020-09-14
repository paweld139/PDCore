using AutoMapper;
using PDCore.Interfaces;
using PDCore.Repositories.IRepo;
using PDCore.Utils;
using PDCoreNew.Context.IContext;
using PDCoreNew.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PDCoreNew.Repositories.Repo
{
    public class SqlRepositoryEntityFrameworkAsync<T> : SqlRepositoryEntityFramework<T>, ISqlRepositoryEntityFrameworkAsync<T> where T : class, IModificationHistory
    {
        public SqlRepositoryEntityFrameworkAsync(IEntityFrameworkDbContext ctx, ILogger logger, IMapper mapper) : base(ctx, logger, mapper)
        {
        }

        public virtual Task<T> FindByIdAsync(int id)
        {
            return set.FindAsync(id);
        }

        public virtual Task<List<T>> GetByQueryAsync(string query)
        {
            return set.SqlQuery(query).ToListAsync();
        }

        public virtual Task<List<T>> GetByWhereAsync(string where)
        {
            string query = GetQuery(where);

            return GetByQueryAsync(query);
        }

        public virtual Task<List<T>> GetAllAsync(bool asNoTracking)
        {
            return FindAll(asNoTracking).ToListAsync();
        }

        public virtual Task<List<T>> GetAllAsync()
        {
            return FindAll().ToListAsync();
        }

        public virtual Task<List<KeyValuePair<TKey, TValue>>> GetKeyValuePairsAsync<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TValue> valueSelector, bool sortByValue = true) where TValue : IComparable<TValue>
        {
            return FindKeyValuePairs(keySelector, valueSelector, sortByValue).ToListAsync();
        }

        public virtual Task<List<T>> GetByFilterAsync(Converter<T, string> converter, string substring)
        {
            return FindByFilter(converter, substring).ToListAsync();
        }

        public virtual Task<List<T>> GetPageAsync(int page, int pageSize)
        {
            return FindPage(page, pageSize).ToListAsync();
        }

        public virtual Task<int> GetCountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
                return FindAll().CountAsync(predicate);
            else
                return FindAll().CountAsync();
        }


        public virtual Task<int> CommitAsync()
        {
            return ctx.SaveChangesWithModificationHistoryAsync(); //Zwraca ilość wierszy wziętych po uwagę
        }

        public virtual Task DeleteAndCommitAsync(T entity)
        {
            Delete(entity);

            return CommitAsync();
        }

        public virtual Task<int> CommitAsClientWinsAsync()
        {
            return DoCommitAsClientWins(false, CommitAsync);
        }

        public virtual Task<int> CommitAsDatabaseWinsAsync()
        {
            return DoCommitAsDatabaseWins(false, CommitAsync);
        }

        public virtual Task<int> CommitWithOptimisticConcurrencyAsync()
        {
            return DoCommitWithOptimisticConcurrency(false, CommitAsync);
        }

        public virtual Task<bool> DeleteAndCommitWithOptimisticConcurrencyAsync(T entity, Action<string, string> writeError)
        {
            return DoDeleteAndCommitWithOptimisticConcurrency(entity, writeError, false, CommitAsync);
        }

        public virtual Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return Find(predicate).ToListAsync();
        }

        public Task<List<TOutput>> GetAsync<TOutput>(Expression<Func<T, bool>> predicate)
        {
            return Find<TOutput>(predicate).ToListAsync();
        }

        public Task<TOutput> FindByIdAsync<TOutput>(int id)
        {
            return Find<TOutput>(GetByIdPredicate(id)).SingleOrDefaultAsync();
        }
    }
}
