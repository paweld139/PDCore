using FTCore.CoreLibrary.AttributeApi;
using FTCore.CoreLibrary.SQLLibrary;
using Microsoft.VisualBasic.Logging;
using PDCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PDCore.Helpers
{
    public class WebSqlHelper : LocalSqlHelper
    {
        public WebSqlHelper(string connectionString) : base(connectionString) { }

        private ILogger Logger;

        public void SaveChanges<T>(List<T> list) where T : Attributable, new()
        {
            Savator.SaveObjectList(list, this);
        }

        public void SaveChanges<T>(T obj) where T : Attributable, new()
        {
            Savator.SaveObject(obj, this);
        }

        private Stopwatch _stopWatch;
        private Stopwatch StopWatch
        {
            get
            {
                if (_stopWatch == null)
                {
                    _stopWatch = new Stopwatch();
                }

                return _stopWatch;
            }
        }

        private void StartWatching()
        {
            if (IsLoggingEnabled)
            {
                StopWatch.Start();
            }
        }

        private void StopWatching(string query)
        {
            if (IsLoggingEnabled)
            {
                StopWatch.Stop();

                string message = string.Format("{5}GetDataTable [{0}][{1}]{2} {3} [{4} ms]{5}",
                    DateTime.Now, base.ConnectionString, string.Empty/*Environment.NewLine + Environment.StackTrace*/,
                    (Environment.NewLine + query), StopWatch.ElapsedMilliseconds, Environment.NewLine);

                Logger.Log(message);

                StopWatch.Reset();
            }
        }

        public new DataTable GetDataTable(string query)
        {
            StartWatching();

            DataTable result = base.GetDataTable(query);

            StopWatching(query);


            return result;
        }

        public DataTable GetDataTable<T>(T o, string where) where T : Attributable, new()
        {
            return Savator.GetDataTable(o, where, this);
        }

        public new int ExecuteSQLQuery(string query)
        {
            StartWatching();

            int result = base.ExecuteSQLQuery(query);

            StopWatching(query);

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

        public List<T> Load<T>(string where) where T : Attributable, new()
        {
            return Savator.FillObjectList(new T(), where, this);
        }

        public void Delete<T>(T obj) where T : Attributable, new()
        {
            Savator.DeleteObject(obj, this);
        }

        public void Delete<T>(List<T> list) where T : Attributable, new()
        {
            list.ForEach(Delete);
        }

        public bool IsLoggingEnabled { get; private set; }

        public void SetLogging(bool res, ILogger logger)
        {
            if (res == IsLoggingEnabled)
            {
                return;
            }

            IsLoggingEnabled = res;

            if (res)
            {
                Logger = logger;
            }
            else
            {
                Logger = null;
            }
        }
    }
}
