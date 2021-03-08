using System.Threading.Tasks;
using Xamarin.Forms;

namespace StockMarketSimulator.Services
{

    // An interface used for dependency injection of Firebase services from native apps
    public interface IAuth
    {
        Task<bool> RegisterUser(string Name, string Email, string Password);
        Task<bool> LoginUser(string Email, string Password);
        bool LogoutUser();
        bool IsLoggedIn();
        string GetCurrentUserId();
    }

    // Used to access Firebase services in the shared project
    public class Auth
    {
        // Variable used to access platform-specific type of Auth class
        public static IAuth auth = DependencyService.Get<IAuth>();

        /*
         * Method used to register the user with email and password with Firebase
         * 
         * @Name 
         *  User's name
         * @Email
         *  User's email
         * @Password
         * User's password
         */
        async public static Task<bool> RegisterUser(string Name, string Email, string Password)
        {
            return await auth.RegisterUser(Name, Email, Password);
        }

        /*
         * Method used to login the user with email and password from Firebase
         * 
         * @Email
         *  User's email
         * @Password
         * User's password
         */
        async public static Task<bool> LoginUser(string Email, string Password)
        {
            return await auth.LoginUser(Email, Password);
        }

        // Method used to logout the user
        public static bool LogoutUser()
        {
            return auth.LogoutUser();
        }

        // Method used to verify if the user is logged in
        public static bool IsLoggedIn()
        {
            return auth.IsLoggedIn();
        }

        // Method used to get the user's id
        public static string GetCurrentUserId()
        {
            return auth.GetCurrentUserId();
        }
    }
}
