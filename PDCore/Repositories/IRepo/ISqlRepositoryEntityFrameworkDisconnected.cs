using PDCore.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Threading.Tasks;

namespace PDCore.Repositories.IRepo
{
    public interface ISqlRepositoryEntityFrameworkDisconnected<T> : ISqlRepositoryEntityFrameworkAsync<T> where T : class, IModificationHistory
    {
        void SaveNew(T entity);

        Task SaveNewAsync(T entity);

        Task SaveNewAsync<TInput>(TInput input);

        Task<bool> SaveNewAsync<TInput>(TInput input, IDataAccessStrategy<T> savingStrategy, IPrincipal principal, params object[] args);


        void SaveUpdated(T entity);

        bool SaveUpdatedWithOptimisticConcurrency(T entity, Action<string, string> writeError, bool update = true, bool? include = null, params Expression<Func<T, object>>[] properties);

        Task SaveUpdatedAsync(T entity);

        Task<bool> SaveUpdatedWithOptimisticConcurrencyAsync(T entity, Action<string, string> writeError, bool update = true, bool? include = null, params Expression<Func<T, object>>[] properties);


        void Delete(params object[] keyValues);


        void DeleteAndCommit(params object[] keyValues);

        Task DeleteAndCommitAsync(params object[] keyValues);

        void Update(T entity, IHasRowVersion dto);

        Task<bool> SaveUpdatedWithOptimisticConcurrencyAsync(T entity, IDataAccessStrategy<T> savingStrategy, IPrincipal principal, Action<string, string> writeError);

        Task<TOutput> SaveUpdatedWithOptimisticConcurrencyAsync<TOutput>(IHasRowVersion source, T destination, IDataAccessStrategy<T> savingStrategy, IPrincipal principal, Action<string, string> writeError);
    }
}
