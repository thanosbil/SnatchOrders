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
        }

        private void IncreaseCount(Item obj) {
            obj.Count += 1;
        }

        private void DecreaseCount(Item obj) {
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
