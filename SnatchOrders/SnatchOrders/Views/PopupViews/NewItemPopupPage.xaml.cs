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
	public partial class NewItemPopupPage : Rg.Plugins.Popup.Pages.PopupPage {
        private int _CategoryId;
        public NewItemPopupPage (int categoryId)
		{
			InitializeComponent ();
            _CategoryId = categoryId;
		}

        private async void Ok_Button_Clicked(object sender, EventArgs e) {            
            if (!string.IsNullOrEmpty(Description.Text)) {
                Item newItem = new Item();
                newItem.Description = Description.Text.Trim();
                newItem.CategoryId = _CategoryId;

                try {
                    await App.Database.SaveItemAsync(newItem);
                    MessagingCenter.Send(this, "Added");
                } catch (Exception ex) {
                    await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την αποθήκευση του είδους"
                        + Environment.NewLine + ex, "OK");
                }
            }
            await Navigation.PopPopupAsync();
        }

        private async void Cancel_Button_Clicked(object sender, EventArgs e) {
            await Navigation.PopPopupAsync();
        }
    }
}