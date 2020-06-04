using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Interfaces
{
    public interface IFromCSVParseable
    {
        void ParseFromCSV(string[] lineFields);
    }
}
