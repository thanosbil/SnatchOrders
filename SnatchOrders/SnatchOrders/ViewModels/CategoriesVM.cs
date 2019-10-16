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
                
        public ICommand AddCategoryCommand { get; set; }
        public ICommand GoToItemsPageCommand { get; set; }
        public ICommand DeleteCategoryCommand { get; set; }

        private INavigation _navigation;
        private List<Category> ListOfCategories;

        public CategoriesVM(INavigation navigation)
        {
            _navigation = navigation;
            CurrentOrder = new Order();
            
            AddCategoryCommand = new Command(CreateNewCategory);
            GoToItemsPageCommand = new Command<Category>(GoToItemsPage);
            DeleteCategoryCommand = new Command<Category>(DeleteCategory);
            Categories = new ObservableCollection<Category>();
            ListOfCategories = new List<Category>();
            AddNewOrder(CurrentOrder);
        }

        private async void DeleteCategory(Category obj)
        {
            bool result = await App.Current.MainPage.DisplayAlert("Διαγραφή", "Πρόκειται να διαγραφεί η παραγγελία. Θέλετε να συνεχίσετε;", "OK", "ΑΚΥΡΟ");
            if (result)
            {
                await App.Database.DeleteCategoryAsync(obj);
                GetCategoriesList();
            }
        }

        public async void GetCategoriesList()
        {
            Categories.Clear();
            ListOfCategories.Clear();
            ListOfCategories = await App.Database.GetCategoriesAsync();
            //List<Item> itemsPerCategory;

            if (ListOfCategories.Count > 0)
            {
                foreach (Category item in ListOfCategories)
                {
                    int itemsPerCategory = App.Database.GetItemsAsync(item.ID).Result.Count;
                    item.ItemCount = itemsPerCategory;
                    Categories.Add(item);
                }
            }
        }

        //private async void AddNewOrder(Order obj)
        //{
        //    obj.DateSent = DateTime.Now;
        //    CurrentOrder.ID = await App.Database.SaveOrderAsync(obj);
        //}

        private async void CreateNewCategory(object obj)
        {
            await _navigation.PushAsync(new CategoryDetailPage());
        }



        private async void GoToItemsPage(Category category)
        {
            //await _navigation.PushAsync(new ItemsPage(category, CurrentOrder.ID));
        }
    }
}
