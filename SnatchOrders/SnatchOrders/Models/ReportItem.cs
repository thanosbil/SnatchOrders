using System;
using System.Collections.Generic;
using System.Text;

namespace SnatchOrders.Models {
    public class ReportItem {
        /// <summary>
        /// ID στο table Items
        /// </summary>
        public int ItemId { get; set; }
        
        /// <summary>
        /// ID στο table Categories
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Περιγραφή είδους
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Σύνολο του είδους που βρέθηκε στις παραγγελίες, βάση των κριτηρίων
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Μέσος όρος ποσότητας ανά παραγγελία, βάση των κριτηρίων
        /// </summary>
        public decimal OrderAverageQuantity { get; set; }

        /// <summary>
        /// Αριθμός παραγγελιών που εμφανίζεται το είδος, βάση των κριτηρίων 
        /// </summary>
        public int InNumberOfOrders { get; set; }
    }
}
