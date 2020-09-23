using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDCore.Interfaces
{
    public interface IDataAccessStrategy<TEntity>
    {
        bool CanUpdate(TEntity entity);

        bool CanUpdateAllProperties(TEntity entity);

        ICollection<string> GetPropertiesForUpdate(TEntity entity);


        bool CanDelete(TEntity entity);


        Task<bool> CanAdd(params object[] args);

        void PrepareForAdd(params object[] args);


        IQueryable<TEntity> PrepareQuery(IQueryable<TEntity> entities);
    }
}
