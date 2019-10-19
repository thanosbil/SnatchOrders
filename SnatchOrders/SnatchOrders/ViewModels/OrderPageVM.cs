using SnatchOrders.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SnatchOrders.ViewModels
{
    public class OrderPageVM {
        public ICommand DeleteOrderItemCommand { get; set; }
        public INavigation _Navigation { get; set; }
        public Order _CurrentOrder { get; set; }
        public ObservableCollection<OrderItem> OrderItemsCollection { get; set; }

        public OrderPageVM(INavigation navigation, Order currentOrder) {
            _Navigation = navigation;
            _CurrentOrder = currentOrder;
            OrderItemsCollection = new ObservableCollection<OrderItem>();

            DeleteOrderItemCommand = new Command<OrderItem>(DeleteOrderItem);
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
