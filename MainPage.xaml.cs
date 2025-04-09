﻿namespace SmartCart
{
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();
            listItems.ItemsSource = GroceryList.GetLatestList();
        }

        public List<GroceryItem> List
        {
            get { return GroceryList.GetLatestList(); }
        }

        private void listItems_ItemTapped(object sender, ItemTappedEventArgs e)
        {
        }

        private void NewItemBtnClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync(nameof(AddItem));
        }

        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            // Perform required operation after examining e.Value
        }

        private void UpdateList()
        {
            Database.PullList();
            listItems.ItemsSource = GroceryList.GetLatestList();
        }

    }

}
