namespace SmartCart
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Database.PullList();
            MainPage = new AppShell();
        }
    }
}
