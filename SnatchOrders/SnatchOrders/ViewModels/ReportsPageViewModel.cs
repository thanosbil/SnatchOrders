using Rg.Plugins.Popup.Extensions;
using SnatchOrders.Models;
using SnatchOrders.Views;
using SnatchOrders.Views.PopupViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SnatchOrders.ViewModels
{
    public class ReportsPageViewModel : ViewModelBase {
        public ICommand RadioButtonTappedCommand { get; set; }
        public ICommand SearchForReportResultsCommand { get; set; }
        public bool _isEmail { get; set; }
        public bool IsEmail {
            get { return _isEmail; } 
            set {
                if(_isEmail != value) {
                    _isEmail = value;
                    OnPropertyChanged("IsEmail");
                }
            }
        }
        public bool _isOther { get; set; }
        public bool IsOther {
            get { return _isOther; }
            set {
                if (_isOther != value) {
                    _isOther = value;
                    OnPropertyChanged("IsOther");
                }
            }
        }
        public bool _isAllSent { get; set; }
        public bool IsAllSent {
            get { return _isAllSent; }
            set {
                if (_isAllSent != value) {
                    _isAllSent = value;
                    OnPropertyChanged("IsAllSent");
                }
            }
        }
        public INavigation _navigation { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<Category> ListOfCategories { get; set; }
        public ObservableCollection<Category> CategoriesCollection { get; set; }
        public Category SelectedCategory { get; set; }
        public Item SelectedItem { get; set; }
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

            SearchForReportResultsCommand = new Command(SearchForReportResults);
            RadioButtonTappedCommand = new Command<StatusOfOrder>(RadioButtonTapped);

            CategoriesCollection = new ObservableCollection<Category>();
            ItemsCollection = new ObservableCollection<Item>();
            DateFrom = DateTime.Today;
            DateTo = DateTime.Today;
            PageInit();
        }

        private void PageInit() {
            int orderCrit = Preferences.Get("ReportSetting", 0);

            if (orderCrit > 0)
                SetRadioButtonValue((StatusOfOrder)orderCrit);
            else
                SetRadioButtonValue(StatusOfOrder.SentViaMail);
        }

        private void SetRadioButtonValue(StatusOfOrder status) {
            switch (status) {
                case StatusOfOrder.Finished:
                IsAllSent = true;
                IsEmail = false;
                IsOther = false;
                break;

                case StatusOfOrder.SentViaMail:
                IsEmail = true;
                IsAllSent = false;
                IsOther = false;
                break;

                case StatusOfOrder.SentOther:
                IsOther = true;
                IsAllSent = false;
                IsEmail = false;
                break;
            }
        }

        private void RadioButtonTapped(StatusOfOrder status) {
            // Κρατάω την επιλογή του χρήστη
            Preferences.Set("ReportSetting", (int)status);
            SetRadioButtonValue(status);
        }

        private void SearchForReportResults() {
            ReportCriteria criteria = new ReportCriteria();

            if(SelectedItem != null) {
                criteria.ItemId = SelectedItem.ID;
            }
            else if(SelectedCategory != null) {
                criteria.CategoryId = SelectedCategory.ID;
            }

            criteria.DateFrom = DateFrom;
            criteria.DateTo = DateTo.AddHours(23).AddMinutes(59).AddSeconds(59);

            _navigation.PushAsync(new ReportResultsPage(criteria));
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
