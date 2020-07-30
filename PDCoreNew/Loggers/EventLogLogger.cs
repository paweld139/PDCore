using PDCore.Enums;
using PDCore.Factories.IFac;
using System;
using System.Diagnostics;

namespace PDCoreNew.Loggers
{
    public class EventLogLogger : Logger, IDisposable
    {
        private readonly ILogMessageFactory logMessageFactory;
        private readonly EventLog eventLog;

        public EventLogLogger(ILogMessageFactory logMessageFactory)
        {
            this.logMessageFactory = logMessageFactory;

            eventLog = new EventLog("Application")
            {
                Source = "Application"
            };
        }

        public override void Log(string message, Exception exception, LogType logType)
        {
            var eventLogType = GetEventLogEntryType(logType);

            string info = logMessageFactory.Create(message, exception, logType);

            eventLog.WriteEntry(info, eventLogType);
        }

        private EventLogEntryType GetEventLogEntryType(LogType logType)
        {
            switch (logType)
            {
                case LogType.Error:
                case LogType.Fatal:
                    return EventLogEntryType.Error;
                case LogType.Info:
                case LogType.Debug:
                    return EventLogEntryType.Information;
                case LogType.Warn:
                    return EventLogEntryType.Warning;
                default:
                    return EventLogEntryType.Information;
            }
        }

        public void Dispose()
        {
            eventLog.Dispose();
        }
    }
}
