using PDCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Interfaces
{
    public interface IAsyncLogger : ILogger, IDisposable
    {
        Task DebugAsync(string message, Exception exception);

        Task ErrorAsync(string message, Exception exception);

        Task FatalAsync(string message, Exception exception);

        Task InfoAsync(string message, Exception exception);

        Task WarnAsync(string message, Exception exception);


        Task DebugAsync(string message);

        Task ErrorAsync(string message);

        Task FatalAsync(string message);

        Task InfoAsync(string message);

        Task WarnAsync(string message);


        Task LogAsync(string message, Exception exception, LogType logType);


        Task LogAsync(Exception exception, LogType logType);

        Task LogAsync(string message, LogType logType);


        Task LogAsync(string message);

        Task LogAsync(Exception exception);
    }
}
