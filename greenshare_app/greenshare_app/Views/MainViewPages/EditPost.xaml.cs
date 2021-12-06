using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using greenshare_app.ViewModels;
using greenshare_app.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace greenshare_app.Views.MainViewPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPost : ContentPage
    {
        public EditPost(Offer offer)
        {
            InitializeComponent();
            BindingContext = new EditPostViewModel(Navigation, this, offer);
        }

        private void selectedImage_Clicked(object sender, EventArgs e)
        {

        }
    }
}