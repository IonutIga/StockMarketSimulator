using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using StockMarketSimulator.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(StockMarketSimulator.Droid.Dependencies.Auth))]
// Exporting this class in order to be used in the shared code
namespace StockMarketSimulator.Droid.Dependencies
{


    // Class that implements IAuth on Android, to use Firebase functionality
    public class Auth : IAuth
    {

        public Auth()
        {

        }

        // Method used to get the user's id
        public string GetCurrentUserId()
        {
            return Firebase.Auth.FirebaseAuth.Instance.CurrentUser.Uid;
        }

        // Method used to verify if the user is logged in
        public bool IsLoggedIn()
        {
            if (Firebase.Auth.FirebaseAuth.Instance.CurrentUser != null)
                return true;
            return false;
        }

        // Method used to logout the user
        public bool LogoutUser()
        {
            try
            {
                Firebase.Auth.FirebaseAuth.Instance.SignOut();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);

            }
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
                await Firebase.Auth.FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(Email, Password);

                // Save user details as properties, to use them at each start of the app w/o sending a new request
                App.Current.Properties["Name"] = FirebaseAuth.Instance.CurrentUser.DisplayName;
                App.Current.Properties["Email"] = Email;
                await App.Current.SavePropertiesAsync();

                return true;
            }

            // Catching Firebase exception to convert them into Xamarin exceptions, in order to know about their existence
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                throw new Exception(e.Message);
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception)
            {
                throw new Exception("An error occured, please try again!");
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
                await Firebase.Auth.FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(Email, Password);

                // Creating a profile change request in order to update the user's name
                var profileChangeRequest = new Firebase.Auth.UserProfileChangeRequest.Builder();
                profileChangeRequest.SetDisplayName(Name);
                var build = profileChangeRequest.Build();

                var User = Firebase.Auth.FirebaseAuth.Instance.CurrentUser;
                await User.UpdateProfileAsync(build);

                // Create user's budget after creation of the user
                var collection = Firebase.Firestore.FirebaseFirestore.Instance.Collection("budgets");
                collection.Add(new Dictionary<string, Java.Lang.Object>
                {
                    {"userid", User.Uid },
                    {"budget", 100000 }
                });

                // Save user details as properties, to use them at each start of the app w/o sending a new request
                App.Current.Properties["Name"] = Name;
                App.Current.Properties["Email"] = Email;
                await App.Current.SavePropertiesAsync();
                return true;
            }

            // Catching Firebase exception to convert them into Xamarin exceptions, in order to know about their existence
            catch (FirebaseAuthWeakPasswordException e)
            {
                throw new Exception(e.Message);
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                throw new Exception(e.Message);
            }
            catch (FirebaseAuthUserCollisionException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception)
            {
                throw new Exception("An error occured, please try again!");
            }


        }
    }
}