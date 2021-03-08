using MediaManager;
using StockMarketSimulator.Services;
using StockMarketSimulator.View;
using Xamarin.Forms;

namespace StockMarketSimulator
{
    public partial class App : Application
    {
        // Used to enable IMyUser functionalities from platform specific code
        // Needed here to refer the same user's details, such as budget, all around the app
        public static IMyUser myUser = DependencyService.Get<IMyUser>();

        public App()
        {
            InitializeComponent();
            // Enable video loading package
            CrossMediaManager.Current.Init();
            // Set default rate
            if (!Properties.ContainsKey("rate"))
                Properties["rate"] = 4.88;
            // If the user is already logged in, show them the user menu (MainTabbedPage), else the main page(MainPage)
            if (!Auth.auth.IsLoggedIn())
                MainPage = new NavigationPage(new MainPage());
            else
                MainPage = new NavigationPage(new MainTabbedPage());

        }



        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
