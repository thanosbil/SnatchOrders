using SnatchOrders.Models;
using SnatchOrders.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        int CategoryId;
        ItemsPageVM itemsPageVM;

		public ItemsPage (int orderId, Category currentCategory){
			InitializeComponent ();
            itemsPageVM = new ItemsPageVM(Navigation, orderId, currentCategory.ID);
            BindingContext = itemsPageVM;
            Title = currentCategory.Description;
            CategoryId = currentCategory.ID;
		}

        protected override async void OnAppearing() {
            base.OnAppearing();
            if(itemsPageVM.ItemsCollection.Count == 0)
                await itemsPageVM.GetCategoryItems(CategoryId);
            FixGrid(itemsPageVM.ItemsCollection);
        }

        private void FixGrid(ObservableCollection<Item> CategoryItems) {
            StackLayout rowStack;
            Grid grid;

            foreach (Item item in CategoryItems) {
                //rowStack = new StackLayout();
                //rowStack.BindingContext = item;
                grid = new Grid();
                grid.Padding = new Thickness(15, 15, 15, 0);
                grid.ColumnSpacing = 15;
                grid.BindingContext = item;
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                // 1η κολώνα - Περιγραφή
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                // 2η κολώνα - Ποσότητα
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                // 3η κολώνα - Κουμπί (-)
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
                // 4η κολώνα - Κουμπί (+)
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
                                
                Label Description = new Label { Style = (Style)Application.Current.Resources["LabelsDefault"], VerticalOptions = LayoutOptions.Center };
                Description.SetBinding(Label.TextProperty, new Binding("Description"));

                Label Count = new Label { Style = (Style)Application.Current.Resources["LabelsDefault"], VerticalOptions = LayoutOptions.Center };
                Count.SetBinding(Label.TextProperty, new Binding("Count"));

                ImageButton MinusBtn = new ImageButton {
                    Style = (Style)Application.Current.Resources["ImageButtonSmall"],
                    Padding = 10,
                    BackgroundColor = Color.PaleGreen,
                    WidthRequest = 40,
                    HeightRequest = 40,
                    BorderColor = Color.Green,
                    Source = "baseline_remove_white_48"
                };
                MinusBtn.BindingContext = OuterStack.BindingContext;
                MinusBtn.SetBinding(ImageButton.CommandProperty, new Binding("DecreaseCountCommand"));
                MinusBtn.SetBinding(ImageButton.CommandParameterProperty, new Binding() { Source = item });

                ImageButton PlusBtn = new ImageButton {
                    Style = (Style)Application.Current.Resources["ImageButtonSmall"],
                    Padding = 10,
                    BackgroundColor = Color.PaleGreen,
                    WidthRequest = 40,
                    HeightRequest = 40,
                    BorderColor = Color.Green,
                    Source = "baseline_add_white_48"
                };
                PlusBtn.BindingContext = OuterStack.BindingContext;
                PlusBtn.SetBinding(Button.CommandProperty, new Binding("IncreaseCountCommand"));
                PlusBtn.SetBinding(Button.CommandParameterProperty, new Binding() { Source = item });

                grid.Children.Add(Description, 0, 0);
                grid.Children.Add(Count, 1, 0);
                grid.Children.Add(MinusBtn, 2, 0);
                grid.Children.Add(PlusBtn, 3, 0);

                //rowStack.Children.Add(grid);
                OuterStack.Children.Add(grid);
            }
        }
    }
}