using Rg.Plugins.Popup.Extensions;
using SnatchOrders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnatchOrders.Views.PopupViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewEmailAccountPopupPage : Rg.Plugins.Popup.Pages.PopupPage {
		public NewEmailAccountPopupPage ()
		{
			InitializeComponent ();
		}

        private async void Cancel_Button_Clicked(object sender, EventArgs e) {
            await Navigation.PopPopupAsync();
        }

        private async void Ok_Button_Clicked(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(EmailAddress.Text)) {
                EmailAccount newEmail = new EmailAccount();
                newEmail.Email = EmailAddress.Text.Trim();
                newEmail.DateSaved = DateTime.Now;

                try {
                    await App.Database.SaveEmailAcountAsync(newEmail);
                    MessagingCenter.Send(this, "Added");

                } catch (Exception ex) {
                    await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την αποθήκευση του email"
                        + Environment.NewLine + ex, "OK");
                }
            }
            await Navigation.PopPopupAsync();
        }
    }
}