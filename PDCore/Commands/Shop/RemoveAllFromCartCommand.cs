using PDCore.Repositories.Repo.Shop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Commands.Shop
{
    public class RemoveAllFromCartCommand : ICommand
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IProductRepository productRepository;

        public RemoveAllFromCartCommand(IShoppingCartRepository shoppingCartRepository,
            IProductRepository productRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;
        }

        public bool CanExecute()
        {
            return shoppingCartRepository.All().Any();
        }

        public void Execute()
        {
            var items = shoppingCartRepository.All().ToArray(); // Make a local copy

            foreach (var (Product, Quantity) in items)
            {
                productRepository.IncreaseStockBy(Product.ArticleId, Quantity);

                shoppingCartRepository.RemoveAll(Product.ArticleId);
            }
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
