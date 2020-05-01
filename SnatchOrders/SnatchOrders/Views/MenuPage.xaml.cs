using SnatchOrders.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnatchOrders.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        MenuPageViewModel menuPageViewModel;
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();
            menuPageViewModel = new MenuPageViewModel();
            BindingContext = menuPageViewModel;
            //menuItems = new List<HomeMenuItem>
            //{
            //    new HomeMenuItem {Id = MenuItemType.Home, Title="Αρχική" },
            //    new HomeMenuItem {Id = MenuItemType.MailSettings, Title="Διευθύνσεις email" },
            //    new HomeMenuItem {Id = MenuItemType.Items, Title="Αποθηκευμένα είδη" },
            //    new HomeMenuItem {Id = MenuItemType.About, Title="Σχετικά με την εφαρμογή" }                
            //};

            //ListViewMenu.ItemsSource = menuItems;

            //ListViewMenu.SelectedItem = menuItems[0];
            //ListViewMenu.ItemSelected += async (sender, e) =>
            //{
            //    if (e.SelectedItem == null)
            //        return;

            //    var id = (int)((HomeMenuItem)e.SelectedItem).Id;
            //    await RootPage.NavigateFromMenu(id);
            //};
        }

        class MenuPageViewModel : INotifyPropertyChanged {
            public ObservableCollection<HomeMenuItem> MenuItems { get; set; }

            public MenuPageViewModel() {

                MenuItems = new ObservableCollection<HomeMenuItem> {
                new HomeMenuItem {Id = MenuItemType.Home, Title="Αρχική" },
                new HomeMenuItem {Id = MenuItemType.MailSettings, Title="Διευθύνσεις email" },
                new HomeMenuItem {Id = MenuItemType.Items, Title="Αποθηκευμένα είδη" },
                new HomeMenuItem {Id = MenuItemType.Reports, Title="Αναφορές" },
                new HomeMenuItem {Id = MenuItemType.About, Title="Σχετικά με την εφαρμογή" }
                };
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "") {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }

        private async void ListViewMenu_ItemTapped(object sender, ItemTappedEventArgs e) {
            if(e.Item != null) {
                var id = (int)((HomeMenuItem)e.Item).Id;
                await RootPage.NavigateFromMenu(id);
                ListViewMenu.SelectedItem = null;
            }
        }
    }
}