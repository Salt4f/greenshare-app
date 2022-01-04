
using greenshare_app.Models;
using greenshare_app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace greenshare_app.Views.MainViewPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage(int? userId = null)
        {
            InitializeComponent();
            if (userId != null) BindingContext = new ProfilePageViewModel(Navigation, this, userId);
            else BindingContext = new ProfilePageViewModel(Navigation, this);
        }

    }
}