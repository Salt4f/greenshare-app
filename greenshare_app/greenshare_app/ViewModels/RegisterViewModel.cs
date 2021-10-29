using greenshare_app.Utils;
using greenshare_app.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
        private string fullName;
        private string dni;
        private DateTime birthDate;

        public AsyncCommand RegisterButtonCommand => new AsyncCommand(OnRegisterButton);
        public AsyncCommand DniButtonCommand => new AsyncCommand(OnDniButton);
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
        public string FullName
        {
            get => fullName;
            set => SetProperty(ref fullName, value);
        }
        public DateTime BirthDate
        {
            get => birthDate;
            set => SetProperty(ref birthDate, value);
        }
        public string Dni
        {
            get => dni;
            set => SetProperty(ref dni, value);
        }
        private async Task OnRegisterButton()
        {
            if (Nickname.Length <= 5)
            {
                await view.DisplayAlert("Nickname too short!", "Please enter a longer nickname", "OK");                
                return;
            }
            if (Nickname.Length > 30)
            {
                await view.DisplayAlert("Nickname too long!", "Please enter a shorter nickname", "OK");
                return;
            }
            if (!Validation.ValidateDni(Dni))
            {
                await view.DisplayAlert("DNI not valid!", "Please enter a valid DNI", "OK");
                return;
            }
            if (string.IsNullOrEmpty(FullName))
            {
                await view.DisplayAlert("Full name not valid!", "Please enter a valid full name", "OK");
                return;
            }

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
                if (await Auth.Instance().Register(Email, Crypto.GetHashString(Password), Nickname, BirthDate, FullName, Dni))
                {
                    Application.Current.MainPage = new MainView();
                }

            }
            catch (Exception)
            {
                await view.DisplayAlert("Internal Server Error", "Something went wrong", "OK");
            }
        
        }
        private async Task OnDniButton()
        {
            var photo = await MediaPicker.CapturePhotoAsync();

            if (photo is null) return;

            System.Console.WriteLine(photo.FullPath);
        }
        private void OnGoogleClicked(object obj)
        {
            throw new NotImplementedException();
        }
    }
}