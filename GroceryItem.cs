using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart
{
    public class GroceryItem
    {
        int entryID;
        string name;
        int quantity;
        string priority;
        bool isChecked;
        public GroceryItem(int entryID, string name, int quantity, int priority, bool isChecked)
        {
            this.entryID = entryID;
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
        public int EntryID { get => entryID; set => entryID = value; }
        public string Name { get => name; set => name = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public string Priority { get => priority; set => priority = value; }
        public bool IsChecked { get => isChecked; set => isChecked = value; }
        public List<int> QuantityOptions { get => Enumerable.Range(1, 99).ToList<int>(); }
        public List<string> PriorityOptions { get => new List<string> { "Low", "Medium", "High" }; }
    }
}
