using System.Windows.Input;
using StockMarketSimulator.View;
using Xamarin.Forms;

namespace StockMarketSimulator.ViewModel
{
    // ViewModel used for the UI logic of the Main Page
    public class MainViewModel
    {
        public ICommand RegisterCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand HelpCommand { get; set; }
        public ImageSource ImageUri { get; set; }

        public MainViewModel()
        {

            RegisterCommand = new Command(Register);
            LoginCommand = new Command(Login);
            HelpCommand = new Command(Help);



        }

        // Method used to enable navigation to Help Page
        private async void Help(object obj)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new HelpPage());
        }

        // Method used to enable navigation to Register Page
        private async void Register()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }

        // Method used to enable navigation to Login Page
        private async void Login()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }


    }
}
