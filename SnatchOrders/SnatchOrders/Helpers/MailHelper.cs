using SnatchOrders.Models;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SnatchOrders.Helpers
{
    public static class MailHelper
    {
        public static string PrepareMailBody(List<OrderItemGroup> OrderList) {
            string body = string.Empty;

            return body;
        }

        public static void SendMail(string to, string cc, string subject, string body) {
            try {

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("");
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;

                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("", "");

                SmtpServer.Send(mail);
            } catch (Exception ex) {
                App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε κάποιο πρόβλημα κατά την αποστολή του email." +
                    Environment.NewLine + ex, "OK");
            }
        }

        public static async Task SendEmail(string subject, string body, List<string> recipients, List<string> ccRecipients, List<string> bccRecipients) {
            try {
                var message = new EmailMessage {
                    Subject = subject,                    
                    Body = body,                    
                    To = recipients,
                    Cc = ccRecipients,
                    Bcc = bccRecipients
                };

                await Email.ComposeAsync(message);
            } catch (FeatureNotSupportedException fbsEx) {
                // Email is not supported on this device
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε κάποιο πρόβλημα κατά τη δημιουργία του email." +
                    Environment.NewLine + fbsEx, "OK");
            } catch (Exception ex) {
                // Some other exception occurred
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε κάποιο πρόβλημα κατά τη δημιουργία του email." +
                    Environment.NewLine + ex, "OK");
            }
        }
    }
}
