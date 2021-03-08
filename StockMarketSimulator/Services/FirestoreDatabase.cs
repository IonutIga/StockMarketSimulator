using System.Collections.Generic;
using System.Threading.Tasks;
using StockMarketSimulator.Model;
using Xamarin.Forms;

namespace StockMarketSimulator.Services
{
    // Interface used to get the stock market
    public interface IFirestore
    {
        Task<IList<Stock>> RetrieveStock(string StockName);

    }

    // Interface used to interact with user's owned stocks
    public interface IMyFirestore
    {
        Task<bool> InsertMyStock(Stock mystock, string UserId, int quantity);
        bool DeleteMyStock(MyStock mystock);
        bool UpdateMyStock(MyStock mystock, int quantity);
        Task<IList<MyStock>> RetrieveMyStock(string UserId);

    }

    // Class used to enable Firestore functionalities into the shared project
    class FirestoreDatabase
    {
        // Instance of platform-specific class, in order to use Firestore services, used for user's actions
        private static IMyFirestore Myfirestore = DependencyService.Get<IMyFirestore>();

        // Instance of platform-specific class, in order to use Firestore services, used for market actions
        private static IFirestore firestore = DependencyService.Get<IFirestore>();

        // Deletes a stock owned by the user
        public static bool DeleteMyStock(MyStock mystock)
        {
            return Myfirestore.DeleteMyStock(mystock);
        }

        // Inserts the stock to the user portofolio (buying operation)
        public static Task<bool> InsertMyStock(Stock mystock, string UserId, int quantity)
        {
            return Myfirestore.InsertMyStock(mystock, UserId, quantity);
        }

        // Retrieves the entire list of stocks owned by the user
        public static Task<IList<MyStock>> RetrieveMyStock(string UserId)
        {
            return Myfirestore.RetrieveMyStock(UserId);
        }

        // Retrieves the entire list of stocks available on the market
        public static Task<IList<Stock>> RetrieveStock(string StockName)
        {
            return firestore.RetrieveStock(StockName);
        }

        // Updates the quantity of a stock owned, after selling
        public static bool UpdateMyStock(MyStock mystock, int quantity)
        {
            return Myfirestore.UpdateMyStock(mystock, quantity);
        }
    }
}
