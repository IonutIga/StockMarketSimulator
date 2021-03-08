using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Microcharts;
using StockMarketSimulator.Model;
using StockMarketSimulator.Services;
using StockMarketSimulator.View;
using Xamarin.Forms;

namespace StockMarketSimulator.ViewModel
{
    // ViewModel used for the UI logic of the Profile Page
    class ProfileViewModel : INotifyPropertyChanged
    {


        private Chart stocksOwnedChart;
        public Chart StocksOwnedChart
        {
            get
            {
                return stocksOwnedChart;
            }
            set
            {
                stocksOwnedChart = value;
                OnPropertyChanged("StocksOwnedChart");
            }
        }

        private string name = App.Current.Properties["Name"] as string;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        private string email = App.Current.Properties["Email"] as string;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }
        public ICommand LogoutCommand { get; set; }
        private IList<ChartEntry> chartEntries = new List<ChartEntry>();
        private Dictionary<string, float> statistics = new Dictionary<string, float>();
        private Random random = new Random();

        public event PropertyChangedEventHandler PropertyChanged;

        public ProfileViewModel()

        {
            LogoutCommand = new Command(Logout);
        }

        // Method used to retrieve data for the chart; read the user's current owend stocks list, group them and calculate the total quantity of each and all together
        public async void GetChartData()
        {
            // No need of old statistics
            chartEntries.Clear();
            statistics.Clear();
            // For total owned stocks
            float quantity = 0;
            IList<MyStock> list = await FirestoreDatabase.RetrieveMyStock(Auth.GetCurrentUserId());

            // Create the groups
            foreach (var stock in list)
            {
                if (statistics.ContainsKey(stock.ShortName))
                    statistics[stock.ShortName] = (float)statistics[stock.ShortName] + stock.Quantity;
                else
                    statistics.Add(stock.ShortName, stock.Quantity);

            }

            // Create entries for the chart
            foreach (var statistic in statistics)
            {
                quantity += statistic.Value;
                ChartEntry chartEntry = new ChartEntry(statistic.Value);
                chartEntry.ValueLabel = statistic.Value.ToString();
                chartEntry.Label = statistic.Key;
                chartEntry.Color = SkiaSharp.SKColor.FromHsv(random.Next(0, 361), random.Next(0, 101), random.Next(0, 101));

                chartEntries.Add(chartEntry);
            }

            // Total stocks owned
            chartEntries.Add(new ChartEntry(quantity)
            {
                Color = SkiaSharp.SKColor.FromHsv(random.Next(0, 361), random.Next(0, 101), random.Next(0, 101)),
                Label = Resources.Language.Owned,
                ValueLabel = quantity.ToString(),
            }
            );


            StocksOwnedChart = new BarChart();
            StocksOwnedChart.LabelTextSize = 64;
            StocksOwnedChart.Entries = chartEntries;

        }

        /*
         * Method used to logout the current user; create for the LogoutCommand
         * 
         * @arg
         *  argument
         * 
         */
        public async void Logout(Object arg)
        {
            try
            {
                Auth.auth.LogoutUser();
                await App.Current.MainPage.Navigation.PushAsync(new MainPage());
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
