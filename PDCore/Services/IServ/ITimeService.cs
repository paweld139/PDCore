using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Services.IServ
{
    public interface ITimeService
    {
        DateTime Now { get; }

        void Sleep(TimeSpan timeout);

        void Sleep(int millisecondsTimeout);
    }
}
