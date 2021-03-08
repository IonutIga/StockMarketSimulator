using StockMarketSimulator.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StockMarketSimulator.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        HomeViewModel vm;
        public HomePage()
        {
            InitializeComponent();

            // Find the binding context of the page, in order to use its methods
            vm = (HomeViewModel)Resources["vm"];

        }

        // Call ReadStocks everytime the page appears
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            // Show Loading Screen
            vm.IsLoading = true;

            // Hide No Stocks Found
            vm.ShowNoStocks = false;

            await vm.ReadStocks();

        }

        /*
         * Method used to enable moving to details page for any stock tapped; ItemTapped event is not a bindable property
         * 
         * @sender
         *  Sender of the event
         * 
         * @args
         *  Event arguments
         */
        public void Details(object sender, ItemTappedEventArgs args)
        {
            vm.DetailsCommand.Execute(args.Item);
        }



    }
}