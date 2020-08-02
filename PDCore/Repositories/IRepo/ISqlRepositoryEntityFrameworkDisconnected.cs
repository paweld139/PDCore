using PDCore.Interfaces;
using System;
using System.Threading.Tasks;

namespace PDCore.Repositories.IRepo
{
    public interface ISqlRepositoryEntityFrameworkDisconnected<T> : ISqlRepositoryEntityFrameworkAsync<T> where T : class, IModificationHistory
    {
        void SaveNew(T entity);

        Task SaveNewAsync(T entity);


        void SaveUpdated(T entity);

        bool SaveUpdatedWithOptimisticConcurrency(T entity, Action<string, string> writeError);

        Task SaveUpdatedAsync(T entity);

        Task<bool> SaveUpdatedWithOptimisticConcurrencyAsync(T entity, Action<string, string> writeError);


        void Delete(params object[] keyValues);


        void DeleteAndCommit(params object[] keyValues);

        Task DeleteAndCommitAsync(params object[] keyValues);
    }
}
