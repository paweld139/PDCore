using PDCore.Models.Shop;
using PDCore.Repositories.Repo.Shop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Repositories.Repo.Shop
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        public Dictionary<string, (Product Product, int Quantity)> LineItems
            = new Dictionary<string, (Product Product, int Quantity)>();

        public IEnumerable<(Product Product, int Quantity)> All()
        {
            return LineItems.Values;
        }

        public (Product Product, int Quantity) Get(string articleId)
        {
            if (LineItems.ContainsKey(articleId))
            {
                return LineItems[articleId];
            }

            return (default, default);
        }

        public void Add(Product product)
        {
            if (LineItems.ContainsKey(product.ArticleId))
            {
                IncreaseQuantity(product.ArticleId);
            }
            else
            {
                LineItems[product.ArticleId] = (product, 1);
            }
        }

        public void DecraseQuantity(string articleId)
        {
            if (LineItems.ContainsKey(articleId))
            {
                var lineItem = LineItems[articleId];

                if (lineItem.Quantity == 1)
                {
                    LineItems.Remove(articleId);
                }
                else
                {
                    LineItems[articleId] = (lineItem.Product, lineItem.Quantity - 1);
                }
            }
            else
            {
                throw new KeyNotFoundException($"Product with article id {articleId} not in cart, please add first");
            }
        }

        public void IncreaseQuantity(string articleId)
        {
            if (LineItems.ContainsKey(articleId))
            {
                var lineItem = LineItems[articleId];
                LineItems[articleId] = (lineItem.Product, lineItem.Quantity + 1);
            }
            else
            {
                throw new KeyNotFoundException($"Product with article id {articleId} not in cart, please add first");
            }
        }

        public void RemoveAll(string articleId)
        {
            LineItems.Remove(articleId);
        }

        public static void PrintCart(ShoppingCartRepository shoppingCartRepository)
        {
            var totalPrice = 0m;
            foreach (var lineItem in shoppingCartRepository.LineItems)
            {
                var price = lineItem.Value.Product.Price * lineItem.Value.Quantity;

                Console.WriteLine($"{lineItem.Key} " +
                    $"${lineItem.Value.Product.Price} x {lineItem.Value.Quantity} = ${price}");

                totalPrice += price;
            }

            Console.WriteLine($"Total price:\t${totalPrice}");
        }
    }
}
