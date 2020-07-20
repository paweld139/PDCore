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

        public void Log(Exception exception)
        {
            Log(string.Empty, exception, LogType.Info);
        }


        public void Debug(string message, Exception exception)
        {
            Log(message, exception, LogType.Debug);
        }

        public void Error(string message, Exception exception)
        {
            Log(message, exception, LogType.Error);
        }

        public void Fatal(string message, Exception exception)
        {
            Log(message, exception, LogType.Fatal);
        }

        public void Info(string message, Exception exception)
        {
            Log(message, exception, LogType.Info);
        }

        public void Warn(string message, Exception exception)
        {
            Log(message, exception, LogType.Warn);
        }


        public void Debug(string message)
        {
            Log(message, null, LogType.Debug);
        }

        public void Error(string message)
        {
            Log(message, null, LogType.Error);
        }

        public void Fatal(string message)
        {
            Log(message, null, LogType.Fatal);
        }

        public void Info(string message)
        {
            Log(message, null, LogType.Info);
        }

        public void Warn(string message)
        {
            Log(message, null, LogType.Warn);
        }

        public abstract void Log(string message, Exception exception, LogType logType);
    }
}
