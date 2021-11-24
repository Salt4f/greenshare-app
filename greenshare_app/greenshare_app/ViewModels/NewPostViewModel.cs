using greenshare_app.Models;
using greenshare_app.Utils;
using greenshare_app.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Command = MvvmHelpers.Commands.Command;
using System.IO;

namespace greenshare_app.ViewModels
{
    class NewPostViewModel : BaseViewModel
    {
        public NewPostViewModel(INavigation navigation, Page view)
        {
            Title = "New Post";
            //Options = Array.Empty;

            this.navigation = navigation;
            this.view = view;
            

        }

        private INavigation navigation;
        private Page view;

        private string name;
        private string description;
        private string postType;

        private IList<Image> photos;
        private Image icon;
        private IEnumerable<Tag> tags;
        private DateTime terminationDateTime;
        private bool isVisible;

        //private Array Options;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        public IEnumerable<Tag> Tags
        {
            get => tags;
            set => SetProperty(ref tags, value);
        }        
        public Image Icon
        {
            get => icon;
            set => SetProperty(ref icon, value);
        }
        public IList<Image> Photos
        {
            get => photos;
            set => SetProperty(ref photos, value);
        }
        public DateTime TerminationDateTime
        {
            get => terminationDateTime;
            set => SetProperty(ref terminationDateTime, value);
        }
        public bool IsVisible
        {
            get => isVisible;
            set => SetProperty(ref isVisible, value);
        }

        public AsyncCommand OnSubmitButtonCommand => new AsyncCommand(OnSubmit);

        private async Task OnSubmit()
        {
            await view.DisplayAlert("Not implemented yet", "Sorry not Sorry", "Yessir");
        }

        public string PostType
        {
            get => postType;
            set
            {
                switch (value)
                {
                    case nameof(Offer):
                        IsVisible = true;
                        break;
                    case nameof(Request):
                        IsVisible = false;
                        break;
                    default:
                        break;
                }
                SetProperty(ref postType, value); 
            }
        }
        
       

        
        /*  
          public EventHandler Picker_OnSelectedIndex(object sender, EventArgs e)
          {
              if (Index == 1) PickerValue = false;
              else PickerValue = true;

          }
        */

        /* WIP RAUL
        public async Task<bool> OnAddPhotoButton()
        {
            var photo = await MediaPicker.CapturePhotoAsync();

            if (photo is null) return false;


            Image photoImage = new Image() { Source=ImageSource.FromStream(() => { return new MemoryStream(photo); }) };
            photos.Add(photo);
        }
        */
    }
}
