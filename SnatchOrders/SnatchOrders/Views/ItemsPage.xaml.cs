using SnatchOrders.Models;
using SnatchOrders.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        int CategoryId;
        ItemsPageVM itemsPageVM;

		public ItemsPage (Order currentOrder, Category currentCategory, bool isMenuAction){
			InitializeComponent ();
            itemsPageVM = new ItemsPageVM(Navigation, currentOrder, currentCategory, isMenuAction);
            BindingContext = itemsPageVM;
            Title = currentCategory.Description;
            CategoryId = currentCategory.ID;
		}

        protected override async void OnAppearing() {
            base.OnAppearing();
            
            await itemsPageVM.GetCategoryItems(CategoryId);            
        }        
    }
}