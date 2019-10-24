using SnatchOrders.Models;
using SnatchOrders.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SnatchOrders.ViewModels
{
    public class OrdersVM : ViewModelBase {
        private bool _hasItems { get; set; }
        public bool HasItems {
            get { return _hasItems; }
            set {
                if(_hasItems != value) {
                    _hasItems = value;
                    OnPropertyChanged("HasItems");
                }
            }
        }
        private ObservableCollection<Order> _Orders { get; set; }
        public ObservableCollection<Order> Orders {
            get { return _Orders; }
            set {
                _Orders = value;
                OnPropertyChanged("Orders");
            }
        }
        private List<Order> dbOrders { get; set; }
        public ICommand ItemTappedCommand { get; set; }
        public ICommand NewOrderCommand { get; set; }
        public ICommand DeleteOrderCommand { get; set; }
        private INavigation _navigation;

        public OrdersVM(INavigation navigation){
            ItemTappedCommand = new Command<Order>(ItemTapped);
            NewOrderCommand = new Command(MakeNewOrder);
            DeleteOrderCommand = new Command<Order>(DeleteOrder);
            _navigation = navigation;
            Orders = new ObservableCollection<Order>();
            dbOrders = new List<Order>();
        }

        private async void ItemTapped(Order obj) {
            await _navigation.PushAsync(new OrderPage(obj));
        }

        private async void DeleteOrder(Order obj)
        {
            bool result = await App.Current.MainPage.DisplayAlert("Διαγραφή", "Πρόκειται να διαγραφεί η παραγγελία. Θέλετε να συνεχίσετε;", "OK", "ΑΚΥΡΟ");

            if (result)
            {
                try {
                    Orders.Remove(obj);
                    await App.Database.DeleteOrderAsync(obj);                    
                }catch(Exception ex) {
                    await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά τη διαγραφή της παραγγελίας"
                   + Environment.NewLine + ex, "OK");
                }
            }
        }

        public async Task GetOrdersList()
        {
            Orders.Clear();
            
            dbOrders = await App.Database.GetOrdersAsync();

            if (dbOrders.Count > 0){
                foreach (Order item in dbOrders)
                {
                    Orders.Add(item);
                }
                HasItems = true;
            } else {
                HasItems = false;
            }
        }

        private async void MakeNewOrder(){
            Order current = new Order();
            current.DateCreated = DateTime.Now;
            current.OrderStatus = StatusOfOrder.New;

            try {
                await App.Database.SaveOrderAsync(current);
            }catch(Exception ex) {
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την αποθήκευση της παραγγελίας"
                    + Environment.NewLine + ex,"OK");
            }
            await _navigation.PushAsync(new CategoriesPage(current));
        }
    }
}
