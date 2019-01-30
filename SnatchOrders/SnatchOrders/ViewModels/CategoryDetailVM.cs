using SnatchOrders.Models;
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
            CreateNewCategory = new Command<string>(AddCategory);
            _navigation = navigation;
        }

        private async void AddCategory(string Description)
        {
            Category temp = new Category();
            temp.Description = Description;
            await App.Database.SaveCategoryAsync(temp);
            await _navigation.PopAsync();
        }
    }
}
