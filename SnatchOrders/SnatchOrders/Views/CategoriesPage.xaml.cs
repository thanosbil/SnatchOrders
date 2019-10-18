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

		public CategoriesPage (int orderId) {
			InitializeComponent ();
            Title = "Κατηγορίες";
            categoriesVM = new CategoriesVM(Navigation, orderId);
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