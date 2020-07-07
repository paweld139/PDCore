using PDCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Interfaces
{
    public interface ILogger
    {       
        void Log(string message, Exception exception, LogType logType);

        void Log(string message, LogType logType);

        void Log(Exception exception, LogType logType);
    }
}
