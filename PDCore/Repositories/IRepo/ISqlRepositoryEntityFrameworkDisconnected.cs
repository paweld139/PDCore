using PDCore.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PDCore.Repositories.IRepo
{
    public interface ISqlRepositoryEntityFrameworkDisconnected<T> : ISqlRepositoryEntityFrameworkAsync<T> where T : class, IModificationHistory
    {
        void SaveNew(T entity);

        Task SaveNewAsync(T entity);

        Task SaveNewAsync<TInput>(TInput input);


        void SaveUpdated(T entity);

        bool SaveUpdatedWithOptimisticConcurrency(T entity, Action<string, string> writeError, bool update, bool? include = null, params Expression<Func<T, object>>[] properties);

        Task SaveUpdatedAsync(T entity);

        Task<bool> SaveUpdatedWithOptimisticConcurrencyAsync(T entity, Action<string, string> writeError, bool update, bool? include = null, params Expression<Func<T, object>>[] properties);


        void Delete(params object[] keyValues);


        void DeleteAndCommit(params object[] keyValues);

        Task DeleteAndCommitAsync(params object[] keyValues);

        void Update(T entity, IHasRowVersion dto);
    }
}
