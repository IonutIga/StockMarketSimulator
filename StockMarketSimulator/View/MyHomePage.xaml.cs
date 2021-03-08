using StockMarketSimulator.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StockMarketSimulator.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyHomePage : ContentPage
    {
        MyHomeViewModel vm;
        public MyHomePage()
        {
            InitializeComponent();
            // Find the binding context of the page, in order to use its methods
            vm = (MyHomeViewModel)Resources["vm"];
        }

        // Call ReadMyStocks each time the page appears
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //Show Loading Screen
            vm.IsLoading = true;

            // Hide No Stocks Found
            vm.ShowNoStocks = false;

            await vm.ReadMyStocks();
        }

        /*
        * Method used to show sell options for any stock tapped; ItemTapped event is not a bindable property
        * 
        * @sender
        *  Sender of the event
        * 
        * @args
        *  Event arguments
        */
        private void Tapped(object sender, ItemTappedEventArgs args)
        {
            vm.TappedCommand.Execute(args.Item);
        }
    }
}