using greenshare_app.Utils;
using greenshare_app.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using greenshare_app.Text;

namespace greenshare_app.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public RegisterViewModel(INavigation navigation, Page view)
        {
            Title = Text.Text.SignIn;
            Email = string.Empty;
            Password = string.Empty;
            RepeatPassword = string.Empty;
            birthDate = DateTime.Today;
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

        private bool dniPhotoValid;

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
            set
            {
                dniPhotoValid = false;
                SetProperty(ref dni, value);
            }
        }
        private async Task OnRegisterButton()
        {           
            if (Nickname.Length <= 5)
            {
                await view.DisplayAlert(Text.Text.NicknameTooShort, Text.Text.PleaseEnterALongerNickname, "OK");                
                return;
            }
            if (Nickname.Length > 30)
            {
                await view.DisplayAlert(Text.Text.NicknameTooLong, Text.Text.PleaseEnterAShorterNickname, "OK");
                return;
            }
            if (!Validation.ValidateDni(Dni))
            {
                await view.DisplayAlert(Text.Text.DNINotValid, Text.Text.PleaseEnterAValidDNI, "OK");
                return;
            }
            if (string.IsNullOrEmpty(FullName))
            {
                await view.DisplayAlert(Text.Text.FullNameNotValid, Text.Text.PleaseEnterAValidFullName, "OK");
                return;
            }

            if (!Validation.PasswordsAreEqual(Password, RepeatPassword))
            {
                await view.DisplayAlert(Text.Text.PasswordsAreNotTheSame, Text.Text.PleaseMakeSureBothPasswordsAreEqual, "OK");
                RepeatPassword = string.Empty;
                return;
            }
            if (!Validation.ValidateEmail(Email))
            {   
                await view.DisplayAlert(Text.Text.EmailNotValid, Text.Text.PleaseCheckIfTheEmailIsCorrect, "OK");
                return;
            }

            if (!dniPhotoValid)
            {
                await view.DisplayAlert(Text.Text.DNINotValid, Text.Text.PleaseTakeAPhotoOfYourDNI, "OK");
                return;
            }

            try
            {
                IsBusy = true;
                if (await Auth.Instance().Register(Email, Crypto.GetHashString(Password), Nickname, BirthDate, FullName, Dni))
                {
                    Application.Current.MainPage = new QuizView();
                    IsBusy = false;
                    return;
                }

            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert(Text.Text.InternalServerError, Text.Text.SomethingWentWrong, "OK");
            }
        
        }
        private async Task OnDniButton()
        {
            var photo = await MediaPicker.CapturePhotoAsync();

            if (photo is null) return;

            dniPhotoValid = true;
            await view.DisplayAlert(Text.Text.DNIVerified, Text.Text.YourDNIHasBeenVerified, "OK");

        }
       
    }
}