using PDCore.Services.IServ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Services.Serv.Time
{
    public class TimeServiceStub : ITimeService
    {
        public TimeServiceStub()
        {
            Now = DateTime.UtcNow;
        }

        public DateTime Now { get; private set; }

        public void Sleep(TimeSpan delay)
        {
            Now += delay;
        }
    }
}
