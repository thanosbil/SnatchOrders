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
        /// <summary>
        /// Όλα τα προϊόντα που περιέχει η παραγγελία
        /// </summary>
        public List<Item> AllItems { get; set; }
        /// <summary>
        /// Ημερομηνία αποστολής της παραγγελίας
        /// </summary>
        public DateTime DateSent { get; set; }
    }
}
