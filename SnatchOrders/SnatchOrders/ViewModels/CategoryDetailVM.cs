using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SnatchOrders.ViewModels
{
    public class CategoryDetailVM
    {
        public ICommand CreateNewCategory { get; set; }
        private INavigation _navigation;

        public CategoryDetailVM(INavigation navigation)
        {
            CreateNewCategory = new Command(AddCategory);
            _navigation = navigation;
        }

        private async void AddCategory(object obj)
        {
            await _navigation.PopAsync();
        }
    }
}
