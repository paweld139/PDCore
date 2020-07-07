using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Repositories.IRepo
{
    public interface IReadOnlyRepository<out T> : IDisposable
    {
        T FindById(int id);

        IQueryable<T> FindAll();
    }
}
