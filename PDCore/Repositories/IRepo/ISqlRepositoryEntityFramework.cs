using PDCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Repositories.IRepo
{
    public interface ISqlRepositoryEntityFramework<T> : ISqlRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(bool asNoTracking = true);

        IQueryable<T> FindAll(bool asNoTracking);

        Task<int> CommitAsync();

        Task<T> FindByIdAsync(int id);

        void Attach(T obj);

        Task<int> GetCountAsync(Expression<Func<T, bool>> predicate = null);

        int GetCount(Expression<Func<T, bool>> predicate);
    }
}
