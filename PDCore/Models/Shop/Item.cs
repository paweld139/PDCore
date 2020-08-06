using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Models.Shop
{
    public class Item
    {
        public Item(string articleId, string name, decimal price)
        {
            ArticleId = articleId;
            Name = name;
            Price = price;
        }

        public string ArticleId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
