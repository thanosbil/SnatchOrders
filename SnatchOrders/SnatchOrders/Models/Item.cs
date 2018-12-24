using SQLite;
using System;

namespace SnatchOrders.Models
{
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public string ID { get; set; }

        /// <summary>
        /// Id παραγγελίας
        /// </summary>
        public int OrderId { get; set; }        

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