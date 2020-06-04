using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PDCore.Repositories.IRepo
{
    public interface IRepo<T> : IDisposable where T : class
    {
        void Save(List<T> list);

        void Save(T obj);

        T Load(int id);

        bool IsLoggingEnabled { get; }

        void SetLogging(bool res);

        List<T> Load(string where);

        DataTable GetDataTable(string where);

        void Delete(List<T> list);

        void Delete(T obj);
    }
}
