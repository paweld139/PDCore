﻿using PDCore.Enums;
using PDCore.Repositories.IRepo;
using PDCoreNew.Loggers;
using PDCoreNew.Loggers.Async;
using PDCoreNew.Models;
using PDCoreNew.Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PDWebCore.Loggers
{
    public class SqlServerWebLogger : SqlServerLogger
    {
        public SqlServerWebLogger(ISqlRepositoryEntityFrameworkAsync<LogModel> logRepository) : base(logRepository)
        {

        }

        protected override LogModel GetLogModel(string message, LogType logType, Exception exception)
        {
            return new LogModel(message, logType, HttpContext.Current?.Request.Url.AbsoluteUri, exception);
        }
    }
}
