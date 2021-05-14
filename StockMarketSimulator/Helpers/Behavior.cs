using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StockMarketSimulator.ViewModel;
using Xamarin.Forms;

namespace StockMarketSimulator.Helpers
{
    // used for checking if name is valid
    class NameEntryBehavior : Behavior<StackLayout>
    {

        // property used to communicate with RegisterViewModel to check if user input is valid and send the request to the server
        public static bool isNameValid = false;

        /*
         * Method that will be executed when the behavior is attached to the view
         * @stackLayout
         *  The StackLayout that contains the Entry and Label used for validation
         */
        protected async override void OnAttachedTo(StackLayout stackLayout)
        {
            await Task.Delay(500);
            ((Entry)stackLayout.Children[0]).Unfocused += OnEntryTextChanged;
            base.OnAttachedTo(stackLayout);
        }

        /*
         * Method that will be executed when the behavior is detached to the view
         * @stackLayout
         *  The StackLayout that contains the Entry and Label used for validation
         */
        protected override void OnDetachingFrom(StackLayout stackLayout)
        {
            ((Entry)stackLayout.Children[0]).Unfocused -= OnEntryTextChanged;
            base.OnDetachingFrom(stackLayout);
        }

        /*
         * Event Handler for unfocused event of the Entry
         * @sender
         *  The object which fires the event
         * @args
         *  Event arguments
         */
        void OnEntryTextChanged(object sender, FocusEventArgs args)
        {
            // Getting the Parent Stack in order to acces the Label which will hold the message
            StackLayout stackLayout = (StackLayout)((Entry)sender).Parent;
            Entry entry = ((Entry)stackLayout.Children[0]);
            Label label = (Label)(stackLayout.Children[1]);
            string text = !string.IsNullOrEmpty(entry.Text) ? entry.Text.Trim() : "" ;
            if (!string.IsNullOrEmpty(text))
            {
                isNameValid = true;
                label.IsVisible = false;
            }
            else
            { 
                // Invalid name message format
                label.Text = Resources.Language.InvalidName;
                label.FontSize = 16;
                label.TextColor = Color.Red;
                label.IsVisible = true;
                isNameValid = false;
            }
            
            

        }
    }
    // Used for validating emails
    class EmailEntryBehavior : Behavior<StackLayout>
    {

        // property used to communicate with RegisterViewModel to check if user input is valid and send the request to the server
        public static bool isEmailValid = false;

        /*
         * Method that will be executed when the behavior is attached to the view
         * @stackLayout
         *  The StackLayout that contains the Entry and Label used for validation
         */
        protected async override void OnAttachedTo(StackLayout stackLayout)
        {
            await Task.Delay(500);
            ((Entry)stackLayout.Children[0]).Unfocused += OnEntryTextChanged;
            base.OnAttachedTo(stackLayout);
        }

        /*
         * Method that will be executed when the behavior is detached to the view
         * @stackLayout
         *  The StackLayout that contains the Entry and Label used for validation
         */
        protected override void OnDetachingFrom(StackLayout stackLayout)
        {
            ((Entry)stackLayout.Children[0]).Unfocused -= OnEntryTextChanged;
            base.OnDetachingFrom(stackLayout);
        }

        /*
         * Event Handler for unfocused event of the Entry
         * @sender
         *  The object which fires the event
         * @args
         *  Event arguments
         */
        void OnEntryTextChanged(object sender, FocusEventArgs args)
        {
            // Getting the Parent Stack in order to acces the Label which will hold the message
            StackLayout stackLayout = (StackLayout)((Entry)sender).Parent;
            Entry entry = ((Entry)stackLayout.Children[0]);
            Label label = (Label)(stackLayout.Children[1]);
            string[] domain = null;

            // Invalid email message format
            label.Text = Resources.Language.InvalidEmail;
            label.FontSize = 16;
            label.TextColor = Color.Red;
            label.IsVisible = true;
            isEmailValid = false;

            // Verifying if the email is of the format "name@domain.something"
            string text = !string.IsNullOrEmpty(entry.Text) ? entry.Text : "";
            text = text.Trim();
            if (text.Contains("@"))
            {
                string[] email = text?.Split('@');
                if (!string.IsNullOrEmpty(email[0]) && !string.IsNullOrEmpty(email[1]))
                {
                    if (email[1].Contains("."))
                    {
                        domain = email[1].Split('.');
                        if (!string.IsNullOrEmpty(domain[0]) && !string.IsNullOrEmpty(domain[1]))
                        {
                            label.IsVisible = false;
                            isEmailValid = true;
                        }
                    }
                }

            }
        }
    }

    // Used for validating passwords
    class PasswordEntryBehavior : Behavior<StackLayout>
    {

