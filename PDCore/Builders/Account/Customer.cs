using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Builders.Account
{
    public class Customer
    {
        public string Name { get; set; }
        public bool IsVip { get; set; }
        public Address Address { get; set; }
    }
}
