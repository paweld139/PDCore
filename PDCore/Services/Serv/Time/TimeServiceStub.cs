using PDCore.Services.IServ;
using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Services.Serv.Time
{
    public class TimeServiceStub : TimeService
    {
        public TimeServiceStub()
        {
            Now = base.Now;
        }

        public override DateTime Now { get; protected set; }

        public override void Sleep(TimeSpan timeout)
        {
            Now += timeout;
        }
    }
}
