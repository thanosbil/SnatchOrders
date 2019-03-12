using SnatchOrders.Models;
using SnatchOrders.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnatchOrders.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemsPage : ContentPage
	{
        ItemsPageVM itemsPageVM;

		public ItemsPage (Category category, int OrderID)
		{
			InitializeComponent ();
            Title = category.Description;
            itemsPageVM = new ItemsPageVM(Navigation, category);
            BindingContext = itemsPageVM;            
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            CreateCustomList();
        }


        private async void CreateCustomList()
        {

            await itemsPageVM.GetItemsList();

            foreach (Item dbItem in itemsPageVM.Items)
            {
                StackLayout SecondStack = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal                                      
                };

                SecondStack.BindingContext = dbItem;

                Label Descr = new Label();
                Descr.FontSize = 18;
                Descr.VerticalTextAlignment = TextAlignment.Center;
                Descr.SetBinding(Label.TextProperty, new Binding("Description"));
                
                Button add = new Button();
                add.WidthRequest = 50;
                add.HeightRequest = 50;
                add.BindingContext = itemsPageVM;
                add.SetBinding(Button.CommandProperty, new Binding("IncreaseQuantityCommand"));
                add.CommandParameter = dbItem;
                add.Text = "+";
                
                

                Label quantity = new Label();
                quantity.FontSize = 18;
                quantity.VerticalTextAlignment = TextAlignment.Center;
                quantity.SetBinding(Label.TextProperty, new Binding("Count"));

                Button decrease = new Button();
                decrease.WidthRequest = 50;
                decrease.HeightRequest = 50;
                decrease.BindingContext = itemsPageVM;
                decrease.SetBinding(Button.CommandProperty, new Binding("DecreaseQuantityCommand"));
                decrease.CommandParameter = dbItem;
                decrease.Text = "-";
                
                

                SecondStack.Children.Add(Descr);
                SecondStack.Children.Add(add);
                SecondStack.Children.Add(quantity);
                SecondStack.Children.Add(decrease);
                FirstStack.Children.Add(SecondStack);
            }
        }
    }
}