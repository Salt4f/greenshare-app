﻿using greenshare_app.Models;
using greenshare_app.Utils;
using greenshare_app.Views.MainViewPages;
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
    public class PostViewModel : BaseViewModel
    {
        private Post post;
        private INavigation navigation;
        private Page view;

        private string name;
        private string description;
        private string postType;
        public AsyncCommand OnEditButtonCommand => new AsyncCommand(OnEdit);

        
        private IList<Image> photos;
        private Image icon;
        private IEnumerable<Tag> tags;
        private DateTime terminationDateTime;
        private bool isVisible;
        
        public PostViewModel(INavigation navigation, Page view, Post post)
        {
            Title = "View Post";
            //Options = Array.Empty;
            this.post = post;
            this.navigation = navigation;
            this.view = view;
            Name = post.Name;
            PostType = post.GetType().ToString();
            Description = post.Description;
            Tags = post.Tags;
            if (post.GetType() == typeof(Offer))
            {
                IsVisible = true;
                Icon = new Image()
                {
                    Source = ImageSource.FromStream(() => { return new MemoryStream(((Offer)post).Icon); })
                };
                Photos = new List<Image>();
                foreach (byte[] photo in ((Offer)post).Photos)
                {
                    Image definitivePhoto = new Image
                    {
                        Source = ImageSource.FromStream(() => { return new MemoryStream(photo); })
                    };
                    Photos.Add(definitivePhoto);
                }
            }
            else IsVisible = false;

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
        public IEnumerable<Tag> Tags
        {
            get => tags;
            set => SetProperty(ref tags, value);
        }

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
        public bool IsVisible
        {
            get => isVisible;
            set => SetProperty(ref isVisible, value);
        }

       
        private async Task OnEdit()
        {
            
            if (post.GetType() == typeof(Offer))await navigation.PushModalAsync(new EditOffer((Offer)post));
            //else await navigation.PushModalAsync(new EditRequest((Request)post));
            await view.DisplayAlert("Sorry not sorry", "WIP", "OK");
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
    }
}
