using Rg.Plugins.Popup.Extensions;
using SnatchOrders.Models;
using SnatchOrders.Views;
using SnatchOrders.Views.PopupViews;
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
        private ObservableCollection<Category> _categoriesCollection { get; set; }
        public ObservableCollection<Category> CategoriesCollection {
            get { return _categoriesCollection; }
            set {
                _categoriesCollection = value;
                OnPropertyChanged("CategoriesCollection");
            }
        }
                
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
            CategoriesCollection = new ObservableCollection<Category>();
            ListOfCategories = new List<Category>();

            MessagingCenter.Subscribe<NewCategoryPopupPage>(this, "Added", (sender) => {
                RefreshCategories();
            });
        }

        private void RefreshCategories() {
            GetCategoriesList();
        }
        
        private async void DeleteCategory (Category obj) {
            bool result = await App.Current.MainPage.DisplayAlert("Διαγραφή", $"Πρόκειται να διαγραφεί η κατηγορία {obj.Description}. Θέλετε να συνεχίσετε;", "OK", "ΑΚΥΡΟ");

            if (result) {
                try { 
                    await App.Database.DeleteCategoryAsync(obj);
                    CategoriesCollection.Remove(obj);
                }catch(Exception ex) {
                    await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά τη διαγραφή της κατηγορίας"
                        + Environment.NewLine + ex, "OK");
                }                
            }
        }

        public async void GetCategoriesList() {
            CategoriesCollection.Clear();
            ListOfCategories.Clear();

            try {
                ListOfCategories = await App.Database.GetCategoriesAsync();
                
                if (ListOfCategories.Count > 0) {
                    foreach (Category item in ListOfCategories) {
                        int itemsPerCategory = App.Database.GetItemsAsync(item.ID).Result.Count;
                        item.ItemCount = itemsPerCategory;
                        CategoriesCollection.Add(item);
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
            await _navigation.PushPopupAsync(new NewCategoryPopupPage());
        }

        private async void GoToItemsPage(Category category){
            await _navigation.PushAsync(new ItemsPage(CurrentOrder, category));
        }
    }
}
