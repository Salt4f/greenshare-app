using greenshare_app.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

using Xamarin.Forms;

namespace greenshare_app.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            this.Email = string.Empty;
            this.Password = string.Empty;
            //loginButtonClicked = new Command(this.OnLoginClicked);
        }

       

        //Binding Objects
        public Command ForgotPasswordCommand => new Command(OnForgotPassword);
        public Command loginButtonClicked => new Command(OnLoginClicked);
        public INavigation Navigation { get; set; }
        public Command registerButtonClicked => new Command(OnRegisterClicked);
        private string email;
        private string password;

        //public event PropertyChangedEventHandler PropertyChanged;

       
            
        
        void RefreshCanExecutes()
        {
            ((Command)loginButtonClicked).ChangeCanExecute();            
        }
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);           
        }
        public string Email
        {
            get { return email; }
            set {
                SetProperty(ref email, value);
            }
        }

        //Binding Actions
        private static void OnForgotPassword()
        {
            throw new NotImplementedException();
        }
        private void OnLoginClicked(object obj)
        {
            if (string.IsNullOrEmpty(this.email) || string.IsNullOrEmpty(this.password)) ;
            //DisplayAlert("Empty Values", "Please enter Email and Password", "OK");
            else
            {

                if (this.email == "abc@gmail.com" && this.password == "1234")   //Verificar aqui les credencials
                {
                    //await DisplayAlert("Login Success", "", "Ok");

                    App.Current.MainPage = new MainView();
                }
                else;
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
