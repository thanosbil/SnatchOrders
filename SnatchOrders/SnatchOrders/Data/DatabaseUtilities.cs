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
            database.CreateTableAsync<OrderItem>().Wait();
            database.CreateTableAsync<EmailAccount>().Wait();
        }

        #region Order


        /// <summary>
        /// Φέρνει όλες τις παραγγελίες
        /// </summary>
        /// <returns></returns>
        public async Task<List<Order>> GetOrdersAsync()
        {
            List<Order> OrderList = await database.Table<Order>().OrderByDescending(i => i.DateCreated).ToListAsync();
            return OrderList;
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
            return database.Table<Category>().Where(i => i.IsDeleted == false).OrderBy(i => i.Description).ToListAsync();
        }

        public Task<Category> GetCategoryAsync(int id) {
            return database.Table<Category>().Where(i => i.ID == id).FirstOrDefaultAsync();
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
        public async Task<int> DeleteCategoryAsync(Category category)
        {
            //int x = database.Table<Item>().DeleteAsync(i => i.CategoryId == item.ID).Result;
            List<Item> itemsList = await GetItemsAsync(category.ID);
            foreach (Item item in itemsList) {
                item.IsDeleted = true;
                await database.UpdateAsync(item);
            }

            category.IsDeleted = true;
            return await database.UpdateAsync(category);
        }

        #endregion Category

        #region Item

        public Task<Item> GetItemAsync(int id) {
            return database.Table<Item>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Φέρνει όλα τα προϊόντα
        /// </summary>
        /// <returns></returns>
        public Task<List<Item>> GetItemsAsync(int ItemCategoryID)
        {
            return database.Table<Item>().Where(i => i.CategoryId == ItemCategoryID && i.IsDeleted == false).OrderBy(i => i.Description).ToListAsync();
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
            //return database.DeleteAsync(item);
            item.IsDeleted = true;
            return database.UpdateAsync(item);
        }

        #endregion Item

        #region OrderItem

        public Task<int> DeleteOrderItemAsync(OrderItem item) {
            return database.DeleteAsync(item);
        }

        public Task<List<OrderItem>> GetOrderItemsAsync(int OrderId) {
            return database.Table<OrderItem>().Where(i => i.OrderId == OrderId).OrderBy(i => i.Description).ToListAsync();
        }

        /// <summary>
        /// Αποθηκεύει ένα είδος της παραγγελίας
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<int> SaveOrderItemAsync(OrderItem item) {
            if (item.ID != 0) {
                return database.UpdateAsync(item);
            } else {
                return database.InsertAsync(item);
            }
        }

        #endregion OrderItem        

        #region Email

        /// <summary>
        /// Φέρνει τις αποθηκευμένες διευθύνσεις email
        /// </summary>
        /// <returns></returns>
        public async Task<List<EmailAccount>> GetEmailAcountsAsync() {
            List<EmailAccount> EmailList = await database.Table<EmailAccount>().OrderBy(i => i.DateSaved).ToListAsync();
            return EmailList;
        }


        /// <summary>
        /// Αποθηκεύει ένα email
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<int> SaveEmailAcountAsync(EmailAccount item) {
            if (item.ID != 0) {
                return database.UpdateAsync(item);
            } else {
                return database.InsertAsync(item);
            }
        }

        /// <summary>
        /// Διαγράφει μια email
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<int> DeleteEmailAcountAsync(EmailAccount item) {
            return database.DeleteAsync(item);
        }


        #endregion Email
    }
}
