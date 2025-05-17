
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartCart
{
    public partial class CategorySelectionPage : ContentPage
    {
        public List<string> CategoryList { get; set; }

        public CategorySelectionPage()
        {
            InitializeComponent();


            CategoryList = Database.categoryDict.Keys.ToList();


            BindingContext = this;
        }


        private void CategoryFrame_Tapped(object sender, TappedEventArgs e)
        {
            var selectedCategory = e.Parameter as string;
            OnCategoryTapped(selectedCategory);
        }


        private async void OnCategoryTapped(string selectedCategory)
        {
            if (!string.IsNullOrWhiteSpace(selectedCategory) &&
                Database.categoryDict.ContainsKey(selectedCategory))
            {
                int categoryID = Database.categoryDict[selectedCategory];


                Database.UpdateCategorizedItems(categoryID);


                await Shell.Current.GoToAsync(nameof(AddItem));
            }
        }


        private async void OnBrowseAllItemsClicked(object sender, EventArgs e)
        {
            Database.UpdateCategorizedItems(0); // 0 = no filter
            await Shell.Current.GoToAsync(nameof(AddItem));
        }
    }
}
