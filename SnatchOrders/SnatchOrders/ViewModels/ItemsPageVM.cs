using SnatchOrders.Models;
using SnatchOrders.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SnatchOrders.ViewModels
{
    public class ItemsPageVM : ViewModelBase
    {
        public Order CurrentOrder { get; set; }

        public ICommand AddNewItemCommand { get; set; }
        public ICommand IncreaseQuantityCommand { get; set; }
        public ICommand DecreaseQuantityCommand { get; set; }

        private INavigation _navigation { get; set; }

        public ItemsPageVM(INavigation navigation)
        {
            _navigation = navigation;
            CurrentOrder = new Order();
            AddNewItemCommand = new Command(AddNewItem);
        }

        private async void AddNewItem()
        {
            await _navigation.PushAsync(new NewItemPage());
        }
    }
}
