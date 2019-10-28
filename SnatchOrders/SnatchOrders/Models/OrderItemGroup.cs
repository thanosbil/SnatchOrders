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
                    OnPropertyChanged(new PropertyChangedEventArgs("Expanded"));
                    OnPropertyChanged(new PropertyChangedEventArgs("StateIcon"));
                }
            }
        }

        public List<OrderItem> BackUpList { get; set; }

        public string GroupTitle { get; set; }
        private int _itemCount { get; set; }
        public int ItemCount {
            get { return _itemCount; }
            set {
                if(_itemCount != value) {
                    _itemCount = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ItemCount"));
                }
            }
        }

        public string StateIcon {
            get { return Expanded ? "baseline_expand_less_black_48.png" : "baseline_expand_more_black_48.png"; }
        }

        public OrderItemGroup(string title, bool expanded = true) {
            GroupTitle = title;
            Expanded = expanded;
            BackUpList = new List<OrderItem>();
        }

        internal void CopyList() {
            BackUpList.Clear();
            BackUpList.AddRange(this);
        }
    }
}
