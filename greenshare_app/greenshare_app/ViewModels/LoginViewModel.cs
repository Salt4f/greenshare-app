﻿using greenshare_app.Utils;
using greenshare_app.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Resources;
using greenshare_app.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace greenshare_app.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private event EventHandler Starting = delegate { };
        public LoginViewModel(INavigation navigation, Page view)
        {
            Email = string.Empty;
            Password = string.Empty;
            this.navigation = navigation;
            this.view = view;

            IsBusy = true;
            Starting += OnStart;
            Starting(this, EventArgs.Empty);
        }

        private async void OnStart(object sender, EventArgs args)
        {
            try 
            {
                if (await Auth.Instance().CheckLoggedIn()) Application.Current.MainPage = new MainView();
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert(Text.Text.InternalServerError, Text.Text.SomethingWentWrong, Text.Text.OK);
            }
            IsBusy = false;
        }

        private Page view;
        private INavigation navigation;

        //Binding Objects
        public Command ForgotPasswordCommand => new Command(OnForgotPassword);
        public AsyncCommand LoginButtonCommand => new AsyncCommand(OnLoginClicked);
        public AsyncCommand RegisterButtonCommand => new AsyncCommand(OnRegisterClicked);
        public AsyncCommand OnGoogleButtonCommand => new AsyncCommand(OnGoogleClicked);

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
                await view.DisplayAlert(Text.Text.WrongInfo, Text.Text.EmailPasswordEmpty, Text.Text.OK);
            }
            else
            {
                try
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
                        await view.DisplayAlert(Text.Text.WrongInfo, Text.Text.EmailPasswordIncorrect, Text.Text.OK);
                        Email = string.Empty;
                        Password = string.Empty;
                    }
                }
                catch (Exception e)
                {
                    var message = e.Message;
                    IsBusy = false;
                    await view.DisplayAlert(Text.Text.InternalServerError, Text.Text.SomethingWentWrong, Text.Text.OK);
                }
            }
            
        }
        private async Task OnRegisterClicked()
        {
            await navigation.PushModalAsync(new RegisterView());
            //throw new NotImplementedException();
        }

        private async Task OnGoogleClicked()
        {
            string token = Auth.Instance().GetGoogleLoginToken();
            string uri = Config.Config.Instance().BaseServerGoogleUrl + "/login?token=" + token;
            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            var res = Tuple.Create(false, false);
            while (!res.Item1)
            {
                res = await Auth.Instance().CheckGoogleLogin(token);
            }
            if (res.Item2)
            {
                Application.Current.MainPage = new QuizView();
                return;
            }
            else
            {
                Application.Current.MainPage = new MainView();
            }
        }
    }
}
