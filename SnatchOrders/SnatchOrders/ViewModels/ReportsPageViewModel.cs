using Rg.Plugins.Popup.Extensions;
using SnatchOrders.Models;
using SnatchOrders.Views.PopupViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SnatchOrders.ViewModels
{
    public class ReportsPageViewModel : ViewModelBase {
        public ICommand OpenFilterPopupCommand { get; set; }
        public INavigation _navigation { get; set; }
        public List<Category> ListOfCategories { get; set; }
        public ObservableCollection<Category> CategoriesCollection { get; set; }
        public Category SelectedCategory { get; set; }
        public List<Item> ListOfItems { get; set; }
        public ObservableCollection<Item> ItemsCollection { get; set; }
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigation"></param>
        public ReportsPageViewModel(INavigation navigation) {
            _navigation = navigation;
            CategoriesCollection = new ObservableCollection<Category>();
            ItemsCollection = new ObservableCollection<Item>();
        }

        public async void GetSavedCategories() {
            CategoriesCollection.Clear();
            
            try {
                ListOfCategories = await App.Database.GetCategoriesAsync();

                if (ListOfCategories != null) {
                    foreach (Category item in ListOfCategories) {
                        int itemsPerCategory = App.Database.GetItemsAsync(item.ID).Result.Count;
                        item.ItemCount = itemsPerCategory;
                        CategoriesCollection.Add(item);
                    }                    
                }                
            }
            catch (Exception ex) {
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την ανάγνωση των κατηγοριών προϊόντων"
                    + Environment.NewLine + ex, "OK");
            }
        }

        public async void GetSavedItems() {
            ItemsCollection.Clear();
                        
            try {
                if (SelectedCategory != null) {
                    ListOfItems = await App.Database.GetItemsAsync(SelectedCategory.ID);
                }
                else {
                    ListOfItems = await App.Database.GetAllItemsAsync();
                }

                if(ListOfItems != null) {
                    foreach(Item item in ListOfItems) {
                        ItemsCollection.Add(item);
                    }
                }
            }
            catch (Exception ex) {
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την ανάγνωση των αποθηκευμένων ειδών"
                    + Environment.NewLine + ex, "OK");
            }            
        }
    }
}
