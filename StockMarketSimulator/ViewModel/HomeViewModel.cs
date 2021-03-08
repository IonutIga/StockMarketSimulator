using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using StockMarketSimulator.Model;
using StockMarketSimulator.Services;
using StockMarketSimulator.View;
using Xamarin.Forms;

namespace StockMarketSimulator.ViewModel
{
    // ViewModel used for the UI logic of the Home Page
    public class HomeViewModel : INotifyPropertyChanged
    {
        // Holds the list of stocks available on the market
        public ObservableCollection<Stock> StockList { get; set; }
        public ICommand DetailsCommand { get; set; }

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

        // Used to indicate no stocks on the market, enable the Label
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
        public event PropertyChangedEventHandler PropertyChanged;
        public HomeViewModel()
        {

            StockList = new ObservableCollection<Stock>();
            DetailsCommand = new Command(Details);

        }


        // Uses Firestore service for reading stocks available on the market
        public async Task<bool> ReadStocks()
        {
            var stocks = await FirestoreDatabase.RetrieveStock("");
            StockList.Clear();
            foreach (var s in stocks)
            {
                StockList.Add(s);
            }
            IsLoading = false;
            OnPropertyChanged("StockList");

            // No stocks on the market
            if (StockList.Count == 0)
            {
                ShowNoStocks = true;
                OnPropertyChanged("ShowNoStocks");
            }

            return true;

        }

        /*
       * Method used to navigate to details page of a specific stock; created for DetailsCommand
       * 
       * @arg
       *  argument
       */
        private async void Details(object arg)
        {
            Stock stock = (Stock)arg;
            await App.Current.MainPage.Navigation.PushAsync(new DetailsPage(stock));

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
