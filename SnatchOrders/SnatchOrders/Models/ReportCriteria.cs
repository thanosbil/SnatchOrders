using System;
using System.Collections.Generic;
using System.Text;

namespace SnatchOrders.Models {
    public class ReportCriteria {
        public int CategoryId { get; set; }
        public int ItemId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public StatusOfOrder StatusCriteria { get; set; }
    }
}
