using PDCore.Interfaces;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PDCore.Repositories.IRepo
{
    public interface ISqlRepositoryEntityFrameworkConnected<T> : ISqlRepositoryEntityFrameworkAsync<T> where T : class, IModificationHistory, new()
    {
        ObservableCollection<T> GetAllFromMemory();

        Task<ObservableCollection<T>> GetAllFromMemoryAsync();


        T Add();


        void DeleteAndCommit(T entity);

        Task DeleteAndCommitAsync(T entity);
    }
}
