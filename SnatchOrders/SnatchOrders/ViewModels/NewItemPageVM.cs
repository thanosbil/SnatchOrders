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
        private int CategoryId { get; set; }

        public NewItemPageVM(INavigation navigation, int categoryId){
            _navigation = navigation;
            CategoryId = categoryId;
            AddNewItemCommand = new Command<string>(AddNewItem);
        }

        private async void AddNewItem(string description){
            Item newItem = new Item();
            newItem.Description = description;
            newItem.CategoryId = CategoryId;

            try {
                await App.Database.SaveItemAsync(newItem);
            }catch(Exception ex) {
                await App.Current.MainPage.DisplayAlert("Σφάλμα", "Παρουσιάστηκε πρόβλημα κατά την αποθήκευση του είδους" 
                    + Environment.NewLine + ex, "OK");
            }
            await _navigation.PopAsync();
        }
    }
}
