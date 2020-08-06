using PDCore.Models.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Repositories.Repo.Shop.Interfaces
{
    public interface IProductRepository
    {
        Product FindBy(string articleId);
        int GetStockFor(string articleId);
        IEnumerable<Product> All();
        void DecreaseStockBy(string articleId, int amount);
        void IncreaseStockBy(string articleId, int amount);
    }
}
