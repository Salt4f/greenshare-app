using greenshare_app.Models;
using greenshare_app.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace greenshare_app.Views.MainViewPages.ProfileViewPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserInfoUpdatePage : ContentPage
    {
        public UserInfoUpdatePage(User user)
        {
            InitializeComponent();
            BindingContext = new UserInfoUpdateViewModel(Navigation, this, user);
        }
    }
}