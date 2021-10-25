using greenshare_app.Utils;
using greenshare_app.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace greenshare_app.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public RegisterViewModel(INavigation navigation, Page view)
        {
            Title = "Sign In";
            Email = string.Empty;
            Password = string.Empty;
            RepeatPassword = string.Empty;

            this.navigation = navigation;
            this.view = view;
        }

        private INavigation navigation;
        private Page view;

        private string nickname;
        private string email;
        private string password;
        private string repeatPassword;
        public AsyncCommand RegisterButtonCommand => new AsyncCommand(OnRegisterButton);

        public string Nickname
        {
            get => nickname;
            set => SetProperty(ref nickname, value);
        }    
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

        private async Task OnRegisterButton()
        {

            if (!Validation.PasswordsAreEqual(Password, RepeatPassword))
            {
                await view.DisplayAlert("Passwords are not the same!", "Please make sure both passwords are equal", "OK");
                RepeatPassword = string.Empty;
                return;
            }
            if (!Validation.ValidateEmail(Email))
            {   
                await view.DisplayAlert("Email not valid!", "Please check if the email is correct", "OK");
                return;
            }

            try
            {
                if (await Auth.Instance().Register(Email, Crypto.GetHashString(Password), Nickname))
                {
                    Application.Current.MainPage = new MainView();
                }

            }
            catch (Exception)
            {
                await view.DisplayAlert("Internal Server Error", "Something went wrong", "OK");
            }
        
        }

        private void OnGoogleClicked(object obj)
        {
            throw new NotImplementedException();
        }
    }
}