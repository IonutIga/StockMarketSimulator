
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StockMarketSimulator.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

       protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Helpers.NameEntryBehavior.isNameValid = false;
            Helpers.EmailEntryBehavior.isEmailValid = false;
            Helpers.PasswordEntryBehavior.isPasswordValid = false;
            Helpers.ConfirmPasswordEntryBehavior.isConfirmPasswordValid = false;
        }
    }
}