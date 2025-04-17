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

    private void Button_Clicked(object sender, EventArgs e)
    {
        string name;
        string priority;
        int quantity;

        int itemID = 1;
        if(ItemNamePicker.SelectedItem != null)
        {
            name = (string)ItemNamePicker.SelectedItem;
            itemID = Database.groceryItemDict[name];
        }

        int priorityID = 1;
        if(PriorityPicker.SelectedItem != null)
        {
            priority = (string)PriorityPicker.SelectedItem;
            priorityID = Database.priorityDict[priority];
        }

        quantity = 1;
        if(QuantityEntry.Text != null)
        {
            quantity = Convert.ToInt32(QuantityEntry.Text);
        }

        Database.AddToList(itemID, quantity, priorityID, false);
        Navigation.PopAsync();
    }
}