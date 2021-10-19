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
            BindingContext = new ViewModels.LoginViewModel(Navigation);
        }
        
    }
}