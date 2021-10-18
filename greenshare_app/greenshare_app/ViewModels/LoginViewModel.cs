using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Common.Mvvm.Base;
using Xamarin.Forms;

namespace greenshare_app.ViewModels
{
    public class LoginViewModel : BindableBase 
    {
        public LoginViewModel()
        {
            this.InitializeProperties();
        }

       

        //Binding Objects
        public Command ForgotPasswordCommand => new Command(OnForgotPassword);
        public ICommand loginButtonClicked { private get; set; }    
        public Command registerButtonClicked => new Command(OnRegisterClicked);
        private string email;
        private string password;

        //public event PropertyChangedEventHandler PropertyChanged;

        private void InitializeProperties()
        {
            this.Email = string.Empty;
            this.Password = string.Empty;
            loginButtonClicked = new Command(
                execute: () =>
                {
                    OnLoginClicked();
                    RefreshCanExecutes();
                });
        }
        void RefreshCanExecutes()
        {
            ((Command)loginButtonClicked).ChangeCanExecute();            
        }
        public string Password
        {
            get { return password; }
            set {
                SetProperty(ref password, value);
            }

            
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
        private void OnLoginClicked()
        {
            if (string.IsNullOrEmpty(this.email) || string.IsNullOrEmpty(this.password)) ;
            //DisplayAlert("Empty Values", "Please enter Email and Password", "OK");
            else
            {

                if (this.email == "abc@gmail.com" && this.password == "1234")   //Verificar aqui les credencials
                {
                    //DisplayAlert("Login Success", "", "Ok");

                    App.Current.MainPage = new Views.MainView();
                }
                else;
                    //DisplayAlert("Login Fail", "Please enter correct Email and Password", "OK");
            }
            //throw new NotImplementedException();
        }
        private static void OnRegisterClicked()
        {
            throw new NotImplementedException();
        }
    }
}
