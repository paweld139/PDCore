using PDCore.Enums;
using PDCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Loggers
{
    public abstract class Logger : ILogger
    {
        public void Log(Exception exception, LogType logType)
        {
            Log(string.Empty, exception, logType);
        }

        public void Log(string message, LogType logType)
        {
            Log(message, null, logType);
        }

        public void Log(string message)
        {
            Log(message, null, LogType.Info);
        }

        public abstract void Log(string message, Exception exception, LogType logType);
    }
}
