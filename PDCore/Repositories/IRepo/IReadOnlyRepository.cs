using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PDCore.Repositories.IRepo
{
    public interface IReadOnlyRepository<out T> : IDisposable
    {
        T FindById(int id);

        IQueryable<T> FindAll();

        IEnumerable<T> GetAll();

        int GetCount();
    }
}
