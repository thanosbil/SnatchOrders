using SnatchOrders.Models;
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
	public partial class CategoriesPage : ContentPage
	{
        CategoriesVM categoriesVM;

        public CategoriesPage() {
            InitializeComponent();
            
            categoriesVM = new CategoriesVM(Navigation);
            BindingContext = categoriesVM;
        }

        public CategoriesPage (Order currentOrder) {
			InitializeComponent ();
            
            categoriesVM = new CategoriesVM(Navigation, currentOrder);
            BindingContext = categoriesVM;
		}

        protected override void OnAppearing() {
            base.OnAppearing();

            categoriesVM.GetCategoriesList();
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e) {
            categoriesVM.GoToItemsPageCommand.Execute(e.Item);
        }
    }
}