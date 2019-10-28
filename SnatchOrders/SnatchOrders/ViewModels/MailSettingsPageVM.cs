using Rg.Plugins.Popup.Extensions;
using SnatchOrders.Models;
using SnatchOrders.Views.PopupViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SnatchOrders.ViewModels
{
    public class MailSettingsPageVM : ViewModelBase
    {
        public ICommand AddMailAccountCommand { get; set; }
        public ICommand DeleteEmailCommand { get; set; }
        public INavigation _Navigation { get; set; }
        public ObservableCollection<EmailAccount> EmailAccountsCollection { get; set; }
        private bool _hasItems { get; set; }
        public bool HasItems {
            get { return _hasItems; }
            set {
                if (_hasItems != value) {
                    _hasItems = value;
                    OnPropertyChanged("HasItems");
                }
            }
        }

        public MailSettingsPageVM(INavigation navigation) {
            _Navigation = navigation;
            EmailAccountsCollection = new ObservableCollection<EmailAccount>();

            AddMailAccountCommand = new Command(AddMailAccount);
            DeleteEmailCommand = new Command<EmailAccount>(DeleteEmail);

            MessagingCenter.Subscribe<NewEmailAccountPopupPage>(this, "Added", (sender) => {
                RefreshCategories();
            });
        }

        private async void DeleteEmail(EmailAccount obj) {
            bool result = await App.Current.MainPage.DisplayAlert("Διαγραφή", $"Πρόκειται να διαγραφεί η διεύθυνση {obj.Email}. Θέλετε να συνεχίσετε;", "OK", "ΑΚΥΡΟ");

            if (result) {
                try {
                    await App.Database.DeleteEmailAcountAsync(obj);
                    EmailAccountsCollection.Remove(obj);
                } catch (Exception ex) {
                    await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά τη διαγραφή του email"
                        + Environment.NewLine + ex, "OK");
                }
            }
        }

        public async void GetDbEmailAccounts() {
            try {
                List<EmailAccount> dbList = await App.Database.GetEmailAcountsAsync();
                if(dbList != null) {
                    ConvertListToCollection(dbList);                    
                }
            }catch(Exception ex) {
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την ανάγνωση των διευθύνσεων email"
                    + Environment.NewLine + ex, "OK");
            }
        }

        private void ConvertListToCollection(List<EmailAccount> dbList) {
            EmailAccountsCollection.Clear();
            if (dbList.Count > 0) {
                foreach (EmailAccount item in dbList) {
                    EmailAccountsCollection.Add(item);
                }
                HasItems = true;
            } else {
                HasItems = false;
            }
        }

        private void RefreshCategories() {
            GetDbEmailAccounts();
        }

        

        private async void AddMailAccount(object obj) {
            await _Navigation.PushPopupAsync(new NewEmailAccountPopupPage());
        }
    }
}
