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

		public ItemsPage (Order currentOrder, Category currentCategory, bool isMenuAction){
			InitializeComponent ();
            itemsPageVM = new ItemsPageVM(Navigation, currentOrder, currentCategory, isMenuAction);
            BindingContext = itemsPageVM;
            Title = currentCategory.Description;
            CategoryId = currentCategory.ID;
		}

        protected override async void OnAppearing() {
            base.OnAppearing();
            
            await itemsPageVM.GetCategoryItems(CategoryId);            
        }

        private void FixGrid(ObservableCollection<Item> CategoryItems) {
            if(OuterStack.Children.Count > 0)
                OuterStack.Children.Clear();

            Grid grid;

            foreach (Item item in CategoryItems) {
                //rowStack = new StackLayout();
                //rowStack.BindingContext = item;
                grid = new Grid();
                grid.Padding = new Thickness(15, 15, 15, 0);
                grid.ColumnSpacing = 10;
                grid.BindingContext = item;
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                // 1η κολώνα - Περιγραφή
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                // 2η κολώνα - Ποσότητα
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                // 3η κολώνα - Κουμπιά
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                                                
                Label Description = new Label {
                    Style = (Style)Application.Current.Resources["LabelsDefault"],
                    VerticalOptions = LayoutOptions.Center
                };
                Description.SetBinding(Label.TextProperty, new Binding("Description"));

                Label Count = new Label {
                    Style = (Style)Application.Current.Resources["LabelsDefault"],
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = Color.FromHex("#2962ff")
                };
                Count.SetBinding(Label.TextProperty, new Binding("Count"));

                Grid ButtonGrid = new Grid();
                ButtonGrid.VerticalOptions = LayoutOptions.Center;
                ButtonGrid.ColumnSpacing = 15;
                ButtonGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                // Κολώνα - Κουμπί (-)
                ButtonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                // Κολώνα - Κουμπί (+)
                ButtonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                ImageButton MinusBtn = new ImageButton {
                    Style = (Style)Application.Current.Resources["ImageButtonSmall"],
                    Padding = 10,
                    BorderWidth = 1,
                    BackgroundColor = Color.LightGray,                    
                    BorderColor = Color.Gray,
                    Source = "baseline_remove_black_48"
                };
                MinusBtn.BindingContext = OuterStack.BindingContext;
                MinusBtn.SetBinding(ImageButton.CommandProperty, new Binding("DecreaseCountCommand"));
                MinusBtn.SetBinding(ImageButton.CommandParameterProperty, new Binding() { Source = item });

                ImageButton PlusBtn = new ImageButton {
                    Style = (Style)Application.Current.Resources["ImageButtonSmall"],
                    Padding = 10,
                    BorderWidth = 1,
                    BackgroundColor = Color.LightGray,                      
                    BorderColor = Color.Gray,
                    Source = "baseline_add_black_48"
                };
                PlusBtn.BindingContext = OuterStack.BindingContext;
                PlusBtn.SetBinding(Button.CommandProperty, new Binding("IncreaseCountCommand"));
                PlusBtn.SetBinding(Button.CommandParameterProperty, new Binding() { Source = item });

                ButtonGrid.Children.Add(MinusBtn, 0, 0);
                ButtonGrid.Children.Add(PlusBtn, 1, 0);

                grid.Children.Add(Description, 0, 0);
                grid.Children.Add(Count, 1, 0);
                grid.Children.Add(ButtonGrid, 2, 0);
                //grid.Children.Add(MinusBtn, 2, 0);
                //grid.Children.Add(PlusBtn, 3, 0);

                //rowStack.Children.Add(grid);
                OuterStack.Children.Add(grid);
            }
        }
    }
}