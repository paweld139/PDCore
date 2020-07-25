using PDCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Repositories.IRepo
{
    public interface ISqlRepositoryEntityFrameworkAsync<T> : ISqlRepositoryEntityFramework<T> where T : class, IModificationHistory
    {
        Task<T> FindByIdAsync(int id);

        Task<List<T>> GetByQueryAsync(string query);

        Task<List<T>> GetByWhereAsync(string where);

        Task<List<T>> GetAllAsync(bool asNoTracking);

        Task<List<T>> GetAllAsync();

        Task<List<KeyValuePair<TKey, TValue>>> GetKeyValuePairsAsync<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TValue> valueSelector, bool sortByValue = true) where TValue : IComparable<TValue>;

        Task<List<T>> GetByFilterAsync(Converter<T, string> converter, string substring);

        Task<List<T>> GetPageAsync(int page, int pageSize);

        Task<int> GetCountAsync(Expression<Func<T, bool>> predicate = null);


        Task<int> CommitAsync();        
    }

    public interface ISqlRepositoryEntityFramework<T> : ISqlRepository<T> where T : class, IModificationHistory
    {
        IQueryable<T> FindAll(bool asNoTracking);

        IQueryable<KeyValuePair<TKey, TValue>> FindKeyValuePairs<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TValue> valueSelector, bool sortByValue = true) where TValue : IComparable<TValue>;

        IQueryable<T> FindByFilter(Converter<T, string> converter, string substring);

        IQueryable<T> FindPage(int page, int pageSize);


        List<T> GetAll(bool asNoTracking);

        List<KeyValuePair<TKey, TValue>> GetKeyValuePairs<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TValue> valueSelector, bool sortByValue = true) where TValue : IComparable<TValue>;

        List<T> GetByFilter(Converter<T, string> converter, string substring);

        List<T> GetPage(int page, int pageSize);

        int GetCount(Expression<Func<T, bool>> predicate);


        void Attach(T obj);                                                                                     
    }

    public interface ISqlRepositoryEntityFrameworkConnected<T> : ISqlRepositoryEntityFrameworkAsync<T> where T : class, IModificationHistory, new()
    {
        ObservableCollection<T> GetAllFromMemory();

        Task<ObservableCollection<T>> GetAllFromMemoryAsync();


        T Add();


        void DeleteAndCommit(T entity);

        Task DeleteAndCommitAsync(T entity);
    }

    public interface ISqlRepositoryEntityFrameworkDisconnected<T> : ISqlRepositoryEntityFrameworkAsync<T> where T : class, IModificationHistory
    {
        void SaveNew(T entity);

        Task SaveNewAsync(T entity);


        void SaveUpdated(T entity);

        Task SaveUpdatedAsync(T entity);


        void Delete(int id);


        void DeleteAndCommit(int id);

        Task DeleteAndCommitAsync(int id);
    }
}
