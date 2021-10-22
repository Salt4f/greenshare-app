using greenshare_app.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace greenshare_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterView : ContentPage
    {
        public RegisterView()
        {
            InitializeComponent();
            BindingContext = new RegisterViewModel(Navigation, this);
        }
               
    }
}