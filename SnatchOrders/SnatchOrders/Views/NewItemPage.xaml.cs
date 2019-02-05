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
	public partial class NewItemPage : ContentPage
	{
        NewItemPageVM newItemPageVM;

		public NewItemPage ()
		{
			InitializeComponent ();
            newItemPageVM = new NewItemPageVM(Navigation);
            BindingContext = newItemPageVM;
		}
	}
}