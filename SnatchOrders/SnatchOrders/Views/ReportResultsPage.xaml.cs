using SnatchOrders.Models;
using SnatchOrders.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnatchOrders.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportResultsPage : ContentPage {
        ReportResultsPageViewModel reportResultsPageViewModel;

        public ReportResultsPage(ReportCriteria criteria) {
            InitializeComponent();
            reportResultsPageViewModel = new ReportResultsPageViewModel(Navigation, criteria);
            BindingContext = reportResultsPageViewModel;
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            reportResultsPageViewModel.SearchForResults();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e) {
            var grid = sender as Grid;
            var group = grid.BindingContext as ReportItemGroup;
            reportResultsPageViewModel.GroupTappedCommand.Execute(group);
        }
    }
}