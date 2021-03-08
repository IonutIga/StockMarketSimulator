using Xamarin.Forms;

namespace StockMarketSimulator.Model
{
    public class MyStock
    {
        public string MyStockId { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public double BuyPrice { get; set; }
        public ImageSource ImageUri { get; set; }
        public int Quantity { get; set; }

        // Used to specify the rate at which the stock was bought; only for Ro
        public double RonRate { get; set; }

        // Used for showing sell button of a specific MyStock
        public bool IsVisible { get; set; } = false;
    }
}
