using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnatchOrders.Models
{
    public enum StatusOfOrder {
        None = 0,
        New = 1,
        InProgress = 2,
        Finished = 3
    }

    /// <summary>
    /// The main class with all the fields to populate the order email
    /// </summary>
    [Table("Orders")]
    public class Order
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Ημ/νία δημιουργίας της παραγγελίας
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Τρέχουσα κατάσταση της παραγγελίας
        /// </summary>
        public StatusOfOrder OrderStatus { get; set; }

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
