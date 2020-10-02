using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace SnatchOrders.Models {
    public class ReportItemGroup : ObservableCollection<ReportItem>, INotifyPropertyChanged {
        private bool _expanded { get; set; }
        public bool Expanded {
            get { return _expanded; }
            set {
                if (_expanded != value) {
                    _expanded = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Expanded"));
                    OnPropertyChanged(new PropertyChangedEventArgs("StateIcon"));
                }
            }
        }

        public List<ReportItem> BackUpList { get; set; }
        public string GroupTitle { get; set; }
        public string StateIcon {
            get { return Expanded ? "baseline_expand_less_black_48.png" : "baseline_expand_more_black_48.png"; }
        }
        public int CategoryId { get; set; }

        /// <summary>
        /// Constructor         
        /// </summary>
        /// <param name="title"></param>
        /// <param name="expanded"></param>
        public ReportItemGroup(string title, bool expanded = true) {
            GroupTitle = title;
            Expanded = expanded;
            BackUpList = new List<ReportItem>();
        }

        internal void CopyList() {
            BackUpList.Clear();
            BackUpList.AddRange(this);
        }
    }
}
