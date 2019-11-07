using SnatchOrders.Models;
using SnatchOrders.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnatchOrders.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ShareOrderPage : ContentPage
	{
        ShareOrderPageVM shareOrderPageVM;

		public ShareOrderPage (Order currentOrder){
			InitializeComponent ();
            shareOrderPageVM = new ShareOrderPageVM(Navigation, currentOrder);
            BindingContext = shareOrderPageVM;
		}

        protected override void OnAppearing() {
            base.OnAppearing();

            shareOrderPageVM.GetEmailList();
        }
        
        private void Entry_TextChanged(object sender, TextChangedEventArgs e) {
            Preferences.Set("MailSubject", e.NewTextValue);
        }
        
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e) {
            if (!shareOrderPageVM.HasItems) {
                await DisplayAlert("Προσοχή", "Δεν βρέθηκε αποθηκευμένη διεύθυνση email. Επιλέξτε από το μενού " +
                    "την ενότητα \"Διευθύνσεις email\" για να δημιουργήσετε μια διεύθυνση.", "OK");
            }
        }
    }
}