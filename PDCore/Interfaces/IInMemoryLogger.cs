using PDCore.Helpers.DataStructures.Buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Interfaces
{
    public interface IInMemoryLogger : ILogger
    {
        IBuffer<string> Logs { get; }
    }
}
