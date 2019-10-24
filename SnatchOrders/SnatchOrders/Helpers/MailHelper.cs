using SnatchOrders.Models;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

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
    }
}
