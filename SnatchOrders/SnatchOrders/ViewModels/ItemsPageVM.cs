using SnatchOrders.Models;
using SnatchOrders.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SnatchOrders.ViewModels
{
    public class ItemsPageVM : ViewModelBase
    {      
        public ICommand GoToNewItemPageCommand { get; set; }
        public ICommand IncreaseQuantityCommand { get; set; }
        public ICommand DecreaseQuantityCommand { get; set; }

        public ObservableCollection<Item> Items { get; set; }

        private List<Item> dbItems { get; set; }
        private INavigation _navigation { get; set; }

        public ItemsPageVM(INavigation navigation)
        {
            _navigation = navigation;
            dbItems = new List<Item>();
            Items = new ObservableCollection<Item>();
            GoToNewItemPageCommand = new Command(GoToNewItemPage);
        }

        public async void GetItemsList()
        {
            Items.Clear();
            dbItems.Clear();
            dbItems = await App.Database.GetItemsAsync();

            if (dbItems.Count > 0)
            {
                foreach (Item item in dbItems)
                {
                    Items.Add(item);
                }
            }
        }

        private async void GoToNewItemPage()
        {
            await _navigation.PushAsync(new NewItemPage());
        }
    }
}
