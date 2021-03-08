using System;
using System.Threading.Tasks;
using Foundation;
using StockMarketSimulator.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(StockMarketSimulator.iOS.Dependencies.Auth))]
// Exporting this class in order to pe used in the shared code
namespace StockMarketSimulator.iOS.Dependencies
{
    // Class that implements IAuth on iOS, to use Firebase functionality
    class Auth : IAuth
    {
        // Method used to get the user's id
        public string GetCurrentUserId()
        {
            return Firebase.Auth.Auth.DefaultInstance.CurrentUser.Uid;
        }

        // Method used to verify if the user is logged in
        public bool IsLoggedIn()
        {

            if (Firebase.Auth.Auth.DefaultInstance.CurrentUser != null)
                return true;
            else return false;
        }

        /*
         * Method used to login the user with email and password from Firebase
         * 
         * @Email
         *  User's email
         * @Password
         * User's password
         */
        public async Task<bool> LoginUser(string Email, string Password)
        {
            try
            {
                await Firebase.Auth.Auth.DefaultInstance.SignInWithPasswordAsync(Email, Password);
                return true;
            }
            // Catching Firebase exception to convert them into Xamarin exceptions, in order to know about their existence
            catch (NSErrorException e)
            {
                // Universal exception, to get the message of the exception, an extraction of content is needed
                string Message = e.Message.Substring(e.Message.IndexOf("NSLocalizedDescription=", StringComparison.CurrentCulture));
                Message = Message.Replace("NSLocalizedDescription=", "").Split(".")[0];
                throw new Exception(Message);
            }
            catch (Exception e)
            {
                throw new Exception("An error occured, please try again!");
            }

        }

        // Method used to logout the user
        public bool LogoutUser()
        {
            try
            {
                return Firebase.Auth.Auth.DefaultInstance.SignOut(out NSError error);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

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
        public async Task<bool> RegisterUser(string Name, string Email, string Password)
        {

            try
            {
                await Firebase.Auth.Auth.DefaultInstance.CreateUserAsync(Email, Password);

                // Creating a profile change request in order to update the user's name
                var profileUpdateRequest = Firebase.Auth.Auth.DefaultInstance.CurrentUser.ProfileChangeRequest();
                profileUpdateRequest.DisplayName = Name;
                await profileUpdateRequest.CommitChangesAsync();
                return true;
            }
            // Catching Firebase exception to convert them into Xamarin exceptions, in order to know about their existence
            catch (NSErrorException e)
            {

                // Universal exception, to get the message of the exception, an extraction of content is needed
                string Message = e.Message.Substring(e.Message.IndexOf("NSLocalizedDescription=", StringComparison.CurrentCulture));
                Message = Message.Replace("NSLocalizedDescription=", "").Split(".")[0];
                throw new Exception(Message);
            }
            catch (Exception e)
            {
                throw new Exception("An error occured, please try again!");
            }


        }
    }
}