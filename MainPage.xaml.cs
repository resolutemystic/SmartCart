using CommunityToolkit.Maui.Views;
using System.Threading.Tasks;
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
            try
            {
                Database.PullList();
                listItems.ItemsSource = GroceryList.GetLatestList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error pulling list: {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
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
            else
            {
                return;
            }
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var delete = (ImageButton)sender;
            var item = (GroceryItem)delete.BindingContext;

            bool yes = await DisplayAlert("Are you sure?", $"Delete {item.Name} from your current list?", "Yes", "Cancel");

            if (item != null && yes)
            {
                Database.DeleteEntry(item.EntryID);
                UpdateList();
            }
        }

        private async void QuantityPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var item = (GroceryItem)picker.BindingContext;

            if (item != null && picker.SelectedItem is int selectedQuantity && selectedQuantity != item.Quantity)
            {
                bool yes = await DisplayAlert("Are you sure?", $"Change the quantity of {item.Name} to {selectedQuantity}?", "Yes", "Cancel");
                if (yes)
                {
                    Database.UpdateQuantity(item.EntryID, selectedQuantity);
                    item.Quantity = selectedQuantity;
                }
                else
                {
                    picker.SelectedItem = item.Quantity;
                }
            }
        }

        private async void PriorityPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var item = (GroceryItem)picker.BindingContext;

            if (item != null && picker.SelectedItem is string selectedPriority && selectedPriority != item.Priority)
            {
                bool yes = await DisplayAlert("Are you sure?", $"Change the priority of {item.Name} to {selectedPriority}?", "Yes", "Cancel");
                if (yes)
                {
                    Database.UpdatePriority(item.EntryID, Database.priorityDict[selectedPriority]);
                    item.Priority = selectedPriority;
                }
                else
                {
                    picker.SelectedItem = item.Priority;
                }
            }
        }
    }
}
