﻿using System;
using System.ComponentModel;
using System.Windows.Input;
using StockMarketSimulator.Services;
using StockMarketSimulator.View;
using Xamarin.Forms;

namespace StockMarketSimulator.ViewModel
{
    // ViewModel used for the UI logic of the Register Page 
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("CanRegister");
            }
        }
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
                OnPropertyChanged("CanRegister");
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
                OnPropertyChanged("CanRegister");
            }
        }
        private string _confirmedpassword;
        public string ConfirmedPassword
        {
            get
            {
                return _confirmedpassword;
            }
            set
            {
                _confirmedpassword = value;
                OnPropertyChanged("ConfirmedPassword");
                OnPropertyChanged("CanRegister");
            }
        }

        // Property used to check if all the entries were filled in and to enable auto call of RegisterCanExecute method
        public bool CanRegister
        {
            get
            {
                if (!string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(ConfirmedPassword) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Name))
                    if (Password.Equals(ConfirmedPassword))
                        return true;
                return false;
            }
        }

        public ICommand RegisterCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public RegisterViewModel()
        {

            RegisterCommand = new Command(Register, RegisterCanExecute);
        }

        /*
         * Method used to enable register
         * 
         * @arg
         *  argument
         */
        private bool RegisterCanExecute(object arg)
        {
            return CanRegister;
        }

        /*
        * Method used to register the user with introduced credentials; created for RegisterCommand
        * 
        * @arg
        *  argument
        */
        private async void Register(object arg)
        {
            try
            {
                await Auth.auth.RegisterUser(Name, Email, Password);
                await App.Current.MainPage.Navigation.PushAsync(new MainTabbedPage());
            }
            catch (Exception e)
            {
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
