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
            photoBytesArray = new List<byte[]>();
            photos = new List<Image>();
            tags = new List<Tag>();
            minDate = DateTime.Now;
            terminationDateTime = DateTime.Now;
            PostTypes = new List<string> {
                nameof(Offer),
                nameof(Request)
            };
        }

        private INavigation navigation;
        private Page view;

        private string name;
        private string description;
        private string postType;

        private IList<Image> photos;
        private Image icon;
        private IList<Tag> tags;
        private DateTime terminationDateTime;
        private bool isVisible;
        private IList<byte[]> photoBytesArray;

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
        public IList<Tag> Tags
        {
            get => tags;
            set => SetProperty(ref tags, value);
        }

        private byte[] iconBytes;
        private DateTime minDate;
        private string newTag;

        private IList<string> postTypes;
        public IList<string> PostTypes
        {
            get => postTypes;
            private set => SetProperty(ref postTypes, value);
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
        public DateTime MinDate
        {
            get => minDate;
            set => SetProperty(ref minDate, value);
        }
        public bool IsVisible
        {
            get => isVisible;
            set => SetProperty(ref isVisible, value);
        }
        
        public string NewTag
        {
            get => newTag;
            set => SetProperty(ref newTag, value);
        }

        public AsyncCommand OnSubmitButtonCommand => new AsyncCommand(OnSubmit);
        public AsyncCommand OnAddTagButtonCommand => new AsyncCommand(OnAddTag);

        private async Task OnAddTag()
        {           
            //Tag no existe
            Random rnd = new Random();
            byte[] colors = new byte[3];
            rnd.NextBytes(colors);
            Color tagColor = Color.FromHex(Convert.ToBase64String(colors));
            Tags.Add(new Tag { Color = tagColor, Name = NewTag });

        }

        public AsyncCommand OnAddPhotoButtonCommand => new AsyncCommand(OnAddPhotoButton);
        public AsyncCommand OnAddIconButtonCommand => new AsyncCommand(OnAddIconButton);
        private async Task OnSubmit()
        {
            if (Name.Length == 0)
            {
                await view.DisplayAlert("Name field not filled", "Please enter a name", "OK");
                return;
            }
            if (Description.Length == 0)
            {
                await view.DisplayAlert("Description field not filled", "Please enter a description", "OK");
                return;
            }           
            switch (PostType)
            {
                case nameof(Offer):
                    if (Icon == null)
                    {
                        await view.DisplayAlert("Icon not found", "Please enter an icon", "OK");
                        return;
                    }                  
                    await PostSender.Instance().PostOffer(Name, Description, TerminationDateTime, await Geolocation.GetLastKnownLocationAsync(), Tags, photoBytesArray, iconBytes);
                    break;
                case nameof(Request):
                    await PostSender.Instance().PostRequest(Name, Description, TerminationDateTime, await Geolocation.GetLastKnownLocationAsync(), Tags);
                    break;
                default:
                    break;
            }
            await view.DisplayAlert("Offer Created", "", "ok");
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
      
        public async Task<bool> OnAddPhotoButton()
        {
            var photo = await MediaPicker.PickPhotoAsync();

            if (photo is null) return false;

            var photoStream = await photo.OpenReadAsync();

            byte[] photoBytes = new byte[photoStream.Length];
            await photoStream.ReadAsync(photoBytes, 0, (int)photoStream.Length);
            Image photoImage = new Image() { Source=ImageSource.FromStream(() => { return new MemoryStream(photoBytes); }) };          
            photoBytesArray.Add(photoBytes);
            Photos.Add(photoImage);
            return true;
        }

        public async Task<bool> OnAddIconButton()
        {
            var photo = await MediaPicker.PickPhotoAsync();

            if (photo is null) return false;

            var photoStream = await photo.OpenReadAsync();

            byte[] photoBytes = new byte[photoStream.Length];
            await photoStream.ReadAsync(photoBytes, 0, (int)photoStream.Length);
            Image photoImage = new Image() { Source = ImageSource.FromStream(() => { return new MemoryStream(photoBytes); }) };
            iconBytes = photoBytes;
            Icon = photoImage;
            return true;
        }
    }
}
