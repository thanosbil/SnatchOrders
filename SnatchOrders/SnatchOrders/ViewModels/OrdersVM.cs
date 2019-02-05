using SnatchOrders.Models;
using SnatchOrders.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SnatchOrders.ViewModels
{
    public class OrdersVM : ViewModelBase {

        public ObservableCollection<Order> Orders { get; set; }
        private List<Order> dbOrders { get; set; }

        public ICommand NewOrderCommand { get; set; }
        private INavigation _navigation;

        public OrdersVM(INavigation navigation)
        {
            NewOrderCommand = new Command(MakeNewOrder);
            _navigation = navigation;
            Orders = new ObservableCollection<Order>();
            dbOrders = new List<Order>();
        }

        public async void GetOrdersList()
        {
            Orders.Clear();
            dbOrders.Clear();
            dbOrders = await App.Database.GetOrdersAsync();

            if (dbOrders.Count > 0)
            {
                foreach (Order item in dbOrders)
                {
                    Orders.Add(item);
                }
            }
        }

        private async void MakeNewOrder()
        {
           await _navigation.PushAsync(new CategoriesPage());
        }
    }
}
