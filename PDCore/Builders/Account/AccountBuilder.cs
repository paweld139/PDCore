using System;

namespace PDCore.Builders.Account
{
    public class AccountBuilder : Builder<Account>, IAccountBuilder
    {
        public AccountBuilder()
        {
            _object = new Account
            {
                Balance = 10000,
                DueDate = DateTime.Now.AddDays(1),
                Customer = new Customer
                {
                    Name = "Michelle",
                    IsVip = false,
                    Address = new Address
                    {
                        City = "D.C",
                        Country = "USA"
                    }
                }
            };
        }

        public AccountBuilder WithLatePaymentStatus()
        {
            _object.DueDate = DateTime.Now.AddDays(-1);

            return this;
        }

        public AccountBuilder WithVipCustomer()
        {
            _object.Customer.IsVip = true;

            return this;
        }
    }
}

