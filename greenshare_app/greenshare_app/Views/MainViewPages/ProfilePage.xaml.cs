
using greenshare_app.Models;
using greenshare_app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace greenshare_app.Views.MainViewPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = new ProfilePageViewModel(Navigation, this);
        }
        public ProfilePage(int userId)
        {
            InitializeComponent();
            BindingContext = new ProfilePageViewModel(Navigation, this, userId);           
        }

    }
}