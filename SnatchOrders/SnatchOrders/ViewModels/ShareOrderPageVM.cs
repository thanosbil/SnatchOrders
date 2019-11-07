using SnatchOrders.Helpers;
using SnatchOrders.Interfaces;
using SnatchOrders.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SnatchOrders.ViewModels
{
    public class ShareOrderPageVM : ViewModelBase {
        public ICommand CreateMailBodyCommand { get; set; }
        public ICommand ShareOrderCommand { get; set; }
        public INavigation _Navigation { get; set; }

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
        public Order _CurrentOrder { get; set; }
        private List<EmailAccount> EmailList { get; set; }
        public EmailAccount EmailTo { get; set; }
        public EmailAccount EmailCc { get; set; }
        public EmailAccount EmailBcc { get; set; }
        public ObservableCollection<EmailAccount> EmailCollection { get; set; }
        public string MailSubject { get; set; }

        public ShareOrderPageVM(INavigation navigation, Order currentOrder) {
            _Navigation = navigation;

            EmailCollection = new ObservableCollection<EmailAccount>();
            _CurrentOrder = currentOrder;
            MailSubject = Preferences.Get("MailSubject", "");
            CreateMailBodyCommand = new Command(CreateMailBody);
            ShareOrderCommand = new Command(ShareOrder);
        }

        private void ShareOrder() {
            string message = string.Empty;
            List<OrderItem> OrderItems = _CurrentOrder.AllItems.OrderBy(i => i.CategoryId).ThenBy(d => d.Description).ToList();

            foreach(OrderItem item in OrderItems) {
                message += $"{item.Description} x{item.Count}";
                if(OrderItems.IndexOf(item) != OrderItems.Count - 1)
                    message += "\r\n";
            }

            DependencyService.Get<IShare>().ShareMessageToApps(message);
            //await Share.RequestAsync(new ShareTextRequest {
            //    Text = message
            //});
        }

        //private string BuildFormattedMessage(OrderItem item, int lineLength) {
        //    int itemCountLength = item.Count.ToString().Length;
        //    string ret = item.Description.PadRight(lineLength - itemCountLength);
        //    ret += item.Count;
        //    return ret;
        //}

        private async void CreateMailBody() {
            string body = string.Empty;
            List<OrderItem> OrderItems = _CurrentOrder.AllItems.OrderBy(i => i.CategoryId).ThenBy(d => d.Description).ToList();

            foreach (OrderItem item in OrderItems) {
                body += $"{item.Description}  x{item.Count}\r\n";                             
            }

            body += "\r\n\r\n";
            body += "Order list was generated automatically by Go SnatchThat! app.";
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

            await MailHelper.SendEmail(Preferences.Get("MailSubject", ""), body, rec, ccRec, bccRec);
            FinishOrder();
        }

        private async void FinishOrder() {
            _CurrentOrder.DateSent = DateTime.Now;
            _CurrentOrder.OrderStatus = StatusOfOrder.Finished;

            try {
                await App.Database.SaveOrderAsync(_CurrentOrder);
            } catch (Exception ex) {
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την αποθήκευση της παραγγελίας"
                    + Environment.NewLine + ex, "OK");
            }            
        }

        public async void GetEmailList() {

            try {
                EmailList = await App.Database.GetEmailAcountsAsync();
                if(EmailList != null && EmailList.Count > 0) {
                    ConvertToObservable(EmailList);
                    HasItems = true;
                } else {
                    HasItems = false;
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
