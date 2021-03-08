using System.Threading.Tasks;

namespace StockMarketSimulator.Services
{
    // Interface used to get the user information
    public interface IMyUser
    {

        /*
         * Method used to retrieve the user's budget
         * 
         * @UserId
         *  User's Id
         * 
         */
        Task<double> RetrieveBudgetAsync(string UserId);

        /*
      * Method used to update the user's budget
      * 
      * @UserId
      *  User's Id
      * 
      * @quantity
      *  Quantity of bought stock
      *  
      * @Price
      *  Price of the bought stock
      */
        bool UpdateBudget(string UserId, int quantity, double Price);

    }
}
