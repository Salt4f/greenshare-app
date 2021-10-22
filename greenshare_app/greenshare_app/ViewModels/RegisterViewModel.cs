using MvvmHelpers;
using MvvmHelpers.Commands;
using System;

namespace greenshare_app.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public RegisterViewModel()
        {
            Title = "Sign In";
            this.email = string.Empty;
            this.password = string.Empty;
            this.repeatPassword = string.Empty;
        }
        public Command registerButtonClicked => new Command(OnRegisterClicked);
        public Command googleButtonClicked => new Command(OnGoogleClicked);

        

        private string email;
        private string password;
        private string repeatPassword;

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }
        public string RepeatPassword
        {
            get => repeatPassword;
            set => SetProperty(ref repeatPassword, value);
        }

        private void OnRegisterClicked(object obj)
        {
            throw new NotImplementedException();


            if (string.IsNullOrEmpty(this.email) || string.IsNullOrEmpty(this.password) || string.IsNullOrEmpty(this.repeatPassword)) ;
            //await DisplayAlert("Empty Values", "Please enter all values", "OK");
            else
            {
                if (this.password == this.repeatPassword)
                {
                    //await DisplayAlert("Sign in Success", "Sign in Success", "OK");
                }
                //else await DisplayAlert("Sign in Failed", "Passwords are not the same!", "OK");
            }
        }

        private void OnGoogleClicked(object obj)
        {
            throw new NotImplementedException();
        }
    }
}