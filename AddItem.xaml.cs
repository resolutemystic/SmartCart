using CommunityToolkit.Maui.Views;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Globalization;
using System.ComponentModel;

namespace SmartCart;

public partial class AddItem : ContentPage
{
    public class SelectableItem : INotifyPropertyChanged
    {
        public string Key { get; set; }
        public int Value { get; set; }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    public List<SelectableItem> SelectableItems { get; set; }
    private SelectableItem selectedItem;

    public AddItem()
    {
        InitializeComponent();

        SelectableItems = Database.categorizedItemDict
       .Select(kvp => new SelectableItem { Key = kvp.Key, Value = kvp.Value })
       .ToList();

        QuantityPicker.ItemsSource = Enumerable.Range(1, 10).ToList();
        QuantityPicker.SelectedIndex = 0;

        PriorityPicker.ItemsSource = new List<string> { "Low", "Medium", "High" };
        PriorityPicker.SelectedIndex = 0;
        BindingContext = this;
        ItemCollectionView.ItemsSource = SelectableItems;

    }

    private void OnItemTapped(object sender, TappedEventArgs e)
    {
        var tappedItem = (SelectableItem)e.Parameter;

        foreach (var item in SelectableItems)
            item.IsSelected = false;

        tappedItem.IsSelected = true;
        selectedItem = tappedItem;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (selectedItem == null)
        {
            await DisplayAlert("No item selected", "Please select an item to add.", "OK");
            return;
        }

        string priority = (string)PriorityPicker.SelectedItem;
        int quantity = (int)QuantityPicker.SelectedItem;

        int itemID = selectedItem.Value;
        int priorityID = Database.priorityDict[priority];

        int existing = Database.ExistingListItem(itemID);
        if (existing > 0)
        {
            if (await DisplayAlert("Already Exists", $"{selectedItem.Key} is already in your list. Increase quantity by {quantity}?", "Yes", "Cancel"))
                Database.IncreaseQuantity(existing, quantity);
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

