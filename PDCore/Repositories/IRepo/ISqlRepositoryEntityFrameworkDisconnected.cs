using PDCore.Interfaces;
using System.Threading.Tasks;

namespace PDCore.Repositories.IRepo
{
    public interface ISqlRepositoryEntityFrameworkDisconnected<T> : ISqlRepositoryEntityFrameworkAsync<T> where T : class, IModificationHistory
    {
        void SaveNew(T entity);

        Task SaveNewAsync(T entity);


        void SaveUpdated(T entity);

        Task SaveUpdatedAsync(T entity);


        void Delete(int id);

        void Update(T entity);


        void DeleteAndCommit(int id);

        Task DeleteAndCommitAsync(int id);
    }
}
