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
        public ICommand DeleteOrderCommand { get; set; }
        private INavigation _navigation;

        public OrdersVM(INavigation navigation)
        {
            NewOrderCommand = new Command(MakeNewOrder);
            DeleteOrderCommand = new Command<Order>(DeleteOrder);
            _navigation = navigation;
            Orders = new ObservableCollection<Order>();
            dbOrders = new List<Order>();
        }

        private async void DeleteOrder(Order obj)
        {
            bool result = await App.Current.MainPage.DisplayAlert("Διαγραφή", "Πρόκειται να διαγραφεί η παραγγελία. Θέλετε να συνεχίσετε;", "OK", "ΑΚΥΡΟ");

            if (result)
            {
                await App.Database.DeleteOrderAsync(obj);
                GetOrdersList();
            }
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
