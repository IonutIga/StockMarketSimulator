using MediaManager;
using Plugin.Multilingual;
using StockMarketSimulator.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StockMarketSimulator.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelpPage : ContentPage
    {
        private HelpViewModel vm;
        public HelpPage()
        {
            InitializeComponent();
            vm = Resources["vm"] as HelpViewModel;
            vm.IsLoading = true;
            initVideo();
        }

        // Method used to load the correct video based on current device language
        private async void initVideo()
        {

            if (!CrossMultilingual.Current.CurrentCultureInfo.TwoLetterISOLanguageName.Equals("ro"))
                await CrossMediaManager.Current.Play("https://firebasestorage.googleapis.com/v0/b/stockmarketsimulator-17d7d.appspot.com/o/XamarinEnFinal.mp4?alt=media&token=aab31b19-4438-4550-bc9b-fae277863fe0");
            else
                await CrossMediaManager.Current.Play("https://firebasestorage.googleapis.com/v0/b/stockmarketsimulator-17d7d.appspot.com/o/XamarinRoFinal.mp4?alt=media&token=20fa3f8d-a1de-464a-be89-1fff773c7b74");
            vm.IsLoading = false;
        }
    }
}