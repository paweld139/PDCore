using PDCore.Services.IServ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Services.Serv.Time
{
    public class SomeClass
    {
        private readonly ISleepService _sleepService;

        public SomeClass(ISleepService sleepService)
        {
            _sleepService = sleepService;
        }

        public void DoSomething(int millisecondsTimeout)
        {
            while (true)
            {
                _sleepService.Sleep(millisecondsTimeout);

                break;
            }
        }
    }
}
