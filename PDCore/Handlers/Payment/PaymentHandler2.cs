using PDCore.Models.Shop;
using PDCore.Models.Shop.Enums;
using PDCore.Models.Shop.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Handlers.Payment
{
    public class PaymentHandler2 : Handler2<Order>
    {
        public PaymentHandler2(params IReceiver<Order>[] receivers) : base(receivers)
        {
        }

        public override void Handle(Order order)
        {
            foreach (var receiver in receivers)
            {
                Console.WriteLine($"Running: {receiver.GetType().Name}");

                if (order.AmountDue > 0)
                {
                    receiver.Handle(order);
                }
                else
                {
                    break;
                }
            }

            if (order.AmountDue > 0)
            {
                throw new InsufficientPaymentException();
            }
            else
            {
                order.ShippingStatus = ShippingStatus.ReadyForShipment;
            }
        }
    }
}
