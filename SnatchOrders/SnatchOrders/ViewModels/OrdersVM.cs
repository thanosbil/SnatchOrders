using SnatchOrders.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SnatchOrders.ViewModels
{
    public class OrdersVM
    {
        public ICommand NewOrder { get; set; }
        private INavigation _navigation;

        public OrdersVM(INavigation navigation)
        {
            NewOrder = new Command(MakeNewOrder);
            _navigation = navigation;
        }

        private async void MakeNewOrder()
        {
           await _navigation.PushAsync(new NewItemPage());
        }
    }
}
