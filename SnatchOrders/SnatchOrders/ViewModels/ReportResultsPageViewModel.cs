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
        private ObservableCollection<ReportItemGroup> groupedReportItemsCollection;
        public ObservableCollection<ReportItemGroup> GroupedReportItemsCollection {
            get { return groupedReportItemsCollection; }
            set {
                groupedReportItemsCollection = value;
                OnPropertyChanged("GroupedReportItemsCollection");
            }
        }      
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
            GroupTappedCommand = new Command<ReportItemGroup>(GroupTapped);
        }

        public async void SearchForResults() {
            isLoading = true;
            GroupedReportItemsCollection.Clear();
            List<Order> OrdersList;
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
                        order.AllItems = await App.Database.GetOrderItemsAsync(order.ID);
                        // Αν ψάχνω συγκεκριμένο είδος και έχω id
                        if (Criteria.ItemId > 0) {
                            matchingItems.Add(order.AllItems.SingleOrDefault(item => item.ItemId == Criteria.ItemId));
                        }
                        // αν ψάχνω σε κατηγορία και έχω id κατηγορίας
                        else if (Criteria.CategoryId > 0) {
                            matchingItems.AddRange(order.AllItems.Where(item => item.CategoryId == Criteria.CategoryId));
                        }
                        // αν θέλω όλα τα είδη
                        else {
                            matchingItems.AddRange(order.AllItems);
                        }
                    }

                    ReportItemGroup ReportGroup;
                    ReportItem reportItem;
                    matchingItems = matchingItems.OrderBy(x => x.OrderId).ToList();
                    
                    foreach (OrderItem orderItem in matchingItems) {                        
                        // Αν δεν υπάρχει group στο collection με αυτό το id κατηγορίας ειδών
                        if (!GroupedReportItemsCollection.Any(group => group.CategoryId == orderItem.CategoryId)) {
                            // βρίσκω την κατηγορία
                            Category category = await App.Database.GetCategoryAsync(orderItem.CategoryId);
                            // φτιάχνω το group με την περιγραφή
                            ReportGroup = new ReportItemGroup(category.Description, true);
                            ReportGroup.CategoryId = category.ID;
                            // και το προσθέτω στο collection
                            GroupedReportItemsCollection.Add(ReportGroup);
                        }
                        else {
                            // Αλλιώς το βρίσκω στο collection
                            ReportGroup = GroupedReportItemsCollection.SingleOrDefault(group => group.CategoryId == orderItem.CategoryId);
                        }

                        // Αν στο group δεν υπάρχει το είδος, το προσθέτω
                        if(!ReportGroup.Any(item => item.ItemId == orderItem.ItemId)) {
                            reportItem = new ReportItem();
                            reportItem.ItemId = orderItem.ItemId;
                            reportItem.CategoryId = orderItem.CategoryId;
                            reportItem.Description = orderItem.Description;
                            reportItem.Quantity = orderItem.Count;
                            reportItem.InNumberOfOrders = 1;

                            ReportGroup.Add(reportItem);
                            ReportGroup.BackUpList.Add(reportItem);
                        }
                        else {
                            // αλλιώς το βρίσκω και προσθέτω την ποσότητα
                            reportItem = ReportGroup.SingleOrDefault(rep => rep.ItemId == orderItem.ItemId);
                            reportItem.Quantity += orderItem.Count;
                            reportItem.InNumberOfOrders++;
                        }
                        // υπολογίζω τη μέση ποσότητα ανά παραγγελία
                        reportItem.OrderAverageQuantity = reportItem.Quantity / reportItem.InNumberOfOrders;
                    }
                }
            }
            catch (Exception ex) {
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την αναζήτηση"
                    + Environment.NewLine + ex, "OK");
            }

            isLoading = false;
        }

        private void GroupTapped(ReportItemGroup obj) {
            obj.Expanded = !obj.Expanded;
            if (!obj.Expanded) {
                obj.Clear();
            }
            else {
                foreach (ReportItem item in obj.BackUpList) {
                    obj.Add(item);
                }
            }
        }
    }
}
