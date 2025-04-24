using CommunityToolkit.Maui.Views;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartCart
{
    public partial class MainPage : ContentPage
    {
        public static List<int> QuantityOptions { get; } = new List<int> { 1, 2, 3, 4, 5 };
        public static List<string> PriorityOptions { get; } = new List<string> { "Low", "Medium", "High" };

        bool isLoading = false;
        private void QuantityPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var item = (GroceryItem)picker.BindingContext;
            if (item != null && picker.SelectedItem is int selectedQuantity)
            {
                item.Quantity = selectedQuantity;
                Database.UpdateQuantity(item.EntryID, selectedQuantity);
            }
        }
        private void PriorityPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var item = (GroceryItem)picker.BindingContext;
            if (item != null && picker.SelectedItem is string selectedPriority)
            {
                item.Priority = selectedPriority;
                int priorityID = Database.priorityDict[selectedPriority];
                Database.UpdatePriority(item.EntryID, priorityID);
            }
        }

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
            if(item != null)
            {
                currentState = Database.GetCheckState(item.EntryID);

                if(!isLoading && currentState != item.IsChecked)
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
            } else
            {
                return;
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
