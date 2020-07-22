using PDCore.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PDCore.Helpers.Wrappers
{
    public class StopWatchWrapper //Timekeeper
    {
        private readonly static Lazy<Stopwatch> stopWatch = new Lazy<Stopwatch>();

        public Tuple<TimeSpan, TOutput> Measure<TOutput>(Func<TOutput> func)
        {
            return stopWatch.Value.Time(func);
        }

        public Tuple<TimeSpan, TOutput> Measure<TInput, TOutput>(Func<TInput, TOutput> func, TInput param)
        {
            return Measure(func.Partial(param));
        }

        public TimeSpan Measure(Action action)
        {
            return Measure(() => { action(); return true; }).Item1;
        }

        public void Execute(Action action, Action<string> print)
        {
            string result = Measure(action).ToString();

            print(result);
        }

        public void Execute(Action action)
        {
            Execute(action, t => Trace.WriteLine(t));
        }
    }
}
