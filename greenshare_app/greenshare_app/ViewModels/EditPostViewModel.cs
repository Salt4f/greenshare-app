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
            this.TerminationDateTime = post.TerminateAt;
            tagNames = new List<string>();
            photoBytesArray = new List<byte[]>();
            Name = post.Name;
        
            Description = post.Description;
            TerminationDateTime = post.TerminateAt;
            tags = new ObservableRangeCollection<Tag>();
            tags.AddRange(post.Tags);
            minDate = DateTime.Now;
            if (post.GetType() == typeof(Offer))
            {
                IsVisible = true;
                Icon = new Image()
                {
                    Source = ImageSource.FromStream(() => { return new MemoryStream(((Offer)post).Icon); })
                };
                Photos = new ObservableRangeCollection<Image>();
                foreach (byte[] photo in ((Offer)post).Photos)
                {
                    photoBytesArray.Add(photo);
                    Image definitivePhoto = new Image
                    {
                        Source = ImageSource.FromStream(() => { return new MemoryStream(photo); })
                    };
                    Photos.Add(definitivePhoto);
                }
            }
            else IsVisible = false;

        }

        private INavigation navigation;
        private Page view;
        private Post post;
        private string name;
        private string description;

        private ObservableRangeCollection<Image> photos;
        private Image icon;
        private ObservableRangeCollection<Tag> tags;
        private DateTime terminationDateTime;
        private bool isVisible;
        private IList<byte[]> photoBytesArray;

        //private Array Options;
        public String NewTag
        {
            get => newTag;
            set => SetProperty(ref newTag, value);
        }
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
        public ObservableRangeCollection<Tag> Tags
        {
            get => tags;
            set => SetProperty(ref tags, value);
        }

        private byte[] iconBytes;
        private DateTime minDate;
        private Image selectedImage;
        private string newTag;
        private Tag selectedTag;
        private IList<string> tagNames;

        public Image Icon
        {
            get => icon;
            set => SetProperty(ref icon, value);
        }
        public ObservableRangeCollection<Image> Photos
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
        public Image SelectedImage
        {
            get => selectedImage;
            set => SetProperty(ref selectedImage, value);
        }

        public Tag SelectedTag
        {
            get => selectedTag;
            set => SetProperty(ref selectedTag, value);
        }
       
        public AsyncCommand OnSubmitButtonCommand => new AsyncCommand(OnSubmit);
        public AsyncCommand OnAddPhotoButtonCommand => new AsyncCommand(OnAddPhotoButton);
        public AsyncCommand OnRemovePhotoButtonCommand => new AsyncCommand(OnRemovePhotoButton);

        public AsyncCommand OnAddIconButtonCommand => new AsyncCommand(OnAddIconButton);
        public AsyncCommand<object> SelectedCommand => new AsyncCommand<object>(Selected);
        public AsyncCommand OnAddTagButtonCommand => new AsyncCommand(OnAddTag);

        public AsyncCommand OnRemoveTagButtonCommand => new AsyncCommand(OnRemoveTag);

        private async Task OnAddTag()
        {
            //Tag no existe
            Random rnd = new Random();
            byte[] colors = new byte[3];
            rnd.NextBytes(colors);
            Color tagColor = Color.FromHex(Convert.ToBase64String(colors));
            if (!tagNames.Contains(NewTag))
            {
                Tags.Add(new Tag { Color = tagColor, Name = NewTag });
                tagNames.Add(NewTag);
            }
            else
            {
                await view.DisplayAlert("Error while adding Tag", "Tags cannot be duplicated", "OK");
            }
            NewTag = string.Empty;
        }

        private async Task OnRemoveTag()
        {
            //Tag no existe
            if (SelectedTag != null)
            {
                string name = SelectedTag.Name;
                tagNames.Remove(name);
                Tags.Remove(SelectedTag);
                SelectedTag = null;
                await view.DisplayAlert("Tag deleted successfully", "", "OK");
            }
            else await view.DisplayAlert("Please select a Tag first", "", "OK");

        }
        private async Task OnSubmit()
        {
            if (Name.Length == 0)
            {
                await view.DisplayAlert("Error while editing Post", "Please enter a name", "OK");
                return;
            }
            if (Description.Length == 0)
            {
                await view.DisplayAlert("Error while editing Post", "Please enter a description", "OK");
                return;
            }
            bool response;
            IsBusy = true;
            if (post.GetType() == typeof(Offer))
            {
                if (Icon == null)
                {
                    await view.DisplayAlert("Error while editing Post", "Please enter an icon", "OK");
                    IsBusy = false;
                    return;
                }
                response = await PostSender.Instance().EditOffer(post.Id, Name, Description, TerminationDateTime, await Geolocation.GetLastKnownLocationAsync(), Tags, photoBytesArray, iconBytes);
            }
            else
            {
                response = await PostSender.Instance().EditRequest(post.Id, Name, Description, TerminationDateTime, await Geolocation.GetLastKnownLocationAsync(), Tags);
            }
            IsBusy = false;
            if (response) await view.DisplayAlert("Post Edited successfully", "", "OK");
            else await view.DisplayAlert("Error while editing Post", "Please make sure you are logged in", "OK");
            await navigation.PopModalAsync();
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
        private async Task Selected(object args)
        {
            var card = args as Image;
            if (card == null)
                return;
            SelectedImage = card;            
            //await Application.Current.MainPage.DisplayAlert("Selected", coffee.Name, "OK");

        }
        private async Task OnRemovePhotoButton()
        {
            if (SelectedImage != null)
            {
                int index = Photos.IndexOf(SelectedImage);
                Photos.RemoveAt(index);
                photoBytesArray.RemoveAt(index);
                SelectedImage = null;
                await view.DisplayAlert(" Photo deleted successfully", "", "OK");
            }
            else await view.DisplayAlert("Please select a photo first", "", "OK");

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
