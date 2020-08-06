using PDCore.Models.Shop.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Models.Shop
{
    public class Payment
    {
        public PaymentProvider PaymentProvider { get; set; }

        public decimal Amount { get; set; }
    }
}
