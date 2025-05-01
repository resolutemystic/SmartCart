using CommunityToolkit.Maui.Views;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace SmartCart;

public partial class AddItem : ContentPage
{
	public AddItem()
	{
		InitializeComponent();

        PriorityPicker.ItemsSource = Priorities;
        ItemNamePicker.ItemsSource = Items;
	}

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        // If the text field is empty or null then leave.
        string regex = e.NewTextValue;
        if (String.IsNullOrEmpty(regex))
            return;

        // If the text field only contains numbers then leave.
        if (!Regex.Match(regex, "^[0-9]+$").Success)
        {
            // This returns to the previous valid state.
            var entry = sender as Entry;
            entry.Text = (string.IsNullOrEmpty(e.OldTextValue)) ?
                    string.Empty : e.OldTextValue;
        }


    }

    public List<string> Items
    {
        get { return new List<string>(Database.categorizedItemDict.Keys); }
    }

    public List<string> Priorities
    {
        get { return new List<string>(Database.priorityDict.Keys); }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        string name = (string)ItemNamePicker.SelectedItem;
        string priority = (string)PriorityPicker.SelectedItem;

        int itemID = 1;
        if(ItemNamePicker.SelectedItem != null)
        {
            itemID = Database.groceryItemDict[name];
        }

        int priorityID = 1;
        if(PriorityPicker.SelectedItem != null)
        {
            priorityID = Database.priorityDict[priority];
        }

        int quantity = 1;
        if(QuantityEntry.Text != null)
        {
            quantity = Convert.ToInt32(QuantityEntry.Text);
        }

        int existing = Database.ExistingListItem(itemID);
        if (existing > 0)
        {
            if (await DisplayAlert("Already Exists", $"{name} is already in your list. Would you like to increase the quantity by {quantity}?", "Yes", "Cancel"))
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
    private async void OnSelectFromCategoryClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CategorySelectionPage));
    }
   
    
}

