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
    public partial class NewPublication : ContentPage
    {
        public NewPublication()
        {
            InitializeComponent();
            PickerOption.Items.Add("Request");
            PickerOption.Items.Add("Offer");
            PickerCategory.Items.Add("Home");
            PickerCategory.Items.Add("Videogames");
            PickerCategory.Items.Add("Fashion");
            PickerCategory.Items.Add("IT");
            PickerCategory.Items.Add("Pets");
            PickerCategory.Items.Add("Sport");

        }
    }
}