using System;
using SQLite;

namespace SmartCart
{
    public class GroceryItem
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }

        [SQLite.MaxLength(100), SQLite.NotNull]
        public string Name { get; set; }

        public bool IsChecked { get; set; } = false;
    }
}


