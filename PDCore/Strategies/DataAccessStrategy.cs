using PDCore.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace PDCore.Strategies
{
    public abstract class DataAccessStrategy<TEntity> : IDataAccessStrategy<TEntity>
    {
        public abstract bool CanAdd(params object[] args);
        public abstract void PrepareForAdd(params object[] args);

        public abstract bool CanUpdate(TEntity entity);
        public abstract bool CanUpdateAllProperties(TEntity entity);
        public abstract ICollection<string> GetPropertiesForUpdate(TEntity entity);

        public abstract bool CanDelete(TEntity entity);

        public virtual IQueryable<TEntity> PrepareQuery(IQueryable<TEntity> entities) => entities;

        protected virtual bool NoRestrictions() => false;
    }
}
