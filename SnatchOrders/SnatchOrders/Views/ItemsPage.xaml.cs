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
	public partial class ItemsPage : ContentPage
	{
        ItemsPageVM itemsPageVM;

		public ItemsPage (Category category, int OrderID)
		{
			InitializeComponent ();
            Title = category.Description;
            itemsPageVM = new ItemsPageVM(Navigation);
            BindingContext = itemsPageVM;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            itemsPageVM.GetItemsList();
        }

        //private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        //{
        //    //itemsPageVM.GoToNewItemPageCommand.Execute(e.Item);
        //}
    }
}