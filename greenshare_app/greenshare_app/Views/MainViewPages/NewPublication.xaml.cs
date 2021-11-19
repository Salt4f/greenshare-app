using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using greenshare_app.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;

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
            PickerOption.Items.Add("Request");
            PickerOption.Items.Add("Offer");
            PickerCategory.Items.Add("Home");
            PickerCategory.Items.Add("Videogames");
            PickerCategory.Items.Add("Fashion");
            PickerCategory.Items.Add("IT");
            PickerCategory.Items.Add("Pets");
            PickerCategory.Items.Add("Sport");
            BindingContext = new ViewModels.NewPublicationViewModel(Navigation, this);

        }
        
        /*private void Picker_OnSelectedIndex(object sender, System.EventArgs e)
        {
            if (PickerOption.SelectedIndex == 1) pickerValue = false;
            else pickerValue = true;
        }
        */
        

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            //! added using Plugin.Media;
            await CrossMedia.Current.Initialize();

            //// if you want to take a picture use this
            // if(!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
            /// if you want to select from the gallery use this
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Not supported", "Your device does not currently support this functionality", "Ok");
                return;
            }

            //! added using Plugin.Media.Abstractions;
            // if you want to take a picture use StoreCameraMediaOptions instead of PickMediaOptions
            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Small
            };
            // if you want to take a picture use TakePhotoAsync instead of PickPhotoAsync
            var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            if (selectedImage == null)
            {
                await DisplayAlert("Error", "Could not get the image, please try again.", "Ok");
                return;
            }

            selectedImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
        }

        private new Task DisplayAlert(string v1, string v2, string v3)
        {
            throw new NotImplementedException();
        }

    }
}
