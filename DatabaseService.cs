using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SmartCart
{
    public class DatabaseService
    {
        private static SQLiteAsyncConnection _database;

        private static async Task Init()
        {
            if (_database != null)
                return;

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "smartcart.db");
            _database = new SQLiteAsyncConnection(dbPath);
            await _database.CreateTableAsync<GroceryItem>();
        }

        public static async Task AddItemAsync(string name)
        {
            await Init();

            var item = new GroceryItem { Name = name };
            await _database.InsertAsync(item);
        }

        public static async Task<List<GroceryItem>> GetItemsAsync()
        {
            await Init();
            return await _database.Table<GroceryItem>().ToListAsync();
        }
    }
}
