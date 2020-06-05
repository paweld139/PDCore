using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PDCore.Helpers
{
    public class DisposableStopwatch : IDisposable
    {
        private readonly Stopwatch sw;

        private readonly Action<TimeSpan> f;

        public DisposableStopwatch()
        {
            sw = Stopwatch.StartNew();
        }

        public DisposableStopwatch(Action<TimeSpan> f) : this()
        {
            this.f = f;
        }

        public void Dispose()
        {
            sw.Stop();

            if (f != null)
                f(sw.Elapsed);
            else
            {
                string elapsed = sw.Elapsed.ToString();

                Trace.WriteLine(elapsed);
            }
        }
    }
}
