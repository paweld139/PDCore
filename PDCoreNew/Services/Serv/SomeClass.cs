using CommonServiceLocator;
using PDCore.Services.IServ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity;

namespace PDCoreNew.Services.Serv
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
