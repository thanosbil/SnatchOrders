using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SnatchOrders.Models {
    public class ReportItem : INotifyPropertyChanged {
        private int quantity;
        private decimal orderAverage;
        private int numberOfOrders;

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
        public int Quantity {
            get { return quantity; }
            set { 
                if(quantity != value) {
                    quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }
        /// <summary>
        /// Μέσος όρος ποσότητας ανά παραγγελία, βάση των κριτηρίων
        /// </summary>        
        public decimal OrderAverageQuantity {
            get { return orderAverage; }
            set { 
                if(orderAverage != value) {
                    orderAverage = Math.Round(value, 2);
                    OnPropertyChanged("OrderAverageQuantity");
                }
            }
        }

        /// <summary>
        /// Αριθμός παραγγελιών που εμφανίζεται το είδος, βάση των κριτηρίων 
        /// </summary>
        public int InNumberOfOrders {
            get { return numberOfOrders; }
            set { 
                if(numberOfOrders != value) {
                    numberOfOrders = value;
                    OnPropertyChanged("InNumberOfOrders");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
