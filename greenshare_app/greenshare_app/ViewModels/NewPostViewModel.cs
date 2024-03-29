﻿using greenshare_app.Models;
using greenshare_app.Utils;
using greenshare_app.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using greenshare_app.Text;
using Command = MvvmHelpers.Commands.Command;
using System.IO;
using System.Threading;
using greenshare_app.Views.MainViewPages;

namespace greenshare_app.ViewModels
{
    class NewPostViewModel : BaseViewModel
    {
        public NewPostViewModel(INavigation navigation, Page view)
        {
            Title = Text.Text.NewPost;
            //Options = Array.Empty;

            this.navigation = navigation;
            this.view = view;
            NewTag = "";
            tagNames = new List<string>();
            photoBytesArray = new List<byte[]>();           
            Photos = new ObservableRangeCollection<Image>();
            Tags = new ObservableRangeCollection<Tag>();
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

        private ObservableRangeCollection<Image> photos;
        private Image icon;
        private ObservableRangeCollection<Tag> tags;
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
        public ObservableRangeCollection<Tag> Tags
        {
            get => tags;
            set => SetProperty(ref tags, value);
        }

        private byte[] iconBytes;
        //Used to check repeated tags
        private IList<string> tagNames;
        private DateTime minDate;
        private string newTag;

        private IList<string> postTypes;
        private Location location;

        public IList<string> PostTypes
        {
            get => postTypes;
            private set => SetProperty(ref postTypes, value);
        }
        public Location Location
        {
            get => location;
            private set => SetProperty(ref location, value);
        }
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
        
        public string NewTag
        {
            get => newTag;
            set => SetProperty(ref newTag, value);
        }

        public AsyncCommand OnSubmitButtonCommand => new AsyncCommand(OnSubmit);
        public AsyncCommand OnAddTagButtonCommand => new AsyncCommand(OnAddTag);
        public AsyncCommand OnAddLocationButtonCommand => new AsyncCommand(OnAddLocationButton);

        private async Task OnAddTag()
        {           
            //Tag no existe
            Random rnd = new Random();
            byte[] colors = new byte[3];
            rnd.NextBytes(colors);
            Color tagColor = Color.FromHex(Convert.ToBase64String(colors));
            if (NewTag == "")
            {
                await view.DisplayAlert(Text.Text.ErrorAddingTag, Text.Text.PleaseEnterAName, "OK");
            }
            else if (!tagNames.Contains(NewTag))
            {
                Tags.Add(new Tag { Color = tagColor, Name = NewTag });
                tagNames.Add(NewTag);
            }           
            else
            {
                await view.DisplayAlert(Text.Text.ErrorAddingTag, Text.Text.TagsCannotBeDuplicated, "OK");
            }            
            NewTag = string.Empty;
        }

        public AsyncCommand OnAddPhotoButtonCommand => new AsyncCommand(OnAddPhotoButton);
        public AsyncCommand OnAddIconButtonCommand => new AsyncCommand(OnAddIconButton);
        private async Task OnSubmit()
        {
            IsBusy = true;
            if (Name.Length == 0)
            {
                await view.DisplayAlert(Text.Text.ErrorWhileCreatingPost, Text.Text.PleaseEnterAName, "OK");
                IsBusy = false;
                return;
            }
            if (Description.Length == 0)
            {
                await view.DisplayAlert(Text.Text.ErrorWhileCreatingPost, Text.Text.PleaseEnterADescription, "OK");
                IsBusy = false;
                return;
            }
            var loc = await Geolocation.GetLocationAsync();
            if (loc == null && Location == null)
            {
                IsBusy = false;
                await view.DisplayAlert(Text.Text.ErrorWhileCreatingPost, Text.Text.PleaseMakeSureLocationIsEnabled, "OK");
                return;
            }
            if (Location != null) loc = Location;
            if (Tags.Count == 0)
            {
                await view.DisplayAlert(Text.Text.ErrorWhileCreatingPost, Text.Text.PleaseEnterATagFirst, "OK");
                IsBusy = false;
                return;
            }
            int response;
            Post post;
            switch (PostType)
            {
                case nameof(Offer):
                    if (Icon == null)
                    {
                        await view.DisplayAlert(Text.Text.ErrorWhileCreatingPost, Text.Text.PleaseEnterAnIcon, "OK");
                        IsBusy = false;
                        return;
                    }                  
                    response = await PostSender.Instance().PostOffer(Name, Description, TerminationDateTime, loc, Tags, photoBytesArray, iconBytes);
                    post = await PostRetriever.Instance().GetOffer(response);
                    break;
                case nameof(Request):
                    response = await PostSender.Instance().PostRequest(Name, Description, TerminationDateTime, loc, Tags);
                    post = await PostRetriever.Instance().GetRequest(response);
                    break;
                default:
                    response = -1;
                    post = null;
                    break;
            }
            IsBusy = false;
            if (response != -1)
            {
                await view.DisplayAlert(Text.Text.PostCreated, Text.Text.NewPostName + ": " + Name, "ok");
                await navigation.PushModalAsync(new Views.MainViewPages.ViewPost(post));
            }
            else await view.DisplayAlert(Text.Text.ErrorWhileCreatingPost, Text.Text.PleaseMakeSureLoggedIn, "OK");
            ResetProperties();

        }

        private void ResetProperties()
        {
            Description = string.Empty;
            Name = string.Empty;
            photoBytesArray = new List<byte[]>();
            Photos = new ObservableRangeCollection<Image>();
            Tags = new ObservableRangeCollection<Tag>();
            Icon = new Image();
            TerminationDateTime = DateTime.Now;
            MinDate = DateTime.Now;
            NewTag = string.Empty;
            tagNames = new List<string>();

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
        private void OnDisappear(object sender, EventArgs args)
        {
            Tuple<bool, Location> location;
            location = PublicationMapViewModel.GetLocation();
            if (location.Item1) Location = location.Item2;
        }  
        public async Task OnAddLocationButton() 
        {
            var view = new PublicationMapPage();
            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            view.Disappearing += OnDisappear;
            await navigation.PushModalAsync(view);
            
        }

        public async Task<bool> OnAddPhotoButton()
        {
            var photo = await MediaPicker.PickPhotoAsync();

            if (photo is null) return false;

            var photoStream = await photo.OpenReadAsync();

            byte[] photoBytes = new byte[photoStream.Length];
            await photoStream.ReadAsync(photoBytes, 0, (int)photoStream.Length);
            Image photoImage = new Image() { Source = ImageSource.FromStream(() => { return new MemoryStream(photoBytes); }) };
            Photos.Add(photoImage);
            photoBytesArray.Add(photoBytes);
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
