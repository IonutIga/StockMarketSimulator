using Xamarin.Forms;

namespace StockMarketSimulator.Model
{
    public class Stock
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public double NowPrice { get; set; }
        public ImageSource ImageUri { get; set; }
        public double OldPrice { get; set; }
        public double Statistic { get; set; }
        public string CompanyDescription { get; set; }

        // Color of the text showing the difference between new and old stock price
        public Color StatisticColor
        {
            get

            {
                if (Statistic < 0)
                    return Color.Red;
                else return Color.Green;
            }
        }


    }
}
