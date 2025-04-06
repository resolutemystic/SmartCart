using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SmartCart.Models
{
    public class GroceryItem {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }
    }

    internal class GroceryItemModel
    {
    }
}
