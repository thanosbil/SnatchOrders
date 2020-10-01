using SnatchOrders.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SnatchOrders.ViewModels {
    public class ReportResultsPageViewModel : ViewModelBase {
        public ICommand GroupTappedCommand { get; set; }
        private string searchType;
        public string SearchType {
            get { return searchType; }
            set { 
                if(searchType != value) {
                    searchType = value;
                    OnPropertyChanged("SearchType");
                }
            }
        }
        private int _ordersCount { get; set; }
        public int OrdersCount {
            get { return _ordersCount; }
            set { 
                if(_ordersCount != value) {
                    _ordersCount = value;
                    OnPropertyChanged("OrdersCount");
                }
            }
        }
        public ObservableCollection<ReportItemGroup> GroupedReportItemsCollection { get; set; }        
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

            GroupedReportItemsCollection = new ObservableCollection<ReportItemGroup>();
            GroupTappedCommand = new Command<OrderItemGroup>(GroupTapped);
        }

        public async void SearchForResults() {
            isLoading = true;
            GroupedReportItemsCollection.Clear();
            List<Order> OrdersList;
            List<ReportItemGroup> unsorted = new List<ReportItemGroup>();
            List<ReportItem> ReportItemsList = new List<ReportItem>();
            List<OrderItem> matchingItems = new List<OrderItem>();
            try {
                // Βρίσκω τις εγγραφές στη χρονική περίοδο που ορίζουν τα κριτήρια               
                OrdersList = await App.Database.GetOrdersInTimePeriodAsync(Criteria);

                if(OrdersList != null) {
                    switch (Criteria.StatusCriteria) {
                        case StatusOfOrder.Finished:
                        SearchType = "Όλα";
                        OrdersList = OrdersList.Where(order => order.OrderStatus >= StatusOfOrder.Finished).ToList();
                        break;

                        case StatusOfOrder.SentViaMail:
                        SearchType = "Email";
                        OrdersList = OrdersList.Where(order => order.OrderStatus == StatusOfOrder.SentViaMail).ToList();
                        break;

                        case StatusOfOrder.SentOther:
                        SearchType = "Άλλη μέθοδο";
                        OrdersList = OrdersList.Where(order => order.OrderStatus == StatusOfOrder.SentOther).ToList();
                        break;
                    }

                    OrdersCount = OrdersList.Count;

                    // Ψάχνω σε κάθε παραγγελία που ταιριάζει στα κριτήρια
                    foreach (Order order in OrdersList) {
                        // Αν ψάχνω συγκεκριμένο είδος και έχω id
                        if (Criteria.ItemId > 0) {
                            matchingItems.Add(order.AllItems.SingleOrDefault(item => item.ItemId == Criteria.ItemId));
                        }
                        // αν ψάχνω σε κατηγορία και έχω id κατηγορίας
                        else if(Criteria.CategoryId > 0) {
                            matchingItems.AddRange(order.AllItems.Where(item => item.CategoryId == Criteria.CategoryId));
                        }
                        // αν θέλω όλα τα είδη
                        else {
                            matchingItems.AddRange(order.AllItems);
                        }
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
