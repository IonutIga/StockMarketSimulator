using StockMarketSimulator.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StockMarketSimulator.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        TabbedViewModel vm;
        public MainTabbedPage()
        {

            InitializeComponent();
            // Menu pages
            Children.Add(new MyHomePage());
            Children.Add(new HomePage());
            Children.Add(new ProfilePage());

            // No need of the back button on the menu
            NavigationPage.SetHasBackButton(this, false);

            // Find the binding context of the page, in order to use its properties
            vm = App.Current.Resources["vm"] as TabbedViewModel;
        }

        // Call ReadBudget everytime the page appears
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.ReadBudget();

        }
    }
}