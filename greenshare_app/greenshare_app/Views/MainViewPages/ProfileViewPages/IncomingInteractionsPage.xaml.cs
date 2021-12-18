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
    public partial class IncomingInteractionsPage : ContentPage
    {
        public IncomingInteractionsPage()
        {
            InitializeComponent();
            BindingContext = new IncomingInteractionsViewModel(Navigation, this);
        }
    }
}