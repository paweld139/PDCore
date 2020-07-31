using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Interfaces
{
    public interface IPeriod
    {
        DateTimeOffset StartDate { get; set; }

        TimeSpan Duration { get; set; }
    }
}
