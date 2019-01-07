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
    public class CategoriesVM
    {
        public ObservableCollection<Category> Categories { get; set; }

        public ICommand AddCategory { get; set; }

        private INavigation _navigation;

        public CategoriesVM(INavigation navigation)
        {
            AddCategory = new Command(CreateNewCategory);
            _navigation = navigation;
        }

        private async void CreateNewCategory(object obj)
        {
            await _navigation.PushAsync(new CategoryDetailPage());
        }
    }
}
