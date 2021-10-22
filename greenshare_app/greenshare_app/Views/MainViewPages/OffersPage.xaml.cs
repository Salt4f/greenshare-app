using greenshare_app.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace greenshare_app.Views.MainViewPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OffersPage : ContentPage
    {
        public OffersPage()
        {
            InitializeComponent();
            BindingContext = new OffersPageViewModel();
        }
        private void OnPerfilClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ProfilePage());
        }
    }
}