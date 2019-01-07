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
	public partial class OrdersPage : ContentPage
	{
        OrdersVM ordersVM;

		public OrdersPage ()
		{
			InitializeComponent ();
            ordersVM = new OrdersVM(Navigation);
            BindingContext = ordersVM;
            Title = "Παραγγελίες";
		}
	}
}