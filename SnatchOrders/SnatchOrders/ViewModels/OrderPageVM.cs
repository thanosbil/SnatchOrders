using SnatchOrders.Models;
using SnatchOrders.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SnatchOrders.ViewModels
{
    public class OrderPageVM : ViewModelBase {
        public ICommand GroupTappedCommand { get; set; }
        public ICommand DeleteOrderItemCommand { get; set; }
        public ICommand AddItemsToOrderCommand { get; set; }
        public ICommand DoneEditingOrderCommand { get; set; }

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
            GroupTappedCommand = new Command<OrderItemGroup>(GroupTapped);
            DoneEditingOrderCommand = new Command(DoneEditingOrder);
        }

        private async void DoneEditingOrder() {
            await _Navigation.PushAsync(new ShareOrderPage(_CurrentOrder));
        }

        private void GroupTapped(OrderItemGroup obj) {
            obj.Expanded = !obj.Expanded;
            if (!obj.Expanded) {
                obj.Clear();
            } else {
                foreach(OrderItem item in obj.BackUpList) {
                    obj.Add(item);
                }
            }
            
        }

        private async void AddItemsToOrder() {
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

            await GetOrderItems();
        }

        internal async Task GetOrderItems() {
            try {
                _CurrentOrder.AllItems = await App.Database.GetOrderItemsAsync(_CurrentOrder.ID);
                if(_CurrentOrder.AllItems.Count > 0) {
                    await GroupedCollectionBuilder(_CurrentOrder.AllItems);
                    HasItems = true;
                } else {
                    HasItems = false;
                }

            } catch (Exception ex) {
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την ανάγνωση των ειδών της παραγγελίας"
                  + Environment.NewLine + ex, "OK");
            }
        }


        private async Task GroupedCollectionBuilder(List<OrderItem> dbItems) {
            GroupedOrderItemsCollection.Clear();

            if (dbItems != null && dbItems.Count > 0) {
                var CategoriesFoundInOrder = dbItems.Select(i => i.CategoryId).Distinct();

                foreach (var categoryId in CategoriesFoundInOrder) {
                    // Βρίσκω την κατηγορία 
                    Category orderCategory = await App.Database.GetCategoryAsync(categoryId);
                    // Φτιάχνω το ItemGroup
                    OrderItemGroup itemGroup = new OrderItemGroup(orderCategory.Description, false);
                    // Βρίσκω τα Items της κατηγορίας
                    List<OrderItem> categorizedList = dbItems.Where(i => i.CategoryId == categoryId).ToList();
                    // Βάζω τα Items στο ItemGroup
                    foreach (OrderItem item in categorizedList) {
                        itemGroup.ItemCount += item.Count;
                        itemGroup.Add(item);
                    }
                    // Αντιγράφω τη λίστα 
                    itemGroup.CopyList();
                    // Και αδειάζω το Collection - default κατάσταση !Expanded
                    itemGroup.Clear();
                    // Βάζω το ItemGroup στο Collection
                    GroupedOrderItemsCollection.Add(itemGroup);

                }
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
