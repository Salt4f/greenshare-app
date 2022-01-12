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
    public partial class SponsorsFormPage : ContentPage
    {
        public SponsorsFormPage()
        {
            InitializeComponent();
            BindingContext = new SponsorsFormViewModel(Navigation, this);
        }
        public SponsorsFormPage(Reward reward)
        {
            InitializeComponent();
            BindingContext = new SponsorsFormViewModel(Navigation, this,reward);
        }
    }
}