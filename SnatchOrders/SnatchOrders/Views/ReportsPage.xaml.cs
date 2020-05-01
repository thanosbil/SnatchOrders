using SnatchOrders.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnatchOrders.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportsPage : ContentPage {
        ReportsPageViewModel reportsPageViewModel;
        
        public ReportsPage() {
            InitializeComponent();
            reportsPageViewModel = new ReportsPageViewModel(Navigation);
            BindingContext = reportsPageViewModel;
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            reportsPageViewModel.GetSavedCategories();
            reportsPageViewModel.GetSavedItems();
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e) {
            reportsPageViewModel.GetSavedItems();
        }
    }
}