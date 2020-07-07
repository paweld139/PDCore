using PDCore.Enums;
using PDCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCoreNew.Helpers;

namespace PDCoreNew.Loggers
{
    public class FileLogger : ILogger
    {
        public void Log(Exception exception, LogType logType)
        {
            Log(string.Empty, exception, logType);
        }

        public void Log(string message, LogType logType)
        {
            Log(message, null, logType);
        }

        public void Log(string message, Exception exception, LogType logType)
        {
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
