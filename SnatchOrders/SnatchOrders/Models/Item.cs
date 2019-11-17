using SQLite;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SnatchOrders.Models
{
    [Table("Items")]
    public class Item : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
                
        /// <summary>
        /// Id κατηγορίας προϊόντος
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Περιγραφή έίδους
        /// </summary>
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}