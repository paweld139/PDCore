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

namespace PDWebCore.Services.Serv
{
    public enum LogType
    {
        Debug,
        Error,
        Fatal,
        Info,
        Warn
    }

    public static class LogService
    {
        private static Type DbContext;
        
        public static void EnableLogInDb<T>() where T : IMainDbContext, new()
        {
            DbContext = typeof(T);
        }

        public static bool IsEnabledLogInDb => DbContext != null;

        public static void Log(Exception exception, LogType logType)
        {
            Log(string.Empty, exception, logType);
        }

        public static Task LogAsync(Exception exception, LogType logType)
        {
            return LogAsync(string.Empty, exception, logType);
        }

        public static void Log(string message, Exception exception, LogType logType)
        {
            DoLogAsync(message, exception, logType, true).Wait();
        }

        public static Task LogAsync(string message, Exception exception, LogType logType)
        {
            return DoLogAsync(message, exception, logType, false);
        }

        public static void Log(string message, LogType logType)
        {
            DoLogAsync(message, null, logType, true).Wait();
        }

        public static Task LogAsync(string message, LogType logType)
        {
            return DoLogAsync(message, null, logType, false);
        }

        private async static Task DoLogAsync(string message, Exception exception, LogType logType, bool sync)
        {         
            if (IsEnabledLogInDb)
            {
                string result = string.Empty;

                using (var dbContext = (IMainDbContext)Activator.CreateInstance(DbContext))
                {
                    using (var errorLogRepo = new LogRepo(dbContext))
                    {
                        result = DbActionWrapper.Execute(() => errorLogRepo.Save(new LogModel(message, logType, HttpContext.Current.Request.Url.AbsoluteUri, exception)));

                        if (WebUtils.WithoutErrors(result))
                        {
                            result = sync ? DbActionWrapper.Execute(errorLogRepo.SaveChanges) : await DbActionWrapper.ExecuteAsync(errorLogRepo.SaveChangesAsync);
                        }
                    }
                }

                if (!WebUtils.WithoutErrors(result))
                {
                    message = $"{message}; {result}";
                }
            }

            switch (logType)
            {
                case LogType.Debug:
                    Helpers.Log.Debug(message, exception);
                    break;
                case LogType.Error:
                    Helpers.Log.Error(message, exception);
                    break;
                case LogType.Fatal:
                    Helpers.Log.Fatal(message, exception);
                    break;
                case LogType.Info:
                    Helpers.Log.Info(message, exception);
                    break;
                case LogType.Warn:
                    Helpers.Log.Warn(message, exception);
                    break;
            }
        }
    }
}
