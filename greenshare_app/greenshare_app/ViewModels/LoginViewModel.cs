using greenshare_app.Utils;
using greenshare_app.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace greenshare_app.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private event EventHandler Starting = delegate { };
        public LoginViewModel(INavigation navigation, Page view)
        {
            Navigation = navigation;
            Email = string.Empty;
            Password = string.Empty;
            this.view = view;

            IsBusy = true;
            Starting += OnStarting;
            Starting(this, EventArgs.Empty);
        }

        private async void OnStarting(object sender, EventArgs args)
        {
            if (await Auth.Instance().CheckLoggedIn()) Application.Current.MainPage = new MainView();
        }

        private Page view;

        //Binding Objects
        public Command ForgotPasswordCommand => new Command(OnForgotPassword);
        public AsyncCommand LoginButtonCommand => new AsyncCommand(OnLoginClicked);
        public Command RegisterButtonCommand => new Command(OnRegisterClicked);
        public INavigation Navigation { get; set; }
        
        private string email;
        private string password;
        private bool rememberMe;

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        public bool RememberMe
        {
            get => rememberMe;
            set => SetProperty(ref rememberMe, value);
        }

        //Binding Actions
        private static void OnForgotPassword()
        {
            throw new NotImplementedException();
        }

        private async Task OnLoginClicked()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await view.DisplayAlert("Wrong Info", "Email or password empty", "OK");
            }
            else
            {
                IsBusy = true;
                if (await Auth.Instance().Login(Email, Crypto.GetHashString(Password), RememberMe))   //Verificar aqui les credencials
                {
                    IsBusy = false;
                    Application.Current.MainPage = new MainView();
                }
                else
                {
                    IsBusy = false;
                    await view.DisplayAlert("Wrong Info", "Email and/or password incorrect", "OK");
                    Email = string.Empty;
                    Password = string.Empty;
                }
                
                    //DisplayAlert("Login Fail", "Please enter correct Email and Password", "OK");
            }
            //throw new NotImplementedException();
        }
        private async void OnRegisterClicked()
        {
            await Navigation.PushModalAsync(new RegisterView());
            //throw new NotImplementedException();
        }
    }
}
