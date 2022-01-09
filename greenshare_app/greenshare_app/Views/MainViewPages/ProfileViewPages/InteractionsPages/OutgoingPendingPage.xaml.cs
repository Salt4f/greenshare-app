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
    public partial class OutgoingPendingPage : ContentPage
    {
        public OutgoingPendingPage()
        {
            InitializeComponent();
            BindingContext = new OutgoingPendingViewModel(this.Navigation, this);
        }
    }
}