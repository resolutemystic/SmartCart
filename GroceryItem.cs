using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart
{
    public class GroceryItem
    {
        int itemID;
        string name;
        int quantity;
        string priority;
        bool isChecked;
        public GroceryItem(int itemID, string name, int quantity, int priority, bool isChecked)
        {
            this.itemID = itemID;
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
        public int ItemID { get => itemID; set => itemID = value; }
        public string Name { get => name; set => name = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public string Priority { get => priority; set => priority = value; }
        public bool IsChecked { get => isChecked; set => isChecked = value; }
    }
}
