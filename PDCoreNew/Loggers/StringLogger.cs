using PDCore.Enums;
using PDCore.Factories.IFac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Loggers
{
    public abstract class StringLogger : Logger
    {
        private readonly ILogMessageFactory logMessageFactory;

        public StringLogger(ILogMessageFactory logMessageFactory)
        {
            this.logMessageFactory = logMessageFactory;
        }

        public override void Log(string message, Exception exception, LogType logType)
        {
            string info = logMessageFactory.Create(message, exception, logType);

            Print(info);
        }

        protected abstract Action<string> Print { get; }
    }
}
