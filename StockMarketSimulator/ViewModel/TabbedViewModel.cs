using System.ComponentModel;
using System.Windows.Input;
using Plugin.Multilingual;
using StockMarketSimulator.Services;
using StockMarketSimulator.View;
using Xamarin.Forms;

namespace StockMarketSimulator.ViewModel
{
    // ViewModel used for the UI logic of the MainTapped Page
    public class TabbedViewModel : INotifyPropertyChanged

    {

        private double budget;
        public double Budget
        {
            get
            {
                return budget;
            }
            set
            {
                budget = value;
                OnPropertyChanged("Budget");

            }
        }

        public ICommand SettingsCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public TabbedViewModel()

        {
            SettingsCommand = new Command(Settings);
        }

        /*
      * Method used to navigate to settings page; created for SettingsCommand
      * 
      * @arg
      *  argument
      */
        private async void Settings(object arg)
        {
            if (CrossMultilingual.Current.CurrentCultureInfo.TwoLetterISOLanguageName.Equals("ro"))
                await App.Current.MainPage.Navigation.PushAsync(new SettingsPage());

        }

        // Method used to get the user's budget
        public async void ReadBudget()
        {
            Budget = await App.myUser.RetrieveBudgetAsync(Auth.GetCurrentUserId());
            OnPropertyChanged("Budget");
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
