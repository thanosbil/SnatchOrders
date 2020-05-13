using SnatchOrders.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SnatchOrders.ViewModels {
    public class ReportResultsPageViewModel : ViewModelBase {
        public ICommand GroupTappedCommand { get; set; }
        public ObservableCollection<OrderItemGroup> GroupedOrderItemsCollection { get; set; }        
        public ReportCriteria Criteria { get; set; }
        private bool _isLoading { get; set; }
        public bool isLoading {
            get { return _isLoading; }
            set {
                if (_isLoading != value) {
                    _isLoading = value;
                    OnPropertyChanged("isLoading");
                    OnPropertyChanged("isLoadingFinished");
                }
            }
        }

        public bool isLoadingFinished { get { return !isLoading; } }

        private bool _hasResults { get; set; }
        public bool HasResults {
            get { return _hasResults; }
            set {
                if (_hasResults != value) {
                    _hasResults = value;
                    OnPropertyChanged("HasResults");
                }
            }
        }

        public INavigation _navigation { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigation"></param>
        public ReportResultsPageViewModel(INavigation navigation, ReportCriteria criteria) {
            _navigation = navigation;
            Criteria = criteria;
            
            GroupedOrderItemsCollection = new ObservableCollection<OrderItemGroup>();
            GroupTappedCommand = new Command<OrderItemGroup>(GroupTapped);
        }

        public async void SearchForResults() {
            isLoading = true;
            List<OrderItem> OrderItemsList;
            
            try {
                OrderItemsList = await App.Database.GetOrderItemsForReportAsync(Criteria.ItemId, Criteria.CategoryId);

                if (OrderItemsList != null) {

                    var CategoriesFoundInOrder = OrderItemsList.Select(i => i.CategoryId).Distinct();

                    foreach (var categoryId in CategoriesFoundInOrder) {
                        // Βρίσκω την κατηγορία 
                        Category orderCategory = await App.Database.GetCategoryAsync(categoryId);
                        // Φτιάχνω το ItemGroup
                        OrderItemGroup itemGroup = new OrderItemGroup(orderCategory.Description, false);
                        // Βρίσκω τα Items της κατηγορίας
                        List<OrderItem> categorizedList = OrderItemsList.Where(i => i.CategoryId == categoryId).ToList();
                        // Βάζω τα Items στο ItemGroup
                        foreach (OrderItem item in categorizedList) {

                            List<OrderItem> SameItemsInCategorizedList = categorizedList.Where(i => i.ItemId == item.ItemId).ToList();
                            int countOfOccurances = SameItemsInCategorizedList.Count;
                            bool isAlreadyAddedinGroup = itemGroup.Where(i => i.ItemId == item.ItemId).ToList().Count > 0;

                            itemGroup.ItemCount += item.Count;

                            if (countOfOccurances > 1 && isAlreadyAddedinGroup) {
                                itemGroup.Where(i => i.ItemId == item.ItemId).ToList()[0].Count += item.Count;
                            }
                            else {
                                itemGroup.Add(item);
                            }                           
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
            catch (Exception ex) {
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την αναζήτηση"
                    + Environment.NewLine + ex, "OK");
            }

            isLoading = false;
        }

        private void GroupTapped(OrderItemGroup obj) {
            obj.Expanded = !obj.Expanded;
            if (!obj.Expanded) {
                obj.Clear();
            }
            else {
                foreach (OrderItem item in obj.BackUpList) {
                    obj.Add(item);
                }
            }
        }
    }
}
