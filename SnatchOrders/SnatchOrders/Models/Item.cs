using SQLite;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SnatchOrders.Models
{
    public class Item : INotifyPropertyChanged
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
        public string Description {
            get { return _Description; }
            set {
                if(_Description != value)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private string _Description { get; set; }

        /// <summary>
        /// Ποσότητα παραγγελίας
        /// </summary>
        public int Count {
            get { return _Count; }
            set {
                if(_Count != value)
                {
                    _Count = value;
                    OnPropertyChanged("Count");
                }
            }
        }

        private int _Count { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}