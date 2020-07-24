using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PDCore.Repositories.IRepo
{
    public interface ISqlRepository<T> : IRepository<T>, ISqlRepository
    {
        List<T> GetByQuery(string query);

        List<T> GetByWhere(string where);

        List<T> GetAll();

        DataTable GetDataTableByWhere(string where);

        string GetQuery(string where);

        string GetQuery();

        int GetCountByWhere(string where);

        int GetCount();

        string GetTableName();
    }

    public interface ISqlRepository
    {
        bool IsLoggingEnabled { get; }

        void SetLogging(bool res);

        DataTable GetDataTableByQuery(string query);

        T GetValue<T>(string query);
    }
}
