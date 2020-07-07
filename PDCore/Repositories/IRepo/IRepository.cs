using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Repositories.IRepo
{
    public interface IRepository<T> : IReadOnlyRepository<T>, IWriteOnlyRepository<T>
    {

    }
}
