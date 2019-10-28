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
	public partial class NewCategoryPopupPage : Rg.Plugins.Popup.Pages.PopupPage
	{
		public NewCategoryPopupPage ()
		{
			InitializeComponent ();
		}

        private async void Ok_Button_Clicked(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(Description.Text)) {
                Category newCategory = new Category();
                newCategory.Description = Description.Text.Trim();
                
                try {
                    await App.Database.SaveCategoryAsync(newCategory);
                    MessagingCenter.Send(this, "Added");

                } catch (Exception ex) {
                    await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την αποθήκευση της κατηγορίας"
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