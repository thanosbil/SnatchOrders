using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SnatchOrders.Models
{
    public class OrderItemGroup : ObservableCollection<OrderItem>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
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
            CollectionChanged += OrderItemGroup_CollectionChanged;
        }

        private void OrderItemGroup_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            if (e.OldItems != null) {
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= item_PropertyChanged;
            }
            if (e.NewItems != null) {
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += item_PropertyChanged;
            }

            OnPropertyChanged();
        }

        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            OnPropertyChanged();
        }

        internal void CopyList() {
            BackUpList.Clear();
            BackUpList.AddRange(this);
        }
    }
}
