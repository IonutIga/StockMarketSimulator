using StockMarketSimulator.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StockMarketSimulator.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        ProfileViewModel vm;
        public ProfilePage()
        {
            InitializeComponent();

            // Find the binding context of the page, in order to use its methods
            vm = Resources["vm"] as ProfileViewModel;
        }

        // Call GetChartData everytime the page appears
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.GetChartData();
        }

    }
}