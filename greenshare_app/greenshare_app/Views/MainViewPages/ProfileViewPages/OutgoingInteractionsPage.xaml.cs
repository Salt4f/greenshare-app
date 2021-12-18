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
    public partial class OutgoingInteractionsPage : TabbedPage
    {
        public OutgoingInteractionsPage()
        {
            InitializeComponent();
            BindingContext = new OutgoingInteractionsViewModel(Navigation, this);
        }
    }
}