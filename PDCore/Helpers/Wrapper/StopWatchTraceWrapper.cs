using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PDCore.Helpers
{
    public static class StopWatchTraceWrapper
    {
        private static Stopwatch _stopWatch;
        private static Stopwatch StopWatch
        {
            get 
            { 
                if(_stopWatch == null)
                {
                    _stopWatch = new Stopwatch();
                }

                return _stopWatch; 
            }
        }

        public static void Execute(Action action)
        {
            StopWatch.Start();

            action();

            StopWatch.Stop();

            string result = StopWatch.Elapsed.ToString();

            StopWatch.Reset();

            Trace.WriteLine(result);
        }
    }
}
