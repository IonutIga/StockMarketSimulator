using System;
using System.ComponentModel;
using System.Windows.Input;
using StockMarketSimulator.Services;
using StockMarketSimulator.View;
using Xamarin.Forms;

namespace StockMarketSimulator.ViewModel
{
    // ViewModel used for the UI logic of the Login Page
    public class LoginViewModel : INotifyPropertyChanged
    {

        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
                OnPropertyChanged("CanLogin");
            }
        }
        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
                OnPropertyChanged("CanLogin");
            }
        }

        private bool isLoading = false;
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        // Property used to check if all the entries were filled in and to enable auto call of LoginCanExecute method
        public bool CanLogin
        {
            get
            {
                return (Helpers.EmailEntryBehavior.isEmailValid && Helpers.PasswordEntryBehavior.isPasswordValid);
            }
        }

        public ICommand LoginCommand { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public LoginViewModel()
        {

            LoginCommand = new Command(Login);
        }
       

        /*
         * Method used to login the user with introduced credentials; created for LoginCommand
         * 
         * @arg
         *  argument
         */
        private async void Login(object arg)
        {
           
            if (CanLogin)
                try
                {
                    IsLoading = true;
                    await Auth.auth.LoginUser(Email, Password);
                    await App.Current.MainPage.Navigation.PushAsync(new MainTabbedPage());
                    IsLoading = false;
                }
                catch (Exception e)
                {
                    IsLoading = false;
                    await App.Current.MainPage.DisplayAlert("Error", e.Message, "Ok");             
                }


        }

        /*
         * Method used when the value of a property changes, it enables trigger of anybody who uses that property
         * 
         * @propertyName
         *  The name of the property that changes value
         */
        private void OnPropertyChanged(string propertyName)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
