using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SmartCart.Models;

namespace SmartCart.Services
{ 
    public class GroceryListServices
    {
        private readonly SQLiteAsyncConnection _grocerylist;

        public GroceryListServices(string dbPath)
        {
            _grocerylist = new SQLiteAsyncConnection(dbPath);
            _grocerylist.CreateTableAsync<GroceryItem>().Wait();
        }

        public Task<List<GroceryItem>> GetItemAsync()
        {
            return _grocerylist.Table<GroceryItem>().ToListAsync();
        }

        public Task<int> DeleteItemAsync(GroceryItem groceryItem)
        {
            return _grocerylist.DeleteAsync(groceryItem);
        }
    }
}
