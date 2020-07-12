using PDCore.Services.IServ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PDCore.Services.Serv.Time
{
    public class TimeService : ITimeService
    {
        public DateTime Now { get { return DateTime.UtcNow; } }

        public void Sleep(TimeSpan delay) { Thread.Sleep(delay); }
    }
}
