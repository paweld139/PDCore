using PDCore.Enums;
using PDCore.Interfaces;
using PDCore.Repositories.IRepo;
using PDCore.Utils;
using PDCoreNew.Loggers;
using PDWebCore.Helpers.ExceptionHandling;
using PDWebCore.Models;
using PDWebCore.Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PDWebCore.Loggers
{
    public class SqlServerLogger : AsyncLogger
    {
        private readonly ILogRepo logRepository;
        private static readonly Lazy<FileLogger> fileLogger = new Lazy<FileLogger>();

        public SqlServerLogger(ILogRepo logRepository)
        {
            this.logRepository = logRepository;
        }

        protected override async Task DoLogAsync(string message, Exception exception, LogType logType, bool sync)
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

        public override void Dispose()
        {
            logRepository.Dispose();
        }
    }
}
