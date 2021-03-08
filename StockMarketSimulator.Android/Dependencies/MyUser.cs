using System;
using System.Threading.Tasks;
using Android.Gms.Tasks;
using Firebase.Firestore;
using Plugin.Multilingual;
using StockMarketSimulator.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(StockMarketSimulator.Droid.Dependencies.MyUser))]
// Exporting this class in order to be used in the shared code
namespace StockMarketSimulator.Droid.Dependencies
{
    // Class that implements IMyUser on Android, to retrieve user related information
    public class MyUser : Java.Lang.Object, IMyUser, IOnCompleteListener
    {
        bool hasFoundDoc = false;
        double Budget;
        string Id;


        /*
         * Method used to retrieve the user's related budget
         * 
         * @UserId
         *  User's Id
         */
        public async Task<double> RetrieveBudgetAsync(string UserId)
        {
            var query = FirebaseFirestore.Instance.Collection("budgets").WhereEqualTo("userid", UserId);
            query.Get().AddOnCompleteListener(this);

            // Giving time to the request to be handled
            for (var i = 0; i < 25; i++)
            {
                await System.Threading.Tasks.Task.Delay(100);
                if (hasFoundDoc)
                    break;
            }

            return Budget;

        }

        /*
         * Method used to update the budget of a specific user
         * 
         * @ UserId
         *  User's Id
         * 
         * @quantity
         *  Quantity to buy of a specific stock
         * 
         * @Price
         *  Price at which a stock is bought
         */
        public bool UpdateBudget(string UserId, int quantity, double Price)
        {
            try
            {
                if (Budget < quantity * Price)
                    return false;
                var collection = FirebaseFirestore.Instance.Collection("budgets");

                if (CrossMultilingual.Current.CurrentCultureInfo.TwoLetterISOLanguageName.Equals("ro"))
                    collection.Document(Id).Update("budget", Budget / (double)App.Current.Properties["rate"] - (quantity * (Price / (double)App.Current.Properties["rate"])));
                else
                    collection.Document(Id).Update("budget", Budget - (quantity * Price));
                return true;
            }
            catch (Exception)
            { return false; }

        }

        // Method used to notify the app that the response from the server was received and how to handle the response
        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                var document = (QuerySnapshot)task.Result;
                foreach (var doc in document.Documents)
                {
                    Id = doc.Id;
                    Budget = (int)doc.Get("budget");
                    if (CrossMultilingual.Current.CurrentCultureInfo.TwoLetterISOLanguageName.Equals("ro"))
                        Budget = Budget * (double)App.Current.Properties["rate"];
                }

            }
        }


    }
}