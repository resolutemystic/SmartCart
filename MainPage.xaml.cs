using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace SmartCart
{
    public partial class MainPage : ContentPage
    {
        bool isLoading = false;

        public MainPage()
        {
            InitializeComponent();
            UpdateList();
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

        private void UpdateList()
        {
            isLoading = true;
            Database.PullList();
            listItems.ItemsSource = GroceryList.GetLatestList();
            isLoading = false;
        }

        private void CheckBox_CheckedChanged(System.Object sender, Microsoft.Maui.Controls.CheckedChangedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            var item = (GroceryItem)checkbox.BindingContext;
            bool currentState;
            if(item != null)
            {
                currentState = Database.GetCheckState(item.EntryID);

                if(!isLoading && currentState != item.IsChecked)
                {
                    Database.UpdateCheck(item.EntryID);
                }
            }


        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var delete = (ImageButton)sender;
            var item = (GroceryItem)delete.BindingContext;

            if (item != null)
            {
                Database.DeleteEntry(item.EntryID);
                UpdateList();
            }
        }
    }

}
