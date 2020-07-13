using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PDCore.Helpers.Calculation
{
    public class PreciseDatetime
    {
        // using DateTime.Now resulted in many many log events with the same timestamp.
        // use static variables in case there are many instances of this class in use in the same program
        // (that way they will all be in sync)
        private static readonly Stopwatch myStopwatch = new Stopwatch();
        private static DateTime myStopwatchStartTime;

        static PreciseDatetime()
        {
            Reset();

            try
            {
                // In case the system clock gets updated
                SystemEvents.TimeChanged += SystemEvents_TimeChanged;
            }
            catch (Exception)
            {
            }
        }

        private static void SystemEvents_TimeChanged(object sender, EventArgs e)
        {
            Reset();
        }

        // SystemEvents.TimeChanged can be slow to fire (3 secs), so allow forcing of reset
        public static void Reset()
        {
            myStopwatchStartTime = DateTime.Now;

            myStopwatch.Restart();
        }

        public DateTime Now { get { return myStopwatchStartTime.Add(myStopwatch.Elapsed); } }
    }
}
