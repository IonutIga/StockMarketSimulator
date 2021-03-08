using System;
using System.ComponentModel;
using System.Windows.Input;
using StockMarketSimulator.Services;
using Xamarin.Forms;

namespace StockMarketSimulator.ViewModel
{
    // ViewModel used for the UI logic of the Settings Page
    public class SettingsViewModel : INotifyPropertyChanged
    {

        private double rate = (double)App.Current.Properties["rate"];
        public double Rate
        {
            get
            {
                return Math.Round(rate, 2);
            }
            set
            {
                rate = Math.Round(value, 2);
                OnPropertyChanged("Rate");
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

        private TabbedViewModel vm;

        public ICommand CheckCommand { get; set; }
        public SettingsViewModel()
        {
            CheckCommand = new Command(SaveSettings);
            vm = App.Current.Resources["vm"] as TabbedViewModel;
        }


        /*
       * Method used to navigate to save settings; created for CheckCommand; Only for Romanian Currency
       * 
       * @arg
       *  argument
       */
        private async void SaveSettings(object arg)
        {
            if (double.IsNaN(Rate) || Rate <= 0)
                await App.Current.MainPage.DisplayAlert(Resources.Language.Alert, "Introduceți o valoare numerică pentru conversie mai mare decât 0!", "Ok");
            else
            {
                App.Current.Properties["rate"] = Rate;
                IsLoading = true;
                await App.Current.SavePropertiesAsync();
                vm.Budget = await App.myUser.RetrieveBudgetAsync(Auth.GetCurrentUserId());
                await App.Current.MainPage.Navigation.PopAsync();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
