using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StockMarketSimulator.Helpers
{
    // Used for validating emails
    class EmailEntryBehavior : Behavior<StackLayout>
    {
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

            // Verifying if the email is of the format "name@domain.something"
            string text = !string.IsNullOrEmpty(entry.Text) ? entry.Text : "";
            if (text.Contains("@"))
            {
                string[] email = text?.Split('@');
                if (!string.IsNullOrEmpty(email[0]) && !string.IsNullOrEmpty(email[1]))
                {
                    if (email[1].Contains("."))
                    {
                        domain = email[1].Split('.');
                        if (!string.IsNullOrEmpty(domain[0]) && !string.IsNullOrEmpty(domain[1]))
                            label.IsVisible = false;
                    }
                }

            }
        }
    }

    // Use for validating passwords
    class PasswordEntryBehavior : Behavior<StackLayout>
    {
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

                label.IsVisible = false;
            else
            {
                // Invalid password message format 
                label.Text = Resources.Language.InvalidPassword;
                label.FontSize = 16;
                label.TextColor = Color.Red;
                label.IsVisible = true;
            }
        }
    }
}
