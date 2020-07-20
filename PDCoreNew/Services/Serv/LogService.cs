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


        public static void Log(string message, LogType logType)
        {
            Log(message, null, logType);
        }

        public static void Log(Exception exception, LogType logType)
        {
            Log(string.Empty, exception, logType);
        }


        public static void Log(string message)
        {
            Log(message, null, LogType.Info);
        }

        public static void Log(Exception exception)
        {
            Log(string.Empty, exception, LogType.Info);
        }


        public static Task LogAsync(string message, Exception exception, LogType logType)
        {
            return DoLogAsync(message, exception, logType, false);
        }


        public static Task LogAsync(string message, LogType logType)
        {
            return LogAsync(message, null, logType);
        }

        public static Task LogAsync(Exception exception, LogType logType)
        {
            return LogAsync(string.Empty, exception, logType);
        }


        public static Task LogAsync(string message)
        {
            return LogAsync(message, null, LogType.Info);
        }

        public static Task LogAsync(Exception exception)
        {
            return LogAsync(string.Empty, exception, LogType.Info);
        }


        public static void Debug(string message, Exception exception)
        {
            Log(message, exception, LogType.Debug);
        }

        public static void Error(string message, Exception exception)
        {
            Log(message, exception, LogType.Error);
        }

        public static void Fatal(string message, Exception exception)
        {
            Log(message, exception, LogType.Fatal);
        }

        public static void Info(string message, Exception exception)
        {
            Log(message, exception, LogType.Info);
        }

        public static void Warn(string message, Exception exception)
        {
            Log(message, exception, LogType.Warn);
        }


        public static void Debug(string message)
        {
            Log(message, null, LogType.Debug);
        }

        public static void Error(string message)
        {
            Log(message, null, LogType.Error);
        }

        public static void Fatal(string message)
        {
            Log(message, null, LogType.Fatal);
        }

        public static void Info(string message)
        {
            Log(message, null, LogType.Info);
        }

        public static void Warn(string message)
        {
            Log(message, null, LogType.Warn);
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
