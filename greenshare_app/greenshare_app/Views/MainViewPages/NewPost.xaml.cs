using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using greenshare_app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace greenshare_app.Views.MainViewPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPublication : ContentPage
    {

       // private bool pickerValue = false;
        public NewPublication()
        {
            InitializeComponent();
            BindingContext = new NewPostViewModel(Navigation, this);
        }

        private void selectedImage_Clicked(object sender, EventArgs e)
        {

        }
    }
}
