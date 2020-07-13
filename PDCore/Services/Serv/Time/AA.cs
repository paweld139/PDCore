using PDCore.Helpers.Calculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Services.Serv.Time
{
    public class AA
    {
        public DateTime When { get; private set; }

        public AA(PreciseDatetime preciseDatetime)
        {
            When = preciseDatetime.Now;
        }

        public AA() : this(new PreciseDatetime())
        {

        }
    }
}
