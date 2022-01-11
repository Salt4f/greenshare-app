using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using greenshare_app.ViewModels;

namespace greenshare_app.Views.MainViewPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PublicationMapPage : ContentPage
    {
        public PublicationMapPage()
        {
            InitializeComponent();
            BindingContext = new PublicationMapViewModel(Navigation, this, PublicationMap);
        }
    }
}