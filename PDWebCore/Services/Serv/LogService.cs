using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDWebCore.Models;
using PDCore.Utils;
using PDWebCore.Repositories.Repo;
using PDWebCore.Context.IContext;
using PDWebCore.Helpers;
using System.Threading.Tasks;
using PDWebCore.Helpers.ExceptionHandling;
using System.Web;
using PDCore.Enums;
using log4net.Core;
using System.IO;
using PDCoreNew.Loggers;
using PDWebCore.Loggers;

namespace PDWebCore.Services.Serv
{
    public static class LogService
    {
        private static Type DbContext;
        private static readonly FileLogger fileLogger;

        static LogService()
        {
            fileLogger = new FileLogger();
        }

        public static void EnableLogInDb<T>() where T : IMainDbContext, new()
        {
            DbContext = typeof(T);
        }

        public static bool IsEnabledLogInDb => DbContext != null;

        public static void Log(string message, Exception exception, LogType logType)
        {
            DoLogAsync(message, exception, logType, true).Wait();
        }

        public static Task LogAsync(string message, Exception exception, LogType logType)
        {
            return DoLogAsync(message, exception, logType, false);
        }

        public static void Log(Exception exception, LogType logType)
        {
            Log(string.Empty, exception, logType);
        }

        public static Task LogAsync(Exception exception, LogType logType)
        {
            return LogAsync(string.Empty, exception, logType);
        }

        public static void Log(string message, LogType logType)
        {
            Log(message, null, logType);
        }

        public static Task LogAsync(string message, LogType logType)
        {
            return LogAsync(message, null, logType);
        }

        private async static Task DoLogAsync(string message, Exception exception, LogType logType, bool sync)
        {
            if (IsEnabledLogInDb)
            {
                using (var dbContext = (IMainDbContext)Activator.CreateInstance(DbContext))
                {
                    using (var logRepository = new LogRepo(dbContext, null))
                    {
                        using (var sqlServerLogger = new SqlServerLogger(logRepository))
                        {
                            if (sync)
                                sqlServerLogger.Log(message, exception, logType);
                            else
                                await sqlServerLogger.LogAsync(message, exception, logType);
                        }
                    }
                }
            }

            fileLogger.Log(message, exception, logType);
        }
    }
}
