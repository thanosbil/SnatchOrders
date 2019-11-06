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
	public partial class MailSettingsPage : ContentPage
	{
        MailSettingsPageVM mailSettingsPageVM;

		public MailSettingsPage ()
		{
			InitializeComponent ();
            mailSettingsPageVM = new MailSettingsPageVM(Navigation);
            BindingContext = mailSettingsPageVM;
		}

        protected override void OnAppearing() {
            base.OnAppearing();

            mailSettingsPageVM.GetDbEmailAccounts();
        }
    }
}