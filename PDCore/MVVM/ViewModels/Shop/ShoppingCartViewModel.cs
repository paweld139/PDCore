using PDCore.Commands.Shop;
using PDCore.Commands.WPF;
using PDCore.Repositories.Repo.Shop.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace PDCore.MVVM.ViewModels.Shop
{
    public class ShoppingCartViewModel : ViewModelBase
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IProductRepository productRepository;

        public System.Windows.Input.ICommand RemoveAllFromCartCommand { get; private set; }
        public System.Windows.Input.ICommand CheckoutCommand { get; private set; }

        public ObservableCollection<ProductViewModel> Products { get; private set; }
        public ObservableCollection<ProductViewModel> LineItems { get; private set; }

        public ShoppingCartViewModel(IShoppingCartRepository shoppingCartRepository,
            IProductRepository productRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;
        }

        public void InitializeViewModel()
        {
            var removeAllFromCartCommand
                = new RemoveAllFromCartCommand(shoppingCartRepository, productRepository);

            RemoveAllFromCartCommand = new RelayCommand2(
                    execute: () =>
                    {
                        removeAllFromCartCommand.Execute();

                        Refresh();
                    },
                    canExecute: () => removeAllFromCartCommand.CanExecute()
                );

            CheckoutCommand = new RelayCommand2(
                    execute: () => {
                        var total = LineItems.Sum(x => x.Product.Price * x.Quantity);
                        MessageBox.Show($"Shopping cart total: ${total}");
                    },
                    canExecute: () => LineItems.Any()
                );

            Refresh();
        }

        public void Refresh()
        {
            var products = productRepository
                .All()
                .Select(product => new ProductViewModel(this,
                                        shoppingCartRepository,
                                        productRepository,
                                        product));

            var lineItems = shoppingCartRepository
                .All()
                .Select(x => new ProductViewModel(this,
                                    shoppingCartRepository,
                                    productRepository,
                                    x.Product,
                                    x.Quantity));

            Products = new ObservableCollection<ProductViewModel>(products);
            LineItems = new ObservableCollection<ProductViewModel>(lineItems);

            OnPropertyRaised(nameof(Products));
            OnPropertyRaised(nameof(LineItems));
        }
    }
}
