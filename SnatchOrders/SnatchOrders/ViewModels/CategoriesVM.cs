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
    public class CategoriesVM : ViewModelBase
    {
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
        public Order CurrentOrder { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
                
        public ICommand AddCategoryCommand { get; set; }
        public ICommand GoToItemsPageCommand { get; set; }
        public ICommand DeleteCategoryCommand { get; set; }

        private INavigation _navigation;
        private List<Category> ListOfCategories;

        public CategoriesVM(INavigation navigation, Order currentOrder) {
            _navigation = navigation;
            CurrentOrder = currentOrder;
            
            AddCategoryCommand = new Command(CreateNewCategory);
            GoToItemsPageCommand = new Command<Category>(GoToItemsPage);
            DeleteCategoryCommand = new Command<Category>(DeleteCategory);
            Categories = new ObservableCollection<Category>();
            ListOfCategories = new List<Category>();         
        }
        
        private async void DeleteCategory (Category obj) {
            bool result = await App.Current.MainPage.DisplayAlert("Διαγραφή", $"Πρόκειται να διαγραφεί η κατηγορία {obj.Description}. Θέλετε να συνεχίσετε;", "OK", "ΑΚΥΡΟ");

            if (result) {
                try { 
                    await App.Database.DeleteCategoryAsync(obj);
                    Categories.Remove(obj);
                }catch(Exception ex) {
                    await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά τη διαγραφή της κατηγορίας"
                        + Environment.NewLine + ex, "OK");
                }                
            }
        }

        public async void GetCategoriesList() {
            Categories.Clear();
            ListOfCategories.Clear();

            try {
                ListOfCategories = await App.Database.GetCategoriesAsync();
                
                if (ListOfCategories.Count > 0) {
                    foreach (Category item in ListOfCategories) {
                        int itemsPerCategory = App.Database.GetItemsAsync(item.ID).Result.Count;
                        item.ItemCount = itemsPerCategory;
                        Categories.Add(item);
                    }
                    HasItems = true;
                } else {
                    HasItems = false;
                }
            }catch(Exception ex) {
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την ανάγνωση των κατηγοριών προϊόντων"
                    + Environment.NewLine + ex, "OK");
            }
        }

        private async void CreateNewCategory(object obj){
            await _navigation.PushAsync(new CategoryDetailPage());
        }

        private async void GoToItemsPage(Category category){
            await _navigation.PushAsync(new ItemsPage(CurrentOrder, category));
        }
    }
}
