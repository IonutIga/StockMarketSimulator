
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StockMarketSimulator.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Helpers.EmailEntryBehavior.isEmailValid = false;
            Helpers.PasswordEntryBehavior.isPasswordValid = false;
        }
    }
}