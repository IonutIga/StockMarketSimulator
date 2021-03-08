using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Gms.Tasks;
using Firebase.Firestore;
using Plugin.Multilingual;
using StockMarketSimulator.Model;
using StockMarketSimulator.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(StockMarketSimulator.Droid.Dependencies.MyFirestore))]
// Exporting this class in order to be used in the shared code
namespace StockMarketSimulator.Droid.Dependencies
{
    // Class that implements IFirestore on Android, to use Firestore functionality
    // On Android, a listener is needed in order to be notified when a response is got from the server; It has to be a java lang object
    // Used for the user's actions upon stocks
    public class MyFirestore : Java.Lang.Object, IMyFirestore, IOnCompleteListener
    {
        // List of stocks owned by a user
        List<MyStock> MyStocks;

        // Checks whether a response was got or not
        bool hasReadStocks = false;
        public MyFirestore()
        {
            MyStocks = new List<MyStock>();
        }



        /*
         * Method used to delete a stock owned by a user
         * @mystock
         *  The stock to be deleted
         */
        public bool DeleteMyStock(MyStock mystock)
        {
            try
            {
                var collection = Firebase.Firestore.FirebaseFirestore.Instance.Collection("mystocks");
                collection.Document(mystock.MyStockId).Delete();
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }

        /*
         * Method used to insert a stock into the user's portofolio (buying operation)
         * @mystock
         *  The stock to be inserted
         */
        public async Task<bool> InsertMyStock(Stock mystock, string UserId, int quantity)
        {
            try
            {
                // If budget does not allow, user cannot buy the stock
                bool canBuy = App.myUser.UpdateBudget(UserId, quantity, mystock.NowPrice);
                if (!canBuy)
                    return false;
                var collection = Firebase.Firestore.FirebaseFirestore.Instance.Collection("mystocks");

                // Verify that the stock is already owned or not
                MyQuery myQuery = new MyQuery();
                MyStock UpdateStock = await myQuery.UpdateQuery(mystock, UserId);
                if (UpdateStock != null)
                {
                    UpdateMyStock(UpdateStock, quantity);
                    return true;
                }

                var stock = new Dictionary<string, Java.Lang.Object>
                {
                    { "userid", UserId },
                    { "shortname", mystock.ShortName},
                    { "longname", mystock.LongName },
                    { "quantity", quantity},
                    { "imageuri", mystock.ImageUri.ToString().Remove(0,5) },
                    {"ronrate", (double)App.Current.Properties["rate"] },
                    {"buyprice", Math.Round(CrossMultilingual.Current.CurrentCultureInfo.TwoLetterISOLanguageName.Equals("ro") ? mystock.NowPrice /  (double)App.Current.Properties["rate"] : mystock.NowPrice ,2) }
                };
                collection.Add(stock);
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }

        // Method used to retrieve the entire list of stocks owned by the user
        public async Task<IList<MyStock>> RetrieveMyStock(string UserId)
        {
            try
            {

                var collection = Firebase.Firestore.FirebaseFirestore.Instance.Collection("mystocks");
                var query = collection.WhereEqualTo("userid", UserId);

                // Add the listener to this object, to know when the response was received
                query.Get().AddOnCompleteListener(this);

                // Giving time to the request to be handled
                for (var i = 0; i < 25; i++)
                {
                    await System.Threading.Tasks.Task.Delay(300);
                    if (hasReadStocks)
                        break;
                }
                return MyStocks;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /*
         * Method used to update the quantity of a stock owned, after selling
         * @mystock
         *  The stock to be updated
         * @quantity
         *  The quantity with which the owned stock's quantity will be updated
         */
        public bool UpdateMyStock(MyStock mystock, int quantity)
        {
            try
            {
                var collection = Firebase.Firestore.FirebaseFirestore.Instance.Collection("mystocks");
                collection.Document(mystock.MyStockId).Update("quantity", mystock.Quantity + quantity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }


        // Method used to notify the app that the response from the server was received and how to handle the response
        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                var documents = (QuerySnapshot)task.Result;
                MyStocks.Clear();
                foreach (var doc in documents.Documents)
                {
                    MyStock stock = new MyStock()
                    {
                        MyStockId = doc.Id,
                        ShortName = (string)doc.Get("shortname"),
                        LongName = (string)doc.Get("longname"),
                        BuyPrice = (double)doc.Get("buyprice"),
                        Quantity = (int)doc.Get("quantity"),
                        RonRate = (double)doc.Get("ronrate"),
                        ImageUri = ImageSource.FromUri(new Uri((string)doc.Get("imageuri")))


                    };

                    // Convert the BuyPrice to the rate at which the stock was bought
                    if (CrossMultilingual.Current.CurrentCultureInfo.TwoLetterISOLanguageName.Equals("ro"))
                    {
                        stock.BuyPrice = stock.BuyPrice * stock.RonRate;
                    }
                    MyStocks.Add(stock);
                }
            }
            else
            {
                MyStocks.Clear();
            }
            hasReadStocks = true;
        }
    }

    // Used to query the collection for a specific MyStock to be updated
    class MyQuery : Java.Lang.Object, IOnCompleteListener
    {
        MyStock myStock = null;
        bool hasFoundDoc = false;

        /*
         * Method used to query the collection for a specific MyStock to be updated
         * 
         * @myStock
         *  Stock to be searched for
         * @UserId
         *  Current logged in user ID
         */
        public async Task<MyStock> UpdateQuery(Stock mystock, string UserId)
        {
            var collection = Firebase.Firestore.FirebaseFirestore.Instance.Collection("mystocks");
            Query query;
            if (CrossMultilingual.Current.CurrentCultureInfo.TwoLetterISOLanguageName.Equals("ro"))
                query = collection.WhereEqualTo("userid", UserId).WhereEqualTo("shortname", mystock.ShortName).WhereEqualTo("buyprice", CrossMultilingual.Current.CurrentCultureInfo.TwoLetterISOLanguageName.Equals("ro") ? mystock.NowPrice / (double)App.Current.Properties["rate"] : mystock.NowPrice).WhereEqualTo("ronrate", (double)App.Current.Properties["rate"]);
            else
                query = collection.WhereEqualTo("userid", UserId).WhereEqualTo("shortname", mystock.ShortName).WhereEqualTo("buyprice", CrossMultilingual.Current.CurrentCultureInfo.TwoLetterISOLanguageName.Equals("ro") ? mystock.NowPrice / (double)App.Current.Properties["rate"] : mystock.NowPrice);
            query.Get().AddOnCompleteListener(this);

            // Giving time to the request to be handled
            for (var i = 0; i < 25; i++)
            {
                await System.Threading.Tasks.Task.Delay(100);
                if (hasFoundDoc)
                    break;
            }
            return myStock;


        }

        // Method used to notify the app that the response from the server was received and how to handle the response
        public void OnComplete(Android.Gms.Tasks.Task task)
        {


            if (task.IsSuccessful)
            {
                var document = (QuerySnapshot)task.Result;
                foreach (var doc in document.Documents)
                {
                    myStock = new MyStock
                    {
                        MyStockId = doc.Id,
                        ShortName = (string)doc.Get("shortname"),
                        LongName = (string)doc.Get("longname"),
                        BuyPrice = (double)doc.Get("buyprice"),
                        Quantity = (int)doc.Get("quantity"),
                        ImageUri = (string)doc.Get("imageuri"),
                    };
                }
                hasFoundDoc = true;
            }
        }
    }


}
