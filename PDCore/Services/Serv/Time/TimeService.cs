using PDCore.Services.IServ;
using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PDCore.Services.Serv.Time
{
    public class TimeService : ITimeService
    {
        public virtual DateTime Now { get; protected set; } = DateTime.Now;

        public virtual void Sleep(TimeSpan timeout) => Thread.Sleep(timeout);

        public void Sleep(int millisecondsTimeout)
        {
            TimeSpan timeSpan = DateTimeUtils.GetTimeSpan(millisecondsTimeout);

            Sleep(timeSpan);
        }
    }
}
