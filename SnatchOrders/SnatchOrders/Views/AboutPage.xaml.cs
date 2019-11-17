using SnatchOrders.Helpers;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnatchOrders.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            string about = "Η εφαρμογή δημιουργήθηκε με σκοπό την εύκολη δημιουργία μιας παραγγελίας η οποία τελικά θα αποστέλεται μέσω email σε κάποιον παραλήπτη. " +
                "Στην πορεία προστέθηκε και η δυνατότητα να γίνει \"share\" της παραγγελίας και σε άλλες εφαρμογές. \r\n\r\n" +
                "Για τυχόν απορίες ή πληροφορίες μπορείτε να απευθυνθείτε στην παρακάτω διεύθυνση email.";
            lblAbout.Text = about;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e) {
            List<string> supportMail = new List<string> { "info.snatchthat@gmail.com" };
            await MailHelper.SendEmail("SnatchThat! help", "", supportMail, new List<string>(), new List<string>());
        }
    }
}