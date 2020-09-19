using System.Collections.Generic;
using System.Linq;

namespace PDCore.Interfaces
{
    public interface IDataAccessStrategy<TEntity>
    {
        bool CanUpdate(TEntity entity);

        bool CanUpdateAllProperties(TEntity entity);

        ICollection<string> GetPropertiesForUpdate(TEntity entity);


        bool CanDelete(TEntity entity);


        bool CanAdd(params object[] args);

        void PrepareForAdd(params object[] args);


        IQueryable<TEntity> PrepareQuery(IQueryable<TEntity> entities);
    }
}