        // property used to communicate with RegisterViewModel to check if user input is valid and send the request to the server
        public static bool isPasswordValid = false;

        /*
         * Method that will be executed when the behavior is attached to the view
         * @stackLayout
         *  The StackLayout that contains the Entry and Label used for validation
         */
        protected async override void OnAttachedTo(StackLayout stackLayout)
        {
            await Task.Delay(500);
            ((Entry)stackLayout.Children[0]).Unfocused += OnEntryTextChanged;
            base.OnAttachedTo(stackLayout);
        }

        /*
         * Method that will be executed when the behavior is attached to the view
         * @stackLayout
         *  The StackLayout that contains the Entry and Label used for validation
         */
        protected override void OnDetachingFrom(StackLayout stackLayout)
        {
            ((Entry)stackLayout.Children[0]).Unfocused -= OnEntryTextChanged;
            base.OnDetachingFrom(stackLayout);
        }

        /*
         * Event Handler for unfocused event of the Entry
         * @sender
         *  The object which fires the event
         * @args
         *  Event arguments
         */
        void OnEntryTextChanged(object sender, FocusEventArgs args)
        {
            // Getting the Parent Stack in order to acces the Label will hold the message
            StackLayout stackLayout = (StackLayout)((Entry)sender).Parent;
            Entry entry = ((Entry)stackLayout.Children[0]);
            Label label = (Label)(stackLayout.Children[1]);
            string text = !string.IsNullOrEmpty(entry.Text) ? entry.Text : "";
           

            // Password must be at least 8 characters long and contain at least one lower case letter, one upper case letter and one number
            if (Regex.IsMatch(text, @"[a-z]") && Regex.IsMatch(text, @"[A-Z]") && Regex.IsMatch(text, @"[0-9]") && text.Length >= 8)
            {
                label.IsVisible = false;
                isPasswordValid = true;
            }
            else
            {
                // Invalid password message format 
                label.Text = Resources.Language.InvalidPassword;
                label.FontSize = 16;
                label.TextColor = Color.Red;
                label.IsVisible = true;
                isPasswordValid = false;
            }
        }
    }

        // Used for validating confirmPassword
        class ConfirmPasswordEntryBehavior : Behavior<StackLayout>
        {

        // property used to communicate with RegisterViewModel to check if user input is valid and send the request to the server
        public static bool isConfirmPasswordValid = false;

        /*
         * Method that will be executed when the behavior is attached to the view
         * @stackLayout
         *  The StackLayout that contains the Entry and Label used for validation
         */
        protected async override void OnAttachedTo(StackLayout stackLayout)
            {
                await Task.Delay(500);
                ((Entry)stackLayout.Children[0]).Unfocused += OnEntryTextChanged;
                base.OnAttachedTo(stackLayout);
            }

            /*
             * Method that will be executed when the behavior is attached to the view
             * @stackLayout
             *  The StackLayout that contains the Entry and Label used for validation
             */
            protected override void OnDetachingFrom(StackLayout stackLayout)
            {
                ((Entry)stackLayout.Children[0]).Unfocused -= OnEntryTextChanged;
                base.OnDetachingFrom(stackLayout);
            }

            /*
             * Event Handler for unfocused event of the Entry
             * @sender
             *  The object which fires the event
             * @args
             *  Event arguments
             */
            void OnEntryTextChanged(object sender, FocusEventArgs args)
            {
                // Getting the Parent Stack in order to acces the Label will hold the message
                StackLayout stackLayout = (StackLayout)((Entry)sender).Parent;
                Entry entry = (Entry)stackLayout.Children[0];
                Label label = (Label)stackLayout.Children[1];
                string text = !string.IsNullOrEmpty(entry.Text) ? entry.Text : "";

                // Parent of stack layout to get the text from password
                StackLayout parentOfstackLayout = (StackLayout)stackLayout.Parent;
                StackLayout passwordStackLayout = (StackLayout)parentOfstackLayout.Children[2];
                Entry passwordEntry = (Entry)passwordStackLayout.Children[0];
                string textPasswordEntry = !string.IsNullOrEmpty(passwordEntry.Text) ? passwordEntry.Text : "";


            // Password must be at least 8 characters long and contain at least one lower case letter, one upper case letter and one number
            if (text.Equals(textPasswordEntry))
                {
                    label.IsVisible = false;
                isConfirmPasswordValid = true;
                }
                else
                {
                    // Invalid password message format 
                    label.Text = Resources.Language.InvalidConfirmPassword;
                    label.FontSize = 16;
                    label.TextColor = Color.Red;
                    label.IsVisible = true;
                isConfirmPasswordValid = false;
                  
                }
            }
        }
}
