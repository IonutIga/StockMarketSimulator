using StockMarketSimulator.Model;
using StockMarketSimulator.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StockMarketSimulator.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsPage : ContentPage
    {
        DetailsViewModel detailsViewModel;
        public DetailsPage(Stock stock)
        {

            InitializeComponent();

            // Find the binding context of the page, in order to use its properties
            detailsViewModel = Resources["vm"] as DetailsViewModel;

            // Stock that was tapped
            detailsViewModel.StockTapped = stock;

            // Set here because the Stock property of the vm is set after the creation of the view model

            detailsViewModel.Maximum = (int)(((TabbedViewModel)App.Current.Resources["vm"]).Budget / stock.NowPrice) == 0 ? 0.1 : (int)(((TabbedViewModel)App.Current.Resources["vm"]).Budget / stock.NowPrice);


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            // Hide the Loading Screen until the stock is bought
            detailsViewModel.IsLoading = false;
        }
    }
}