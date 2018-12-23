using System;
using System.Collections.Generic;
using System.Text;

namespace SnatchOrders.Models
{
    /// <summary>
    /// The main class with all the fields to populate the order email
    /// </summary>
    public class Order
    {
        public List<Item> Whiskey { get; set; }
        public List<Item> Vodka { get; set; }
        public List<Item> Rum { get; set; }
        public List<Item> Tequila { get; set; }
        public List<Item> Gin{ get; set; }
        public List<Item> OtherLiquors { get; set; }
    }
}
