using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnatchOrders.Data
{
    public class DatabaseUtilities
    {
        readonly SQLiteAsyncConnection database;

        public DatabaseUtilities(string dbPath)
        {
             database = new SQLiteAsyncConnection(dbPath);
        }
    }
}
