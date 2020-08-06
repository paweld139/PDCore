using PDCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Helpers.Comparers
{
    public class NamedObjectComparer : IEqualityComparer<NamedObject>, IComparer<NamedObject>
    {
        public bool Equals(NamedObject x, NamedObject y)
        {
            return string.Equals(x.Name, y.Name);
        }

        public int GetHashCode(NamedObject obj)
        {
            return obj.Name.GetHashCode();
        }

        public int Compare(NamedObject x, NamedObject y)
        {
            return string.Compare(x.Name, y.Name); // "<0", gdy pierwszy jest większy, ">0" gdy drugi jest większy i 0 gdy są równe
        }
    }
}
