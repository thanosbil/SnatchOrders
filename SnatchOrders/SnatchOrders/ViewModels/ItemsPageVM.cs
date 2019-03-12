using SnatchOrders.Models;
using SnatchOrders.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SnatchOrders.ViewModels
{
    public class ItemsPageVM : ViewModelBase
    {      
        public ICommand GoToNewItemPageCommand { get; set; }
        public ICommand IncreaseQuantityCommand { get; set; }
        public ICommand DecreaseQuantityCommand { get; set; }


        public Category ItemCategory { get; set; }
        public Item ItemSelected {
            get { return _ItemSelected; }
            set {
                _ItemSelected = value;
                OnPropertyChanged("ItemSelected");
            }
        }
        private Item _ItemSelected { get; set; }
        public ObservableCollection<Item> Items { get; set; }

        private List<Item> dbItems { get; set; }
        private INavigation _navigation { get; set; }

        public ItemsPageVM(INavigation navigation, Category category)
        {
            _navigation = navigation;
            ItemCategory = category;
            _ItemSelected = new Item();
            dbItems = new List<Item>();
            Items = new ObservableCollection<Item>();
            GoToNewItemPageCommand = new Command(GoToNewItemPage);
            DecreaseQuantityCommand = new Command<Item>(DecreaseQuantity);
            IncreaseQuantityCommand = new Command<Item>(IncreaseQuantity);       
        }

        private void IncreaseQuantity(Item item)
        {
             item.Count++;
        }

        private void DecreaseQuantity(Item item)
        {
            if(item.Count > 0)
            {
                item.Count--;
            }
        }

        public async Task GetItemsList()
        {
            Items.Clear();
            dbItems.Clear();
            dbItems = await App.Database.GetItemsAsync(ItemCategory.ID);

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
            await _navigation.PushAsync(new NewItemPage(ItemCategory));
        }
    }
}
