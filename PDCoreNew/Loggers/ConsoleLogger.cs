using PDCore.Enums;
using PDCoreNew.Factories;
using PDCoreNew.Factories.IFac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Loggers
{
    public class ConsoleLogger : StringLogger
    {
        public ConsoleLogger(ILogMessageFactory logMessageFactory) : base(logMessageFactory)
        {

        }

        protected override Action<string> Print => Console.WriteLine;
    }
}
