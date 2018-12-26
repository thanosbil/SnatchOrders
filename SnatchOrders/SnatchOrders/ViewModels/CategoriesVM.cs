using SnatchOrders.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SnatchOrders.ViewModels
{
    public class CategoriesVM
    {
        public ObservableCollection<Category> Categories { get; set; }

        public CategoriesVM()
        {

        }
    }
}
