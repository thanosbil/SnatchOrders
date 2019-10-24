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
    public class OrderPageVM : ViewModelBase {
        public ICommand DeleteOrderItemCommand { get; set; }
        public ICommand AddItemsToOrderCommand { get; set; }
        public INavigation _Navigation { get; set; }
        private bool _hasItems { get; set; }
        public bool HasItems {
            get { return _hasItems; }
            set {
                if (_hasItems != value) {
                    _hasItems = value;
                    OnPropertyChanged("HasItems");
                }
            }
        }
        public Order _CurrentOrder { get; set; }
        public ObservableCollection<OrderItem> OrderItemsCollection { get; set; }
        public ObservableCollection<OrderItemGroup> GroupedOrderItemsCollection { get; set; }

        public OrderPageVM(INavigation navigation, Order currentOrder) {
            _Navigation = navigation;
            _CurrentOrder = currentOrder;
            OrderItemsCollection = new ObservableCollection<OrderItem>();
            GroupedOrderItemsCollection = new ObservableCollection<OrderItemGroup>();

            DeleteOrderItemCommand = new Command<OrderItem>(DeleteOrderItem);
            AddItemsToOrderCommand = new Command(AddItemsToOrder);
        }

        private async void AddItemsToOrder(object obj) {
            await _Navigation.PushAsync(new CategoriesPage(_CurrentOrder));
        }

        private async void DeleteOrderItem(OrderItem obj) {
            bool result = await App.Current.MainPage.DisplayAlert("Διαγραφή", "Πρόκειται να διαγραφεί το είδος από την παραγγελία. Θέλετε να συνεχίσετε;", "OK", "ΑΚΥΡΟ");
            try {
                if(result)
                    await App.Database.DeleteOrderItemAsync(obj);
            }catch(Exception ex) {
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά τη διαγραφή του είδους"
                  + Environment.NewLine + ex, "OK");
            }
        }

        internal async Task GetOrderItems() {
            try {
                _CurrentOrder.AllItems = await App.Database.GetOrderItemsAsync(_CurrentOrder.ID);
                if(_CurrentOrder.AllItems.Count > 0) {
                    ConvertToObservable(_CurrentOrder.AllItems);
                    HasItems = true;
                } else {
                    HasItems = false;
                }

            } catch (Exception ex) {
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την ανάγνωση των ειδών της παραγγελίας"
                  + Environment.NewLine + ex, "OK");
            }
        }

        private void ConvertToObservable(List<OrderItem> allItems) {
            OrderItemsCollection.Clear();
            foreach(OrderItem item in allItems) {
                OrderItemsCollection.Add(item);
            }
        }
    }
}
