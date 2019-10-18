using SnatchOrders.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SnatchOrders.Data
{
    public class DatabaseUtilities
    {
        readonly SQLiteAsyncConnection database;

        public DatabaseUtilities(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            
            database.CreateTableAsync<Order>().Wait();
            database.CreateTableAsync<Category>().Wait();
            database.CreateTableAsync<Item>().Wait();
        }

        #region Order


        /// <summary>
        /// Φέρνει όλες τις παραγγελίες
        /// </summary>
        /// <returns></returns>
        public Task<List<Order>> GetOrdersAsync()
        {
            return database.Table<Order>().OrderByDescending(i => i.DateCreated).ToListAsync();
        }


        /// <summary>
        /// Αποθηκεύει μια παραγγελία
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<int> SaveOrderAsync(Order item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        /// <summary>
        /// Διαγράφει μια παραγγελία
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<int> DeleteOrderAsync(Order item)
        {
            return database.DeleteAsync(item);
        }

        #endregion Order

        #region Category

        /// <summary>
        /// Φέρνει όλες τις κατηγορίες προϊόντων
        /// </summary>
        /// <returns></returns>
        public Task<List<Category>> GetCategoriesAsync()
        {
            return database.Table<Category>().ToListAsync();
        }


        /// <summary>
        /// Αποθηκεύει μια κατηγορία προϊόντων
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<int> SaveCategoryAsync(Category item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        /// <summary>
        /// Διαγράφει μια κατηγορία προϊόντων και τα προϊόντα που ανήκουν σε αυτή
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<int> DeleteCategoryAsync(Category item)
        {
            int x = database.Table<Item>().DeleteAsync(i => i.CategoryId == item.ID).Result;
            return database.DeleteAsync(item);
        }

        #endregion Category

        #region Item

        /// <summary>
        /// Φέρνει όλα τα προϊόντα
        /// </summary>
        /// <returns></returns>
        public Task<List<Item>> GetItemsAsync(int ItemCategoryID)
        {
            return database.Table<Item>().Where(i => i.CategoryId == ItemCategoryID).ToListAsync();
        }

        /// <summary>
        /// Αποθηκεύει ένα προϊόν
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<int> SaveItemAsync(Item item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        /// <summary>
        /// Διαγράφει ένα προϊόν
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<int> DeleteItemAsync(Item item)
        {
            return database.DeleteAsync(item);
        }

        #endregion Item

        //public Task<List<TodoItem>> GetItemsAsync()
        //{
        //    return database.Table<TodoItem>().ToListAsync();
        //}

        //public Task<List<TodoItem>> GetItemsNotDoneAsync()
        //{
        //    return database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        //}

        //public Task<TodoItem> GetItemAsync(int id)
        //{
        //    return database.Table<TodoItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        //}

        //public Task<int> SaveItemAsync(TodoItem item)
        //{
        //    if (item.ID != 0)
        //    {
        //        return database.UpdateAsync(item);
        //    }
        //    else
        //    {
        //        return database.InsertAsync(item);
        //    }
        //}

        //public Task<int> DeleteItemAsync(TodoItem item)
        //{
        //    return database.DeleteAsync(item);
        //}
    }
}
