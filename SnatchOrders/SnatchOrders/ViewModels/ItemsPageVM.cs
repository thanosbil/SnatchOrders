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

        private string quantity { get; set; }
        public string Quantity {
            get { return quantity; }
            set {
                if (quantity != value){
                    quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }

        private int Counter { get; set; }

        public Item SelectedItem { get; set; }
        public ObservableCollection<Item> Items { get; set; }

        private List<Item> dbItems { get; set; }
        private INavigation _navigation { get; set; }

        public ItemsPageVM(INavigation navigation)
        {
            _navigation = navigation;
            SelectedItem = new Item(); 
            dbItems = new List<Item>();
            Items = new ObservableCollection<Item>();
            GoToNewItemPageCommand = new Command(GoToNewItemPage);
            DecreaseQuantityCommand = new Command(DecreaseQuantity);
            IncreaseQuantityCommand = new Command(IncreaseQuantity);
            Counter = 0;
            Quantity = Counter.ToString();

        }

        private void IncreaseQuantity()
        {
            
        }

        private void DecreaseQuantity()
        {
            if(Counter > 0)
            {
                Counter--;
                Quantity = Counter.ToString();
            }
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
