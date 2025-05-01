using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
namespace SmartCart;


    public partial class CategorySelectionPage : ContentPage
{
    
    public List<string> CategoryList { get; set; }

    public CategorySelectionPage()
    {
        InitializeComponent();

        
        CategoryList = Database.categoryDict.Keys.ToList();

        
        BindingContext = this;
    }

    private void OnCategoryChanged(object sender, EventArgs e)
    {
        string selectedCategory = CategoryPicker.SelectedItem?.ToString();

        if (!string.IsNullOrWhiteSpace(selectedCategory) &&
            Database.categoryDict.ContainsKey(selectedCategory))
        {
            int categoryID = Database.categoryDict[selectedCategory];

            // This updates the item list bound to AddItem
            Database.UpdateCategorizedItems(categoryID);

            // Navigate to AddItem page
            Shell.Current.GoToAsync(nameof(AddItem));
        }
    }

    private async void OnBrowseAllItemsClicked(object sender, EventArgs e)
    {
        Database.UpdateCategorizedItems(0); // resets to unfiltered
        await Shell.Current.GoToAsync(nameof(AddItem));
    }

}
