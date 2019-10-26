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
	public partial class OrderPage : ContentPage
	{
        OrderPageVM orderPageVM;

		public OrderPage (Order currentOrder)
		{
			InitializeComponent ();
            orderPageVM = new OrderPageVM(Navigation, currentOrder);
            BindingContext = orderPageVM;
            Title = currentOrder.DateCreated.ToString("dd/MM/yy - HH:mm");
		}

        protected override async void OnAppearing() {
            base.OnAppearing();

            await orderPageVM.GetOrderItems();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e) {
            var grid = sender as Grid;
            var group = grid.BindingContext as OrderItemGroup;            
            orderPageVM.GroupTappedCommand.Execute(group);
        }
    }
}