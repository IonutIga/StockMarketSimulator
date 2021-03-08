using StockMarketSimulator.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StockMarketSimulator.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {

        private SettingsViewModel vm;
        public SettingsPage()
        {
            InitializeComponent();
            // Find the binding context of the page, in order to use its properties
            vm = Resources["vm"] as SettingsViewModel;

        }

        // Set IsLoading to false, until save button is clicked
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.IsLoading = false;

        }
    }


}