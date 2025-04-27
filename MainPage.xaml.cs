using CommunityToolkit.Maui.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq; // <-- Needed for .Any()

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

        protected override void OnAppearing()
        {
            UpdateList();
        }

        public List<GroceryItem> List
        {
            get { return GroceryList.GetLatestList(); }
        }

        private void NewItemBtnClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync(nameof(CategorySelectionPage));
        }

        public void UpdateList()
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

            if (item != null)
            {
                currentState = Database.GetCheckState(item.EntryID);

                if (!isLoading && currentState != item.IsChecked)
                {
                    Database.UpdateCheck(item.EntryID);
                }
            }

            UpdateDeleteSelectedButtonVisibility();
        }

        private void UpdateDeleteSelectedButtonVisibility()
        {
            var list = GroceryList.GetLatestList();
            bool hasChecked = list.Any(i => i.IsChecked);
            DeleteSelectedButton.IsVisible = hasChecked;
        }

        private async void DeleteSelected_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Confirm Deletion", "Would you like to delete all selected items from the list?", "Yes", "No");
            if (answer)
            {
                Database.DeleteCheckedItems();
                UpdateList();
                DeleteSelectedButton.IsVisible = false;
            }
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var delete = (ImageButton)sender;
            var item = (GroceryItem)delete.BindingContext;

            if (item != null)
            {
                bool answer = await DisplayAlert(
                    "Confirm Deletion",
                    $"Are you sure you want to delete {item.Name} from your list?",
                    "Yes",
                    "No");

                if (answer)
                {
                    Database.DeleteEntry(item.EntryID);
                    UpdateList();
                }
                else
                {
                    // User cancelled, do nothing
                    return;
                }
            }
        }
    }
}