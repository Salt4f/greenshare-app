using greenshare_app.Models;
using greenshare_app.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace greenshare_app.Views.MainViewPages.ProfileViewPages.InteractionsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatePage : ContentPage
    {
        public RatePage(AcceptedPostInteraction acceptedPost)
        {
            InitializeComponent();
            BindingContext = new RatePageViewModel(this.Navigation, this, acceptedPost);
        }
    }
}