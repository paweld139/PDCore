using PDCore.Models.Shop;
using PDCore.Models.Shop.Enums;
using PDCore.Processors.Payment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Processors.Payment
{
    public class InvoicePaymentProcessor : IPaymentProcessor
    {
        public void Finalize(Order order)
        {
            // Create an invoice and email it

            var payment = order.SelectedPayments.FirstOrDefault(p => p.PaymentProvider == PaymentProvider.Invoice);

            if (payment == null) return;

            order.FinalizedPayments.Add(payment);
        }
    }
}
