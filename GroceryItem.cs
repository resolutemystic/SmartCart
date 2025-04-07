using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
=======
using SQLite;
>>>>>>> 8b7d3ce (Initial commit of working SmartCart features)

namespace SmartCart
{
    public class GroceryItem
    {
<<<<<<< HEAD
        string name;
        int quantity;
        string priority;
        bool isChecked;
        public GroceryItem(string name, int quantity, int priority, bool isChecked)
        {
            this.name = name;
            this.quantity = quantity;

            switch (priority)
            {
                case 1:
                    this.priority = "Low";
                    break;
                case 2:
                    this.priority = "Medium";
                    break;
                case 3:
                    this.priority = "High";
                    break;
            }

            this.isChecked = isChecked;
        }
        public string Name { get => name; set => name = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public string Priority { get => priority; set => priority = value; }
        public bool IsChecked { get => isChecked; set => isChecked = value; }
    }
}
=======
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }

        [SQLite.MaxLength(100), SQLite.NotNull]
        public string Name { get; set; }

        public bool IsChecked { get; set; } = false;
    }
}


>>>>>>> 8b7d3ce (Initial commit of working SmartCart features)
