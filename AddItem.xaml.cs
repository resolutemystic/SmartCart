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
        PriorityPicker.SelectedIndex = 0;
        QuantityPicker.ItemsSource = Enumerable.Range(1, 99).ToList<int>();
        QuantityPicker.SelectedIndex = 0;
        ItemNameList.ItemsSource = Items;
	}

    public Dictionary<string, int> Items
    {
        get { return Database.categorizedItemDict; }
    }

    public List<string> Priorities
    {
        get { return new List<string>(Database.priorityDict.Keys); }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        KeyValuePair<string, int> item = (KeyValuePair<string, int>)ItemNameList.SelectedItem;
        string priority = (string)PriorityPicker.SelectedItem;

        int itemID = 1;
        if(ItemNameList.SelectedItem != null)
        {
            itemID = item.Value;
        }

        int priorityID = 1;
        if(PriorityPicker.SelectedItem != null)
        {
            priorityID = Database.priorityDict[priority];
        }

        int quantity = 1;
        if(QuantityPicker.SelectedItem != null)
        {
            quantity = (int)QuantityPicker.SelectedItem;
        }

        int existing = Database.ExistingListItem(itemID);
        if (existing > 0)
        {
            if (await DisplayAlert("Already Exists", $"{item.Key} is already in your list. Would you like to increase the quantity by {quantity}?", "Yes", "Cancel"))
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

