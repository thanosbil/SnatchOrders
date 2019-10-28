using SnatchOrders.Helpers;
using SnatchOrders.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SnatchOrders.ViewModels
{
    public class ShareOrderPageVM : ViewModelBase {
        public ICommand CreateMailBodyCommand { get; set; }
        public INavigation _Navigation { get; set; }

        public Order _CurrentOrder { get; set; }
        public List<EmailAccount> EmailList { get; set; }
        public EmailAccount EmailTo { get; set; }
        public EmailAccount EmailCc { get; set; }
        public EmailAccount EmailBcc { get; set; }
        public ObservableCollection<EmailAccount> EmailCollection { get; set; }

        public ShareOrderPageVM(INavigation navigation, Order currentOrder) {
            _Navigation = navigation;

            EmailCollection = new ObservableCollection<EmailAccount>();
            _CurrentOrder = currentOrder;

            CreateMailBodyCommand = new Command(CreateMailBody);
        }

        private async void CreateMailBody() {
            string body = string.Empty;
            List<OrderItem> OrderItems = _CurrentOrder.AllItems.OrderBy(i => i.CategoryId).ThenBy(d => d.Description).ToList();
            foreach(OrderItem item in OrderItems) {
                body += $"{item.Description} \t {item.Count} \r\n";
            }

            List<string> rec = new List<string>();
            List<string> ccRec = new List<string>();
            List<string> bccRec = new List<string>();

            if(EmailTo != null) {
                rec.Add(EmailTo.Email);
            }

            if (EmailCc != null) {
                ccRec.Add(EmailCc.Email);
            }

            if (EmailBcc != null) {
                bccRec.Add(EmailBcc.Email);
            }

            await MailHelper.SendEmail("Παραγγελία mail test", body, rec, ccRec, bccRec);
            
        }

        public async void GetEmailList() {

            try {
                EmailList = await App.Database.GetEmailAcountsAsync();
                if(EmailList != null && EmailList.Count > 0) {
                    ConvertToObservable(EmailList);
                }

            } catch (Exception ex) {
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την ανάγνωση της λίστας διεθύνσεων email"
                    + Environment.NewLine + ex, "OK");
            }
        }

        private void ConvertToObservable(List<EmailAccount> emailList) {
            EmailCollection.Clear();

            foreach (EmailAccount item in emailList) {
                EmailCollection.Add(item);
            }
        }
    }
}
