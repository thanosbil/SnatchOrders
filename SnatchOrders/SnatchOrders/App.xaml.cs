using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SnatchOrders.Views;
using SnatchOrders.Data;
using System.IO;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SnatchOrders
{
    public partial class App : Application
    {
        static DatabaseUtilities database;

        public static DatabaseUtilities Database
        {
            get
            {
                if (database == null)
                {
                    database = new DatabaseUtilities(
                      Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TodoSQLite.db3"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();


            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
