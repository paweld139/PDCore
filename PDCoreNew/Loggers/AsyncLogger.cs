using PDCore.Enums;
using PDCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Loggers
{
    public abstract class AsyncLogger : Logger, IAsyncLogger
    {
        public Task LogAsync(string message, Exception exception, LogType logType)
        {
            return DoLogAsync(message, exception, logType, false);
        }

        public Task LogAsync(Exception exception, LogType logType)
        {
            return LogAsync(string.Empty, exception, logType);
        }

        public Task LogAsync(string message, LogType logType)
        {
            return LogAsync(message, null, logType);
        }

        public override void Log(string message, Exception exception, LogType logType)
        {
            DoLogAsync(message, exception, logType, true).Wait();
        }

        protected abstract Task DoLogAsync(string message, Exception exception, LogType logType, bool sync);

        public abstract void Dispose();
    }
}
