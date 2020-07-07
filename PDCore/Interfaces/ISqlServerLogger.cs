using PDCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Interfaces
{
    public interface ISqlServerLogger : ILogger, IDisposable
    {
        Task LogAsync(string message, Exception exception, LogType logType);

        Task LogAsync(string message, LogType logType);

        Task LogAsync(Exception exception, LogType logType);
    }
}
