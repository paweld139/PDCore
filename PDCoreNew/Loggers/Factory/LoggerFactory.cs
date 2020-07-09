using PDCore.Interfaces;
using PDCoreNew.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Loggers.Factory
{
    public static class LoggerFactory
    {
        public static ILogger GetLogger()
        {
            return new FileLogger();
        }
    }
}
