using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PDCore.Repositories.IRepo
{
    public interface ISqlRepository<T> : IRepository<T>, ISqlRepository
    {
        List<T> GetByWhere(string where);
    }

    public interface ISqlRepository
    {
        bool IsLoggingEnabled { get; }

        void SetLogging(bool res);

        DataTable GetDataTableByWhere(string where);
    }
}
