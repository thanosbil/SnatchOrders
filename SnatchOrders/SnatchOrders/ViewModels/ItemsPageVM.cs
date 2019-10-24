using Rg.Plugins.Popup.Extensions;
using SnatchOrders.Models;
using SnatchOrders.Views;
using SnatchOrders.Views.PopupViews;
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
    public class ItemsPageVM : ViewModelBase {
        public ICommand AddItemCommand { get; set; }
        public ICommand DecreaseCountCommand { get; set; }
        public ICommand IncreaseCountCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }
        public ICommand AddItemsToOrderCommand { get; set; }

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
        private INavigation _Navigation { get; set; }
        private int CategoryId { get; set; }        
        public Order CurrentOrder { get; set; }
        public List<Item> DbItems { get; set; }
        public ObservableCollection<OrderItem> ItemsCollection { get; set; }

        public ItemsPageVM(INavigation navigation, Order currentOrder, int categoryId) {
            _Navigation = navigation;

            ItemsCollection = new ObservableCollection<OrderItem>();

            CategoryId = categoryId;
            CurrentOrder = currentOrder;
            AddItemCommand = new Command(AddItem);
            DecreaseCountCommand = new Command<OrderItem>(DecreaseCount);
            IncreaseCountCommand = new Command<OrderItem>(IncreaseCount);
            DeleteItemCommand = new Command<OrderItem>(DeleteItem);
            AddItemsToOrderCommand = new Command(AddItemsToOrder);

            MessagingCenter.Subscribe<NewItemPopupPage>(this, "Added", (sender) => {
                RefreshItems();
            });
        }

        private async void RefreshItems() {
            await GetCategoryItems(CategoryId);
        }

        private async void AddItemsToOrder() {
            bool result = await App.Current.MainPage.DisplayAlert("Επιβεβαίωση", "Εισαγωγή των ειδών στην παραγγελία;", "OK", "ΑΚΥΡΟ");

            if (result) {
                try {
                    foreach (OrderItem item in ItemsCollection) {
                        if (item.Count > 0) {
                            item.OrderId = CurrentOrder.ID;
                            CurrentOrder.AllItems.Add(item);
                            await App.Database.SaveOrderItemAsync(item);
                        }
                    }

                    if(CurrentOrder.AllItems.Count > 0) {
                        CurrentOrder.OrderStatus = StatusOfOrder.InProgress;
                        await App.Database.SaveOrderAsync(CurrentOrder);
                    }
                    
                } catch (Exception ex) {
                    await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την αποθήκευση των ειδών της παραγγελίας"
                        + Environment.NewLine + ex, "OK");
                }
                await _Navigation.PopAsync();
            }
        }

        private async void DeleteItem(OrderItem obj) {
            bool result = await App.Current.MainPage.DisplayAlert("Διαγραφή", $"Πρόκειται να διαγραφεί το είδος {obj.Description}. Θέλετε να συνεχίσετε;", "OK", "ΑΚΥΡΟ");

            try {
                if (result) {
                    Item oneToDelete = DbItems.FirstOrDefault(i => i.ID == obj.ItemId);
                    await App.Database.DeleteItemAsync(oneToDelete);
                    ItemsCollection.Remove(obj);
                }
            }catch(Exception ex) {
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την διαγραφή του είδους"
                    + Environment.NewLine + ex, "OK");
            }
        }

        private void IncreaseCount(OrderItem obj) {
            obj.Count += 1;
        }

        private void DecreaseCount(OrderItem obj) {
            if (obj.Count > 0)
                obj.Count -= 1; 
        }

        public async Task GetCategoryItems(int categoryId) {            
            try {
                DbItems = await App.Database.GetItemsAsync(categoryId);
            }catch(Exception ex) {
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την ανάγνωση των αποθηκευμένων ειδών"
                    + Environment.NewLine + ex, "OK");
            }

            ConverToObservable(DbItems);
        }

        private void ConverToObservable(List<Item> dbItems) {
            ItemsCollection.Clear();
            OrderItem tempItem;

            if (dbItems != null) {
                foreach(Item categoryItem in dbItems) {
                    tempItem = GetOrderItem(categoryItem);
                    ItemsCollection.Add(tempItem);
                }

                if(ItemsCollection.Count > 0) {
                    HasItems = true;
                } else {
                    HasItems = false;
                }
            }
        }

        private OrderItem GetOrderItem(Item categoryItem) {
            OrderItem temp = new OrderItem();

            temp.ItemId = categoryItem.ID;
            temp.Description = categoryItem.Description;

            return temp;
        }

        private async void AddItem() {
            //            await _Navigation.PushAsync(new NewItemPage(CategoryId));
            await _Navigation.PushPopupAsync(new NewItemPopupPage(CategoryId));
        }
    }
}
