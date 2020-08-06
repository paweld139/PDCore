using PDCore.Models.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Processors.Payment.Interfaces
{
    public interface IPaymentProcessor
    {
        void Finalize(Order order);
    }
}
