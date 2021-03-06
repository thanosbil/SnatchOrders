﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnatchOrders.Models
{
    [Table("Categories")]
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Περιγραφή κατηγορίας
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Λογικό delete
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Αριθμός ειδών στην κατηγορία
        /// </summary>
        [Ignore]
        public int ItemCount { get; set; }
    }
}
