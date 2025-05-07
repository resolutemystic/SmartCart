using CommunityToolkit.Maui.Views;
using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.Maui.Controls; // Required for SelectionChangedEventArgs and ContentPage

namespace SmartCart;

public partial class AddItem : ContentPage
{
    private string selectedItemName;

    public AddItem()
    {
        InitializeComponent();

        // Set the item sources for your pickers and collection
        PriorityPicker.ItemsSource = Priorities;
        QuantityPicker.ItemsSource = Quantities;
        ItemCollectionView.ItemsSource = Items;
    }

    // Assuming your item list is a list of strings
    public List<string> Items => new(Database.categorizedItemDict.Keys);

    public List<string> Priorities => new(Database.priorityDict.Keys);

    public List<int> Quantities => Enumerable.Range(1, 50).ToList(); // 1–50 quantity range

    private void ItemCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            selectedItemName = e.CurrentSelection[0] as string;
        }
        else
        {
            selectedItemName = null;
        }
    }

    private async void OnAddItemClicked(object sender, EventArgs e)
    {
        // Safely retrieve selected values
        string name = selectedItemName;
        string priority = PriorityPicker.SelectedItem as string;
        var quantityItem = QuantityPicker.SelectedItem;

        // Validation
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(priority) || quantityItem == null)
        {
            await DisplayAlert("Missing Info", "Please select an item, quantity, and priority.", "OK");
            return;
        }

        int quantity = (int)quantityItem;

        int itemID = Database.groceryItemDict[name];
        int priorityID = Database.priorityDict[priority];
        int existing = Database.ExistingListItem(itemID);

        if (existing > 0)
        {
            bool increase = await DisplayAlert("Already Exists", $"{name} is already in your list. Would you like to increase the quantity by {quantity}?", "Yes", "Cancel");
            if (increase)
            {
                Database.IncreaseQuantity(existing, quantity);
            }
        }
        else
        {
            Database.AddToList(itemID, quantity, priorityID, false);
        }

        await Shell.Current.GoToAsync("///MainPage");
    }

    private async void OnGoBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }
}
