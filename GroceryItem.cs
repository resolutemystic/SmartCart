using System;

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
            this.quantity = quantity > 0 ? quantity : 1; // fallback to 1 if quantity is 0

            // Priority switch
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
                default:
                    this.priority = "Low"; // fallback
                    break;
            }

            this.isChecked = isChecked;
        }

        public int EntryID { get => entryID; set => entryID = value; }
        public string Name { get => name; set => name = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public string Priority { get => priority; set => priority = value; }
        public bool IsChecked { get => isChecked; set => isChecked = value; }

        // For dropdown (index-based)
        public int QuantityIndex
        {
            get => Quantity - 1;
            set => Quantity = value + 1;
        }
    }
}
