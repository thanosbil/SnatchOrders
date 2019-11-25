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
        public ICommand DeleteCategoryCommand { get; set; }
        public ICommand AddItemsToOrderCommand { get; set; }

        private bool _hasItems { get; set; }
        public bool HasItems {
            get { return _hasItems; }
            set {
                if (_hasItems != value) {
                    _hasItems = value;
                    OnPropertyChanged("HasItems");
                    OnPropertyChanged("HasItemsAndIsNotMenuAction");
                }
            }
        }        
        public bool IsMenuAction { get; set; }
        public bool IsNotMenuAction { get { return !IsMenuAction; } }
        public bool HasItemsAndIsNotMenuAction { get { return HasItems && IsNotMenuAction; } }
        private INavigation _Navigation { get; set; }
        private Category CurrentCategory { get; set; }        
        public Order CurrentOrder { get; set; }
        public List<Item> DbItems { get; set; }
        private ObservableCollection<OrderItem> _itemsCollection { get; set; }
        public ObservableCollection<OrderItem> ItemsCollection {
            get { return _itemsCollection; }
            set {
                _itemsCollection = value;
                OnPropertyChanged("ItemsCollection");
            }
        }
        
        public ItemsPageVM(INavigation navigation, Order currentOrder, Category currentCategory, bool isMenuAction) {
            _Navigation = navigation;

            ItemsCollection = new ObservableCollection<OrderItem>();

            IsMenuAction = isMenuAction;
            CurrentCategory = currentCategory;
            CurrentOrder = currentOrder;
            AddItemCommand = new Command(AddItem);
            DecreaseCountCommand = new Command<OrderItem>(DecreaseCount);
            IncreaseCountCommand = new Command<OrderItem>(IncreaseCount);
            DeleteItemCommand = new Command<OrderItem>(DeleteItem);
            DeleteCategoryCommand = new Command(DeleteCategory);
            AddItemsToOrderCommand = new Command(AddItemsToOrder);

            MessagingCenter.Subscribe<NewItemPopupPage, Item>(this, "Added", (sender, addedItem) => {
                GetAddedItem(addedItem);
            });
        }

        private void GetAddedItem(Item added) {
            OrderItem newItem = new OrderItem();
            newItem.ItemId = added.ID;
            newItem.CategoryId = added.CategoryId;
            newItem.Description = added.Description;

            DbItems.Add(added);
            ItemsCollection.Add(newItem);
            ItemsCollection = new ObservableCollection<OrderItem>(ItemsCollection.OrderBy(i => i.Description));
            HasItems = true;
        }

        /// <summary>
        /// Προσθέτει τα είδη στην παραγγελία
        /// </summary>
        private async void AddItemsToOrder() {
            bool hasAddedItems = CheckHasItems(ItemsCollection);
            bool result = false;
            bool saveOrderOk = false;

            if (IsNotMenuAction) {
                if(CurrentOrder.ID == 0) {
                    saveOrderOk = await SaveOrder();

                    if (!saveOrderOk) {
                        return;
                    }
                }
                if (hasAddedItems)
                    result = await App.Current.MainPage.DisplayAlert("Επιβεβαίωση", "Εισαγωγή των ειδών στην παραγγελία;", "OK", "ΑΚΥΡΟ");
                else
                    await App.Current.MainPage.DisplayAlert("Προσοχή", "Δεν έχετε προσθέσει ποσότητα σε κάποιο είδος", "OK");

                if (result) {
                    try {
                        foreach (OrderItem item in ItemsCollection) {
                            if (item.Count > 0) {
                                item.OrderId = CurrentOrder.ID;                                
                                CurrentOrder.AllItems.Add(item);
                                await App.Database.SaveOrderItemAsync(item);
                            }
                        }

                        if (CurrentOrder.AllItems.Count > 0 && CurrentOrder.OrderStatus == StatusOfOrder.New) {
                            CurrentOrder.OrderStatus = StatusOfOrder.InProgress;
                            await App.Database.SaveOrderAsync(CurrentOrder);
                        }

                    } catch (Exception ex) {
                        await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την αποθήκευση των ειδών της παραγγελίας"
                            + Environment.NewLine + ex, "OK");
                    }
                    await _Navigation.PopAsync();
                }
            } else {
                await _Navigation.PopAsync();
            }
        }

        private async Task<bool> SaveOrder() {
            bool lRet = false;
            int affected = 0;
            try {
                affected = await App.Database.SaveOrderAsync(CurrentOrder);

                if (affected > 0)
                    lRet = true;

            } catch (Exception ex) {
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την αποθήκευση της παραγγελίας"
                    + Environment.NewLine + ex, "OK");
            }

            return lRet;
        }

        /// <summary>
        /// Διαγράφει το είδος
        /// </summary>
        /// <param name="obj"></param>
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

        /// <summary>
        /// Διαγράφει την κατηγορία και τα είδη της
        /// </summary>
        /// <param name="obj"></param>
        private async void DeleteCategory() {
            bool result = await App.Current.MainPage.DisplayAlert("Διαγραφή", $"Πρόκειται να διαγραφεί η κατηγορία {CurrentCategory.Description}. Θέλετε να συνεχίσετε;", "OK", "ΑΚΥΡΟ");

            if (result) {
                try {
                    await App.Database.DeleteCategoryAsync(CurrentCategory);
                    await _Navigation.PopAsync();
                } catch (Exception ex) {
                    await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά τη διαγραφή της κατηγορίας"
                        + Environment.NewLine + ex, "OK");
                }
            }
        }

        /// <summary>
        /// Αυξάνει την ποσότητα του είδους
        /// </summary>
        /// <param name="obj"></param>
        private void IncreaseCount(OrderItem obj) {
            obj.Count += 1;
        }

        /// <summary>
        /// Μειώνει την ποσότητα του είδους
        /// </summary>
        /// <param name="obj"></param>
        private void DecreaseCount(OrderItem obj) {
            if (obj.Count > 0)
                obj.Count -= 1; 
        }

        /// <summary>
        /// Φέρνει από τη database τα είδη της κατηγορίας
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
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
            OrderItem temp = null;

            if (IsMenuAction) {
                temp = new OrderItem();
                temp.ItemId = categoryItem.ID;
                temp.CategoryId = categoryItem.CategoryId;
                temp.Description = categoryItem.Description;                
            } else {
                if (CurrentOrder.AllItems.Exists(x => x.ItemId == categoryItem.ID)) {
                    temp = CurrentOrder.AllItems.FirstOrDefault(x => x.ItemId == categoryItem.ID);
                } else {
                    temp = new OrderItem();
                    temp.ItemId = categoryItem.ID;
                    temp.CategoryId = categoryItem.CategoryId;
                    temp.Description = categoryItem.Description;
                }
            }                  

            return temp;
        }

        private async void AddItem() {
            //            await _Navigation.PushAsync(new NewItemPage(CategoryId));
            await _Navigation.PushPopupAsync(new NewItemPopupPage(CurrentCategory.ID));
        }

        private bool CheckHasItems(ObservableCollection<OrderItem> itemsCollection) {
            bool lRet = false;

            foreach(OrderItem item in itemsCollection) {
                if(item.Count > 0) {
                    lRet = true;
                    break;
                }
            }

            return lRet;
        }
    }
}
