using greenshare_app.Models;
using greenshare_app.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace greenshare_app.Views.MainViewPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RewardsPage : ContentPage
    {
        public RewardsPage(User user)
        {
            InitializeComponent();
            BindingContext = new RewardsPageViewModel(Navigation, this, user);
        }
    }
}