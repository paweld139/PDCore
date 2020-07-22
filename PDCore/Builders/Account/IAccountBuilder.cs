using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Builders.Account
{
    public interface IAccountBuilder : IBuilder<Account>
    {
        AccountBuilder WithLatePaymentStatus();

        AccountBuilder WithVipCustomer();
    }
}
