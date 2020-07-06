using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Helpers
{
    public class NamedObject
    {
        public NamedObject(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
