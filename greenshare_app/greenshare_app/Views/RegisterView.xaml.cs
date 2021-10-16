using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace greenshare_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterView : ContentPage
    {
        public RegisterView()
        {
            InitializeComponent();
        }
        private async void registerButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(email.Text) || string.IsNullOrEmpty(password.Text) || string.IsNullOrEmpty(passwordRepeat.Text))
                await DisplayAlert("Empty Values", "Please enter all values", "OK");
            else
            {
                if (password.Text == passwordRepeat.Text)
                {
                    await DisplayAlert("Sign in Success", "Sign in Success", "OK");
                }
                else await DisplayAlert("Sign in Failed", "Passwords are not the same!", "OK");
            }

        }

        private void googleButton_Clicked(object sender, EventArgs e)
        {
            //google sign up
        }
    }
}