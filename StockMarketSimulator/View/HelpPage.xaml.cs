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
                await CrossMediaManager.Current.Play("https://firebasestorage.googleapis.com/v0/b/stockmarketsimulator-17d7d.appspot.com/o/EnTutorial.mp4?alt=media&token=06f3f993-72a2-46f5-8ca1-f7fa9220abd6");
            else
                await CrossMediaManager.Current.Play("https://firebasestorage.googleapis.com/v0/b/stockmarketsimulator-17d7d.appspot.com/o/RoTutorial.mp4?alt=media&token=b9916f2e-d48e-48fe-ab95-07af987eada2");
            vm.IsLoading = false;
        }
    }
}