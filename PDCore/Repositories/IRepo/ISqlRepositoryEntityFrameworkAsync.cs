using PDCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PDCore.Repositories.IRepo
{
    public interface ISqlRepositoryEntityFrameworkAsync<T> : ISqlRepositoryEntityFramework<T> where T : class, IModificationHistory
    {
        Task<T> FindByIdAsync(long id);

        Task<T> FindByIdAsync(long id, bool asNoTracking);

        Task<List<T>> GetByQueryAsync(string query);

        Task<List<T>> GetByWhereAsync(string where);

        Task<List<T>> GetAllAsync(bool asNoTracking);

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate);

        Task<List<KeyValuePair<TKey, TValue>>> GetKeyValuePairsAsync<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TValue> valueSelector, bool sortByValue = true) where TValue : IComparable<TValue>;

        Task<List<T>> GetByFilterAsync(Expression<Func<T, string>> propertySelector, string substring);

        Task<List<T>> GetPageAsync(int page, int pageSize);

        Task<int> GetCountAsync(Expression<Func<T, bool>> predicate = null);

        Task<List<TOutput>> GetAsync<TOutput>(Expression<Func<T, bool>> predicate);

        Task<TOutput> FindByIdAsync<TOutput>(long id);


        Task<int> CommitAsync();

        Task DeleteAndCommitAsync(T entity);

        Task<int> CommitAsClientWinsAsync();

        Task<int> CommitAsDatabaseWinsAsync();

        Task<int> CommitWithOptimisticConcurrencyAsync();

        Task<bool> DeleteAndCommitWithOptimisticConcurrencyAsync(T entity, Action<string, string> writeError);
    }
}
