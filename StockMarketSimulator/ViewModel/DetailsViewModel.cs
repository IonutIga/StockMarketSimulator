using System;
using System.ComponentModel;
using System.Windows.Input;
using StockMarketSimulator.Model;
using StockMarketSimulator.Services;
using Xamarin.Forms;

namespace StockMarketSimulator.ViewModel
{
    // ViewModel used for the UI logic of the Details Page
    public class DetailsViewModel : INotifyPropertyChanged

    {
        private double quantity;
        public double Quantity
        {
            get
            {
                // Only full stocks are okay
                return Math.Round(quantity, 0);
            }
            set
            {
                quantity = value; OnPropertyChanged("Quantity");
            }
        }
        private Stock stockTapped;
        public Stock StockTapped
        {
            get
            {
                return stockTapped;
            }
            set
            {
                stockTapped = value; OnPropertyChanged("StockTapped");
            }
        }

        // Max amount of stocks of a specific type that can be bought by a user, considering the current price and budget
        private double maximum = 1;
        public double Maximum
        {
            get
            {

                return maximum;
            }
            set
            {
                maximum = value; OnPropertyChanged("Maximum");
            }
        }

        // Used to indicate the buying process is loading
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

        // Needed to update the Budget
        private TabbedViewModel vm;

        public ICommand BuyCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public DetailsViewModel()
        {
            BuyCommand = new Command(Buy);
            // Find the binding context of tabbed page, in order to use its properties
            vm = App.Current.Resources["vm"] as TabbedViewModel;
        }



        /*
         * Method used to buy a stock from the market; created for BuyCommand
         * 
         * @arg
         *  argument
         */
        private async void Buy(object arg)
        {
            if (Quantity > 0)
            {
                // Check for budget criteria
                bool hasInserted = await FirestoreDatabase.InsertMyStock(StockTapped, Auth.GetCurrentUserId(), (int)Quantity);
                if (hasInserted)
                {

                    IsLoading = true;
                    vm.Budget = await App.myUser.RetrieveBudgetAsync(Auth.GetCurrentUserId());
                    await App.Current.MainPage.Navigation.PopAsync();
                }
                else
                    await App.Current.MainPage.DisplayAlert(Resources.Language.Alert, Resources.Language.BudgetCheckMessage, "Ok");
            }
            else
                await App.Current.MainPage.DisplayAlert(Resources.Language.Alert, Resources.Language.QuantityCheckMessage, "Ok");
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
