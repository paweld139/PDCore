using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Repositories.IRepo
{
    public interface IWriteOnlyRepository<in T> : IDisposable
    {
        void Add(T newEntity);

        void AddRange(IEnumerable<T> newEntities);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);

        int Commit();
    }
}
