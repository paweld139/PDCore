using FTCore.CoreLibrary.AttributeApi;
using FTCore.CoreLibrary.SQLLibrary;
using PDCore.Context.IContext;
using PDCore.Extensions;
using PDCore.Helpers.Wrappers;
using PDCore.Interfaces;
using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace PDCore.Context
{
    public class AttributableDbContext : LocalSqlHelper, IAttributableDbContext
    {
        public AttributableDbContext(string nameOrConnectionString) : base(SqlUtils.GetConnectionString(nameOrConnectionString)) { }

        private ILogger Logger;

        private T Log<T>(Func<string, T> func, string query)
        {
            return DbLogWrapper.Execute(func, query, ConnectionString, Logger, IsLoggingEnabled);
        }

        public void SaveChanges<T>(IEnumerable<T> list) where T : Attributable, new()
        {
            Savator.SaveObjectList(list.ToList(), this);
        }

        public void SaveChanges<T>(T obj) where T : Attributable, new()
        {
            Savator.SaveObject(obj, this);
        }

        public new DataTable GetDataTable(string query)
        {
            DataTable result = Log(base.GetDataTable, query);

            return result;
        }

        public DataTable GetDataTable<T>(string where) where T : Attributable, new()
        {
            return Savator.GetDataTable(new T(), where, this);
        }

        public new int ExecuteSQLQuery(string query)
        {
            int result = Log(base.ExecuteSQLQuery, query);

            return result;
        }

        public T Load<T>(int id) where T : Attributable, new()
        {
            T o = new T
            {
                ObjectID = id
            };

            Savator.FillObject(o, this);


            return o;
        }

        public List<T> LoadByWhere<T>(string where) where T : Attributable, new()
        {
            return Savator.FillObjectList(new T(), where, this);
        }

        public List<T> LoadByQuery<T>(string query) where T : Attributable, new()
        {
            return Savator.FillObjectList<T>(query, this);
        }

        public List<T> Load<T>() where T : Attributable, new()
        {
            return Savator.FillObjectList(new T(), this);
        }

        public void Delete<T>(T obj) where T : Attributable, new()
        {
            Savator.DeleteObject(obj, this);
        }

        public void Delete<T>(IEnumerable<T> list) where T : Attributable, new()
        {
            list.ForEach(Delete);
        }

        public string GetQuery<T>() where T : Attributable, new()
        {
            string query = Savator.GetSelectString(new T(), false, false);

            return query;
        }

        public bool IsLoggingEnabled => Logger != null;

        public void SetLogging(bool input, ILogger logger)
        {
            RepositoryUtils.SetLogging(input, logger, IsLoggingEnabled,
                () => Logger = logger,
                () => Logger = null
            );
        }
    }
}
