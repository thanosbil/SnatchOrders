using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnatchOrders.Models
{
    [Table("EmailAccounts")]
    public class EmailAccount
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Email { get; set; }
        public DateTime DateSaved { get; set; }
    }
}
