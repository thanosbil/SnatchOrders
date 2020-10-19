using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SnatchOrders.Models
{
    [Table("OrderItems")]
    public class OrderItem : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int ItemId { get; set; }
        public int CategoryId { get; set; }
        /// <summary>
        /// Id παραγγελίας
        /// </summary>
        public int OrderId { get; set; }
        private string _description { get; set; }
        public string Description {
            get { return _description; } 
            set {
                if(_description != value) {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            } 
        }
        private int _Count { get; set; }
        public int Count {
            get { return _Count; }
            set {
                if(_Count != value) {
                    _Count = value;
                    OnPropertyChanged("Count");
                }
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
