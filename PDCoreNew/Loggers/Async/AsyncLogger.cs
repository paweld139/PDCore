using PDCore.Enums;
using PDCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Loggers.Async
{
    public abstract class AsyncLogger : Logger, IAsyncLogger
    {
        public override void Log(string message, Exception exception, LogType logType)
        {
            DoLogAsync(message, exception, logType, true).Wait();
        }


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


        public Task LogAsync(string message)
        {
            return LogAsync(message, null, LogType.Info);
        }

        public Task LogAsync(Exception exception)
        {
            return LogAsync(string.Empty, exception, LogType.Info);
        }


        public Task DebugAsync(string message, Exception exception)
        {
            return DoLogAsync(message, exception, LogType.Debug, false);
        }

        public Task ErrorAsync(string message, Exception exception)
        {
            return DoLogAsync(message, exception, LogType.Error, false);
        }

        public Task FatalAsync(string message, Exception exception)
        {
            return DoLogAsync(message, exception, LogType.Fatal, false);
        }

        public Task InfoAsync(string message, Exception exception)
        {
            return DoLogAsync(message, exception, LogType.Info, false);
        }

        public Task WarnAsync(string message, Exception exception)
        {
            return DoLogAsync(message, exception, LogType.Warn, false);
        }


        public Task DebugAsync(string message)
        {
            return DoLogAsync(message, null, LogType.Debug, false);
        }

        public Task ErrorAsync(string message)
        {
            return DoLogAsync(message, null, LogType.Error, false);
        }

        public Task FatalAsync(string message)
        {
            return DoLogAsync(message, null, LogType.Fatal, false);
        }

        public Task InfoAsync(string message)
        {
            return DoLogAsync(message, null, LogType.Info, false);
        }

        public Task WarnAsync(string message)
        {
            return DoLogAsync(message, null, LogType.Warn, false);
        }


        protected abstract Task DoLogAsync(string message, Exception exception, LogType logType, bool sync);

        public abstract void Dispose();
    }
}
