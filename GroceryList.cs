using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart
{
    public static class GroceryList
    {
        internal static List<GroceryItem> currentList = new List<GroceryItem>();

        public static List<GroceryItem> BuildList(SqliteDataReader reader)
        {
            List<GroceryItem> list = new List<GroceryItem>();

            while (reader.Read())
            {
                GroceryItem item = new GroceryItem(
                    reader.GetString(0),
                    Convert.ToInt32(reader.GetValue(1)),
                    Convert.ToInt32(reader.GetValue(2)),
                    Convert.ToBoolean(Convert.ToInt32(reader.GetValue(3)))
                    );
                list.Add(item);
            }

            return list;
        }

        public static List<GroceryItem> GetLatestList()
        {
            return currentList;
        }
        public static void SetLatestList(List<GroceryItem> newList)
        {
            currentList = newList;
        }
    }
}
