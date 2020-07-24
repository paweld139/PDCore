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
    public interface ISqlRepositoryEntityFramework<T> : ISqlRepository<T> where T : class, new()
    {
        Task<List<T>> GetAllAsync(bool asNoTracking = true);

        IQueryable<T> FindAll(bool asNoTracking);

        Task<int> CommitAsync();

        Task<T> FindByIdAsync(int id);

        void Attach(T obj);

        Task<int> GetCountAsync(Expression<Func<T, bool>> predicate = null);

        int GetCount(Expression<Func<T, bool>> predicate);

        List<T> GetAll(bool asNoTracking);

        T Add();

        ObservableCollection<T> GetAllFromMemory();

        void DeleteAndCommit(T entity);

        Task DeleteAndCommitAsync(T entity);

        Task<ObservableCollection<T>> GetAllFromMemoryAsync();

        Task<List<T>> GetByQueryAsync(string query);

        Task<List<T>> GetByWhereAsync(string where);
    }
}
