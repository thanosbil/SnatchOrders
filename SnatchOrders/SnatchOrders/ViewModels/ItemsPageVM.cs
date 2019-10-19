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
    public class ItemsPageVM : ViewModelBase {
        public ICommand AddItemCommand { get; set; }
        public ICommand DecreaseCountCommand { get; set; }
        public ICommand IncreaseCountCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }
        public ICommand AddItemsToOrderCommand { get; set; }

        private INavigation _Navigation { get; set; }
        private int CategoryId { get; set; }
        public List<Item> DbItems { get; set; }
        public ObservableCollection<Item> ItemsCollection { get; set; }

        public ItemsPageVM(INavigation navigation, int orderId, int categoryId) {
            _Navigation = navigation;

            ItemsCollection = new ObservableCollection<Item>();

            CategoryId = categoryId;
            AddItemCommand = new Command(AddItem);
            DecreaseCountCommand = new Command<Item>(DecreaseCount);
            IncreaseCountCommand = new Command<Item>(IncreaseCount);
            DeleteItemCommand = new Command<Item>(DeleteItem);
            AddItemsToOrderCommand = new Command(AddItemsToOrder);
        }

        private void AddItemsToOrder(object obj) {
            
        }

        private async void DeleteItem(Item obj) {
            bool result = await App.Current.MainPage.DisplayAlert("Διαγραφή", $"Πρόκειται να διαγραφεί το είδος {obj.Description}. Θέλετε να συνεχίσετε;", "OK", "ΑΚΥΡΟ");

            try {
                if (result) {
                    await App.Database.DeleteItemAsync(obj);
                    ItemsCollection.Remove(obj);
                }
            }catch(Exception ex) {
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την διαγραφή του είδους"
                    + Environment.NewLine + ex, "OK");
            }
        }

        private void IncreaseCount(Item obj) {
            obj.Count += 1;
        }

        private void DecreaseCount(Item obj) {
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
            if (dbItems != null) {
                foreach(Item categoryItem in dbItems) {
                    ItemsCollection.Add(categoryItem);
                }
            }
        }

        private async void AddItem() {
            await _Navigation.PushAsync(new NewItemPage(CategoryId));
        }
    }
}
