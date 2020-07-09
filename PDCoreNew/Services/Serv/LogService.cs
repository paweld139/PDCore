using PDCore.Enums;
using PDCore.Interfaces;
using PDCoreNew.Context.IContext;
using PDCoreNew.Loggers;
using PDCoreNew.Repositories.Repo;
using System;
using System.Threading.Tasks;

namespace PDCoreNew.Services.Serv
{
    public static class LogService
    {
        private static Type DbContext;
        private static Type SqlLogger;
        private static readonly FileLogger fileLogger;

        static LogService()
        {
            fileLogger = new FileLogger();
        }

        public static void EnableLogInDb<TDbContext, TSqlLogger>() where TDbContext : IEntityFrameworkDbContext, IHasLogDbSet, new() where TSqlLogger : IAsyncLogger
        {
            DbContext = typeof(TDbContext);
            SqlLogger = typeof(TSqlLogger);
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
                using (var dbContext = (IEntityFrameworkDbContext)Activator.CreateInstance(DbContext))
                {
                    using (var logRepository = new LogRepo(dbContext))
                    {
                        using (var sqlServerLogger = (IAsyncLogger)Activator.CreateInstance(SqlLogger, logRepository))
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
