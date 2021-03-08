using System.ComponentModel;

namespace StockMarketSimulator.ViewModel
{
    // ViewModel used for the UI logic of the Help Page
    public class HelpViewModel : INotifyPropertyChanged
    {
        // Used to indicate the video is loading
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



        public event PropertyChangedEventHandler PropertyChanged;


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
