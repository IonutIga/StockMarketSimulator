using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using StockMarketSimulator.Model;
using StockMarketSimulator.Services;
using Xamarin.Forms;

namespace StockMarketSimulator.ViewModel
{
    // ViewModel used for the UI logic of the MyHome Page
    public class MyHomeViewModel : INotifyPropertyChanged
    {
        // Holds the list of stocks owned by the user
        public ObservableCollection<MyStock> MyStockList { get; set; }
        public ICommand SellCommand { get; set; }
        public ICommand TappedCommand { get; set; }

        private double quantity;
        public double Quantity
        {
            get
            {
                return Math.Round(quantity, 0);
            }
            set
            {
                quantity = value; OnPropertyChanged("Quantity");
            }
        }

        private MyStock lastTappedStock = new MyStock();
        public MyStock LastTappedStock
        {
            get
            {
                return lastTappedStock;
            }
            set
            {
                lastTappedStock = value;
                OnPropertyChanged("LastTappedStock");
            }
        }

        private bool isLoading = true;
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

        // Indicates that the user has no stocks, enable the Label
        private bool showNoStocks = false;
        public bool ShowNoStocks
        {
            get
            {
                return showNoStocks;
            }
            set
            {
                showNoStocks = value;
                OnPropertyChanged("ShowNoStocks");
            }
        }

        // Used to show the current price in order to have a better view on whether selling or not
        public double NowPrice { get; set; }

        // Needed to update the Budget
        TabbedViewModel vm;

        public event PropertyChangedEventHandler PropertyChanged;

        public MyHomeViewModel()
        {

            MyStockList = new ObservableCollection<MyStock>();
            SellCommand = new Command(Sell);
            TappedCommand = new Command(TappedStock);
            // Find the binding context of tabbed page, in order to use its properties
            vm = App.Current.Resources["vm"] as TabbedViewModel;
        }

        // Uses Firestore service for reading stocks owned by the user
        public async Task<bool> ReadMyStocks()
        {
            var stocks = await FirestoreDatabase.RetrieveMyStock(Auth.GetCurrentUserId());
            MyStockList.Clear();
            foreach (var s in stocks)
            {
                MyStockList.Add(s);
            }

            // No stocks owned
            if (MyStockList.Count == 0)
            {
                ShowNoStocks = true;
                OnPropertyChanged("ShowNoStocks");
            }
            IsLoading = false;

            // Set the quantity to 0, to delete the old inserted value
            quantity = 0;
            return true;
        }

        /*
        * Method used to sell a stock owned by the user; created for SellCommand
        * 
        * @arg
        *  argument
        */
        private async void Sell(object arg)
        {


            // Delete
            if (LastTappedStock.Quantity == Quantity)
            {
                // Show the Sell process is loading
                IsLoading = true;
                FirestoreDatabase.DeleteMyStock(LastTappedStock);
                App.myUser.UpdateBudget(Auth.GetCurrentUserId(), (int)Quantity, -NowPrice);
                vm.Budget = await App.myUser.RetrieveBudgetAsync(Auth.GetCurrentUserId());

                await ReadMyStocks();
            }
            // Error
            else
                if (Quantity <= 0 || Quantity > LastTappedStock.Quantity)
            {

                await App.Current.MainPage.DisplayAlert(Resources.Language.Alert, Resources.Language.SellCheckMessage, "Ok");

            }
            // Update
            else
                if (LastTappedStock.Quantity > Quantity)
            {
                // Show the Sell process is loading
                IsLoading = true;
                // Negative quantity, because it is a sell operation
                FirestoreDatabase.UpdateMyStock(LastTappedStock, -(int)Quantity);
                App.myUser.UpdateBudget(Auth.GetCurrentUserId(), (int)Quantity, -NowPrice);
                vm.Budget = await App.myUser.RetrieveBudgetAsync(Auth.GetCurrentUserId());

                await ReadMyStocks();
            }


        }

        /*
        * Method used to show Sell options on tap; created for TappedCommand
        * 
        * @arg
        *  argument
        */
        private async void TappedStock(object arg)
        {
            IList<Stock> stocks = new List<Stock>();
            MyStock myStock = (MyStock)arg;

            // Change IsVisible property based on the last tapped item
            if (myStock == LastTappedStock)
                LastTappedStock.IsVisible = !LastTappedStock.IsVisible;
            else
            {
                myStock.IsVisible = true;
                LastTappedStock.IsVisible = false;
                LastTappedStock = myStock;

            }
            if (LastTappedStock.IsVisible)
            {
                // Show current price when stock is tapped
                stocks = await FirestoreDatabase.RetrieveStock(LastTappedStock.ShortName);
                NowPrice = stocks[0].NowPrice;
            }
            OnPropertyChanged("IsVisible");
            OnPropertyChanged("NowPrice");
            UpdateList();


        }

        // Method used to update the MyStockList, to show the updated version of it; this way IsVisible changes are seen
        private void UpdateList()
        {
            ObservableCollection<MyStock> UpdatedList = new ObservableCollection<MyStock>();
            foreach (var stock in MyStockList)
            {
                UpdatedList.Add(stock);
            }
            MyStockList = UpdatedList;
            OnPropertyChanged("MyStockList");
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
