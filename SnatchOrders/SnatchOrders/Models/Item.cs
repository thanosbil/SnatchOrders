using SQLite;
using System;

namespace SnatchOrders.Models
{
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Id παραγγελίας
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Id κατηγορίας προϊόντος
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Περιγραφή έίδους
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ποσότητα παραγγελίας
        /// </summary>
        public int Count { get; set; }
    }
}