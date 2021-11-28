using greenshare_app.Models;
using greenshare_app.Utils;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace greenshare_app.ViewModels
{
    public class EditPostViewModel : BaseViewModel
    {
        public EditPostViewModel(INavigation navigation, Page view, Post post)
        {
            Title = "New Post";
            //Options = Array.Empty;

            this.navigation = navigation;
            this.view = view;
            this.post = post;
            photoBytesArray = new List<byte[]>();
            Name = post.Name;
            Description = post.Description;
            TerminationDateTime = post.TerminateAt;
            tags = post.Tags;
            
            minDate = DateTime.Now;
            if (post.GetType() == typeof(Offer))
            {
                IsVisible = true;
                Icon = ((Offer) post).Icon;
                Photos = ((Offer) post).Photos;
            }

        }

        private INavigation navigation;
        private Page view;
        private Post post;
        private string name;
        private string description;

        private IList<Image> photos;
        private Image icon;
        private IEnumerable<Tag> tags;
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
        public IEnumerable<Tag> Tags
        {
            get => tags;
            set => SetProperty(ref tags, value);
        }

        private byte[] iconBytes;
        private DateTime minDate;

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

        public AsyncCommand OnSubmitButtonCommand => new AsyncCommand(OnSubmit);
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
            if (post.GetType() == typeof(Offer))
            {
                if (Icon == null)
                {
                    await view.DisplayAlert("Icon not found", "Please enter an icon", "OK");
                    return;
                }
                await PostSender.Instance().EditOffer(post.Id, Name, Description, TerminationDateTime, await Geolocation.GetLastKnownLocationAsync(), Tags, photoBytesArray, iconBytes);
            }
            else
            {
                await PostSender.Instance().EditRequest(post.Id, Name, Description, TerminationDateTime, await Geolocation.GetLastKnownLocationAsync(), Tags);
            }           
        }

        public async Task<bool> OnAddPhotoButton()
        {
            var photo = await MediaPicker.PickPhotoAsync();

            if (photo is null) return false;

            var photoStream = await photo.OpenReadAsync();

            byte[] photoBytes = new byte[photoStream.Length];
            await photoStream.ReadAsync(photoBytes, 0, (int)photoStream.Length);
            Image photoImage = new Image() { Source = ImageSource.FromStream(() => { return new MemoryStream(photoBytes); }) };
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
