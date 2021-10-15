using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace greenshare_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
        }
        private void loginButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(email.Text) || string.IsNullOrEmpty(password.Text))
                DisplayAlert("Empty Values", "Please enter Email and Password", "OK");
            else
            {

                if (email.Text == "abc@gmail.com" && password.Text == "1234")   //Verificar aqui les credencials
                {
                    DisplayAlert("Login Success", "", "Ok");

                    App.Current.MainPage = new Views.MainView();
                }
                else
                    DisplayAlert("Login Fail", "Please enter correct Email and Password", "OK");
            }
        }

        private void registerButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterView());
            Navigation.RemovePage(this);
        }

        private void googleButton_Clicked(object sender, EventArgs e)
        {
            //google sign in
        }
    }
}