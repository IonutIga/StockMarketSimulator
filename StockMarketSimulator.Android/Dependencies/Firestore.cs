using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Gms.Tasks;
using Firebase.Firestore;
using Plugin.Multilingual;
using StockMarketSimulator.Model;
using StockMarketSimulator.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(StockMarketSimulator.Droid.Dependencies.Firestore))]
// Exporting this class in order to be used in the shared code
namespace StockMarketSimulator.Droid.Dependencies
{
    // Class that implements IFirestore on Android, to use Firestore functionality
    // On Android, a listener is needed in order to be notified when a response is got from the server; It has to be a java lang object
    // Used for market actions
    public class Firestore : Java.Lang.Object, IFirestore, IOnCompleteListener
    {
        // List of stocks available on the market
        List<Stock> Stocks;

        // Checks whether a response was got or not
        bool hasReadStocks = false;
        public Firestore()
        {
            Stocks = new List<Stock>();
        }


        /* Method used to retrieve the entire list of stocks available on the market; same method for a unique stock; no need for another class
         *
         *@StockName
         * The stock to retrieve
         *
         */
        public async Task<IList<Stock>> RetrieveStock(string StockName)
        {
            try
            {

                var collection = Firebase.Firestore.FirebaseFirestore.Instance.Collection("stocks");
                if (string.IsNullOrEmpty(StockName))
                {
                    var query = collection.WhereGreaterThan("nowprice", 0);

                    // Add the listener to this object, to know when the response was received
                    query.Get().AddOnCompleteListener(this);
                }
                else
                {
                    var query = collection.WhereEqualTo("shortname", StockName);
                    query.Get().AddOnCompleteListener(this);
                }
                // Giving time to the request to be handled
                for (var i = 0; i < 25; i++)
                {
                    await System.Threading.Tasks.Task.Delay(300);
                    if (hasReadStocks)
                        break;
                }
                return Stocks;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        /*
         * Method used to notify the app that the response from the server was received and how to handle the response
         * @mystock
         *  The stock to be deleted
         */
        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                var documents = (QuerySnapshot)task.Result;
                Stocks.Clear();
                foreach (var doc in documents.Documents)
                {
                    Stock stock = new Stock()
                    {
                        ShortName = (string)doc.Get("shortname"),
                        LongName = (string)doc.Get("longname"),
                        NowPrice = (double)doc.Get("nowprice"),
                        OldPrice = (double)doc.Get("oldprice"),
                        ImageUri = ImageSource.FromUri(new Uri((string)doc.Get("imageuri"))),
                        CompanyDescription = (string)doc.Get("companydescription")


                    };

                    // Change the price based on the current rate
                    if (CrossMultilingual.Current.CurrentCultureInfo.TwoLetterISOLanguageName.Equals("ro"))
                    {
                        stock.NowPrice = stock.NowPrice * (double)App.Current.Properties["rate"];
                        stock.OldPrice = stock.OldPrice * (double)App.Current.Properties["rate"];
                    }
                    stock.Statistic = Math.Round(stock.NowPrice - stock.OldPrice, 2);
                    Stocks.Add(stock);
                }
            }
            else
            {
                Stocks.Clear();
            }
            hasReadStocks = true;
        }
    }
}