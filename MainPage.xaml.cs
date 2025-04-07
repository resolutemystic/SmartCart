using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCart
{
    public partial class MainPage : ContentPage
    {
<<<<<<< HEAD


        public MainPage()
        {
            InitializeComponent();
            listItems.ItemsSource = GroceryList.GetLatestList();
        }

        public List<GroceryItem> List
        {
            get { return GroceryList.GetLatestList(); }
=======
        public MainPage()
        {
            InitializeComponent();
            LoadGroceryItems(); // Load on startup
        }

        private async void OnAddItemClicked(object sender, EventArgs e)
        {
            string itemName = ItemNameEntry.Text?.Trim();

            if (string.IsNullOrEmpty(itemName))
            {
                StatusLabel.Text = "Please enter an item name.";
                return;
            }

            await DatabaseService.AddItemAsync(itemName);

            StatusLabel.Text = $"Item \"{itemName}\" saved to your list!";
            ItemNameEntry.Text = string.Empty;

            await LoadGroceryItems(); // Refresh list after adding
        }

        private async Task LoadGroceryItems()
        {
            List<GroceryItem> items = await DatabaseService.GetItemsAsync();
            GroceryListView.ItemsSource = items;
>>>>>>> 8b7d3ce (Initial commit of working SmartCart features)
        }

        private void listItems_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            GroceryItem item = listItems.SelectedItem as GroceryItem;
            Database.AddToList(2, 3, 1, true);
            UpdateList();
        }

        private void UpdateList()
        {
            Database.PullList();
            listItems.ItemsSource = GroceryList.GetLatestList();
        }

    }
}



