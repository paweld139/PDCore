using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PDCore.Repositories.IRepo
{
    public interface ISqlRepository<T> : IRepository<T>
    {
        bool IsLoggingEnabled { get; }

        void SetLogging(bool res);

        List<T> GetByWhere(string where);

        DataTable GetDataTableByWhere(string where);
    }
}
