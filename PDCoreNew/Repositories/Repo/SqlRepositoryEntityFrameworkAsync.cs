using PDCore.Interfaces;
using PDCore.Repositories.IRepo;
using PDCoreNew.Context.IContext;
using PDCoreNew.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PDCoreNew.Repositories.Repo
{
    public class SqlRepositoryEntityFrameworkAsync<T> : SqlRepositoryEntityFramework<T>, ISqlRepositoryEntityFrameworkAsync<T> where T : class, IModificationHistory
    {
        public SqlRepositoryEntityFrameworkAsync(IEntityFrameworkDbContext ctx, ILogger logger) : base(ctx, logger)
        {
        }

        public Task<T> FindByIdAsync(int id)
        {
            return set.FindAsync(id);
        }

        public Task<List<T>> GetByQueryAsync(string query)
        {
            return set.SqlQuery(query).ToListAsync();
        }

        public Task<List<T>> GetByWhereAsync(string where)
        {
            string query = GetQuery(where);

            return GetByQueryAsync(query);
        }

        public Task<List<T>> GetAllAsync(bool asNoTracking)
        {
            return FindAll(asNoTracking).ToListAsync();
        }

        public Task<List<T>> GetAllAsync()
        {
            return FindAll().ToListAsync();
        }

        public Task<List<KeyValuePair<TKey, TValue>>> GetKeyValuePairsAsync<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TValue> valueSelector, bool sortByValue = true) where TValue : IComparable<TValue>
        {
            return FindKeyValuePairs(keySelector, valueSelector, sortByValue).ToListAsync();
        }

        public Task<List<T>> GetByFilterAsync(Converter<T, string> converter, string substring)
        {
            return FindByFilter(converter, substring).ToListAsync();
        }

        public Task<List<T>> GetPageAsync(int page, int pageSize)
        {
            return FindPage(page, pageSize).ToListAsync();
        }

        public Task<int> GetCountAsync(Expression<Func<T, bool>> predicate = null)
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

        public Task<int> CommitAsClientWinsAsync()
        {
            return DoCommitAsClientWins(false, CommitAsync);
        }

        public Task<int> CommitAsDatabaseWinsAsync()
        {
            return DoCommitAsDatabaseWins(false, CommitAsync);
        }

        public Task<int> CommitWithOptimisticConcurrencyAsync()
        {
            return DoCommitWithOptimisticConcurrency(false, CommitAsync);
        }

        public Task<bool> DeleteAndCommitWithOptimisticConcurrencyAsync(T entity, Action<string, string> writeError)
        {
            return DoDeleteAndCommitWithOptimisticConcurrency(entity, writeError, false, CommitAsync);
        }
    }
}
