using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDCore.Builders.Account;

namespace PDCore.Tests
{
    [TestClass]
    public class AccountBuilderTests
    {
        [TestMethod]
        public void DemonstrationOfCreatingAccount()
        {
            var account = new Account
            {
                Balance = 9000,
                Customer = new Customer
                {
                    Address = new Address()
                }
            };
            account.Customer.Address.City = "London";
            account.Customer.Address.Country = "UK";
            account.Customer.IsVip = true;
            account.DueDate = DateTime.Now.AddDays(-1);
            account.Customer.Name = "Penelope";
        }

        [TestMethod]
        public void CanCreateAccount()
        {
            var account = AccountBuilder.DefaultAccount().Build();

            Assert.IsNotNull(account);
        }

        [TestMethod]
        public void AssignsDefaultValues()
        {
            var account = AccountBuilder.DefaultAccount().Build();


            Assert.IsTrue(account.Balance > 0);
            Assert.IsTrue(account.DueDate > DateTime.Now);

            Assert.IsNotNull(account.Customer.Address.City);
            Assert.IsNotNull(account.Customer.Address.Country);

            Assert.IsFalse(account.Customer.IsVip);
            Assert.IsNotNull(account.Customer.Name);
        }

        [TestMethod]
        public void CanCreateLateAccount()
        {
            var account = AccountBuilder.DefaultAccount()
                                        .WithLatePaymentStatus()
                                        .Build();

            Assert.IsTrue(account.DueDate < DateTime.Now);
        }

        [TestMethod]
        public void CanCreateLateAccountWithVipCustomer()
        {
            var account = AccountBuilder.DefaultAccount()
                                        .WithLatePaymentStatus()
                                        .WithVipCustomer()
                                        .Build();

            Assert.IsTrue(account.Customer.IsVip);
        }

        class AccountBuilder
        {
            private readonly Account _account;

            public AccountBuilder()
            {
                _account = new Account
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

            public static AccountBuilder DefaultAccount()
            {
                return new AccountBuilder();
            }

            public Account Build()
            {
                return _account;
            }

            public AccountBuilder WithLatePaymentStatus()
            {
                _account.DueDate = DateTime.Now.AddDays(-1);

                return this;
            }

            public AccountBuilder WithVipCustomer()
            {
                _account.Customer.IsVip = true;

                return this;
            }
        }
    }
}
