namespace SmartCart
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
            GroceryItem item = listItems.SelectedItem as GroceryItem;
            Database.AddToList(2, 3, 1, true);
            UpdateList();
        }

        private void UpdateList()
        {
            Database.PullList();
            listItems.ItemsSource = GroceryList.GetLatestList();
        }

    }

}
