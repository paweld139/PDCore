using PDCore.Enums;
using PDCore.Interfaces;
using PDCore.Repositories.IRepo;
using PDCore.Utils;
using PDCoreNew.Helpers.ExceptionHandling;
using PDCoreNew.Loggers;
using PDCoreNew.Models;
using PDCoreNew.Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PDCoreNew.Loggers.Async
{
    public class SqlServerLogger : AsyncLogger
    {
        private readonly ISqlRepositoryEntityFramework<LogModel> logRepository;
        private static readonly Lazy<FileLogger> fileLogger = new Lazy<FileLogger>();

        public SqlServerLogger(ISqlRepositoryEntityFramework<LogModel> logRepository)
        {
            this.logRepository = logRepository;
        }

        protected override async Task DoLogAsync(string message, Exception exception, LogType logType, bool sync)
        {
            string result = string.Empty;

            result = DbActionWrapper.Execute(() => logRepository.Add(GetLogModel(message, logType, exception)));

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

        protected virtual LogModel GetLogModel(string message, LogType logType, Exception exception)
        {
            return new LogModel(message, logType, null, exception);
        }

        public override void Dispose()
        {
            logRepository.Dispose();
        }
    }
}
