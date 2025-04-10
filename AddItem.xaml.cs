using System;
using Microsoft.Maui.Controls;

namespace SmartCart
{
    public partial class AddItem : ContentPage
    {
        public AddItem()
        {
            InitializeComponent(); 
        }

        private async void OnAddItemClicked(object sender, EventArgs e)
{
    // Validate input
    if (string.IsNullOrWhiteSpace(ItemNameEntry.Text) ||
        string.IsNullOrWhiteSpace(QuantityEntry.Text) ||
        PriorityPicker.SelectedIndex == -1 ||
        CategoryPicker.SelectedIndex == -1)
    {
        await DisplayAlert("Error", "Please fill out all fields.", "OK");
        return;
    }

    // Convert quantity safely
    int quantity = 1;
    int.TryParse(QuantityEntry.Text, out quantity);

    // Map selected priority (1 = Low, 2 = Medium, 3 = High)
    int priority = PriorityPicker.SelectedIndex + 1;

    // Create new item using the constructor 
    GroceryItem newItem = new GroceryItem(
        GroceryList.GetLatestList().Count + 1, // ItemID
        ItemNameEntry.Text,                    // Name
        quantity,                              // Quantity
        priority,                              // Priority as int
        false                                   // IsChecked
    );

    // Add to static list or database here
    GroceryList.AddItem(newItem);

    await DisplayAlert("Success", "Item added!", "OK");
    await Shell.Current.GoToAsync(".."); // Navigate back
}

    }
}
