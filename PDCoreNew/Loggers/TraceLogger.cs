using PDCore.Enums;
using PDCore.Factories.IFac;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Loggers
{
    public class TraceLogger : StringLogger
    {
        public TraceLogger(ILogMessageFactory logMessageFactory) : base(logMessageFactory)
        {

        }

        protected override Action<string> Print => m => Trace.WriteLine(m);
    }
}
