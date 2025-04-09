namespace SmartCart
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddItem), typeof(AddItem));
        }
    }
}
