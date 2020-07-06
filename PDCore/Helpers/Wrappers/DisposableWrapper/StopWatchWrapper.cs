using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PDCore.Helpers.Wrappers.DisposableWrapper
{
    public class StopWatchWrapper : DisposableWrapper<DisposableStopwatch>
    {
        public StopWatchWrapper(DisposableStopwatch disposableStopwatch) : base(disposableStopwatch) { }

        protected override void OnDispose()
        {
            // lots of code per state of BaseObject
            BaseObject.Dispose();
        }
    }
}
