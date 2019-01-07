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
	public partial class CategoryDetailPage : ContentPage
	{
        CategoryDetailVM categoryDetailVM;

        public CategoryDetailPage ()
		{
			InitializeComponent ();
            Title = "Νέα κατηγορία";
            categoryDetailVM = new CategoryDetailVM(Navigation);
            BindingContext = categoryDetailVM;
		}
	}
}