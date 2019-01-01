using SQLite;
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
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Όλα τα προϊόντα που περιέχει η παραγγελία
        /// </summary>
        [Ignore]
        public List<Item> AllItems { get; set; }
        /// <summary>
        /// Ημερομηνία αποστολής της παραγγελίας
        /// </summary>
        public DateTime DateSent { get; set; }

        /// <summary>
        /// Σε ποιον στάλθηκε η παραγγελία
        /// </summary>
        public string OrderRecipient { get; set; }
    }
}
