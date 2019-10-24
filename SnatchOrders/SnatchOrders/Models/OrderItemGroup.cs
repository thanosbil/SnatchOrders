using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace SnatchOrders.Models
{
    public class OrderItemGroup : ObservableCollection<OrderItem>, INotifyPropertyChanged
    {
        private bool _expanded { get; set; }
        public bool Expanded {
            get { return _expanded; }
            set {
                if(_expanded != value) {
                    _expanded = value;
                    OnPropertyChanged("Expanded");
                }
            }
        }

        public string GroupTitle { get; set; }

        public int ItemCount { get; set; }

        public string StateIcon {
            get { return Expanded ? "baseline_expand_less_black_48.png" : "baseline_expand_more_black_48.png"; }
        }

        public OrderItemGroup(string title, bool expanded = true) {
            GroupTitle = title;
            Expanded = expanded;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
