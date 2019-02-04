using SnatchOrders.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SnatchOrders.ViewModels
{
    public class NewItemPageVM
    {
        public ICommand AddNewItemCommand { get; set; }

        private INavigation _navigation { get; set; }

        public NewItemPageVM(INavigation navigation)
        {
            _navigation = navigation;
            AddNewItemCommand = new Command<string>(AddNewItem);
        }

        private async void AddNewItem(string description)
        {
            Item newItem = new Item();
            newItem.Description = description;
            await App.Database.SaveItemAsync(newItem);
            await _navigation.PopAsync();
        }
    }
}
