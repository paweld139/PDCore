using PDCore.Enums;
using PDCore.Interfaces;
using PDCore.Repositories.IRepo;
using PDCore.Utils;
using PDCoreNew.Loggers;
using PDWebCore.Helpers.ExceptionHandling;
using PDWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PDWebCore.Loggers
{
    public class SqlServerLogger : ISqlServerLogger
    {
        private readonly ISqlRepositoryEntityFramework<LogModel> logRepository;
        private static readonly Lazy<FileLogger> fileLogger = new Lazy<FileLogger>();

        public SqlServerLogger(ISqlRepositoryEntityFramework<LogModel> logRepository)
        {
            this.logRepository = logRepository;
        }

        public void Log(Exception exception, LogType logType)
        {
            Log(string.Empty, exception, logType);
        }

        public Task LogAsync(Exception exception, LogType logType)
        {
            return LogAsync(string.Empty, exception, logType);
        }

        public void Log(string message, Exception exception, LogType logType)
        {
            DoLogAsync(message, exception, logType, true).Wait();
        }

        public Task LogAsync(string message, Exception exception, LogType logType)
        {
            return DoLogAsync(message, exception, logType, false);
        }

        public void Log(string message, LogType logType)
        {
            DoLogAsync(message, null, logType, true).Wait();
        }

        public Task LogAsync(string message, LogType logType)
        {
            return DoLogAsync(message, null, logType, false);
        }

        private async Task DoLogAsync(string message, Exception exception, LogType logType, bool sync)
        {
            string result = string.Empty;

            result = DbActionWrapper.Execute(() => logRepository.Add(new LogModel(message, logType, HttpContext.Current.Request.Url.AbsoluteUri, exception)));

            if (WebUtils.WithoutErrors(result))
            {
                result = sync ? DbActionWrapper.Execute(() => logRepository.Commit()) : await DbActionWrapper.ExecuteAsync(logRepository.CommitAsync);
            }

            if (!WebUtils.WithoutErrors(result))
            {
                message = $"{message}; {result}";

                fileLogger.Value.Log(message, exception, logType);
            }           
        }

        public void Dispose()
        {
            logRepository.Dispose();
        }
    }
}
