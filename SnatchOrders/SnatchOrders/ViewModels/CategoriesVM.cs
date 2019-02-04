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
        public Order CurrentOrder { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        //public ICommand AddNewOrderCommand { get; set; }
        public ICommand AddCategoryCommand { get; set; }
        public ICommand GoToItemsPageCommand { get; set; }

        private INavigation _navigation;
        private List<Category> ListOfCategories;

        public CategoriesVM(INavigation navigation)
        {
            _navigation = navigation;
            CurrentOrder = new Order();
            //AddNewOrderCommand = new Command<Order>(AddNewOrder);
            AddCategoryCommand = new Command(CreateNewCategory);
            GoToItemsPageCommand = new Command<Category>(GoToItemsPage);            
            Categories = new ObservableCollection<Category>();
            ListOfCategories = new List<Category>();
            AddNewOrder(CurrentOrder);
        }

        
        public async void GetCategoriesList()
        {
            Categories.Clear();
            ListOfCategories.Clear();
            ListOfCategories = await App.Database.GetCategoriesAsync();

            if (ListOfCategories.Count > 0)
            {
                foreach (Category item in ListOfCategories)
                {
                    Categories.Add(item);
                }
            }
        }

        private async void AddNewOrder(Order obj)
        {
            obj.DateSent = DateTime.Now;
            CurrentOrder.ID = await App.Database.SaveOrderAsync(obj);
        }

        private async void CreateNewCategory(object obj)
        {
            await _navigation.PushAsync(new CategoryDetailPage());
        }

        private async void GoToItemsPage(Category category)
        {
            await _navigation.PushAsync(new ItemsPage(category, CurrentOrder.ID));
        }
    }
}
