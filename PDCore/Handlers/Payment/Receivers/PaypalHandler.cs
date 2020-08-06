using PDCore.Models.Shop;
using PDCore.Models.Shop.Enums;
using PDCore.Processors.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Handlers.Payment.Receivers
{
    public class PaypalHandler : IReceiver<Order> //PaymentHandler
    {
        private readonly PaypalPaymentProcessor PaypalPaymentProcessor = new PaypalPaymentProcessor();

        public void Handle(Order order)
        {
            if (order.SelectedPayments.Any(x => x.PaymentProvider == PaymentProvider.Paypal))
                PaypalPaymentProcessor.Finalize(order);
        }
    }
}
