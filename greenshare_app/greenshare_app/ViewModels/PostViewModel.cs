﻿using greenshare_app.Models;
using greenshare_app.Utils;
using greenshare_app.Views.MainViewPages;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using greenshare_app.Text;

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
        public AsyncCommand OnDeactivateButtonCommand => new AsyncCommand(OnDeactivate);
        public AsyncCommand OnReportButtonCommand => new AsyncCommand(OnReport);
        public AsyncCommand OnRequestToOfferButtonCommand => new AsyncCommand(OnRequestToOffer);
        public AsyncCommand OnOfferToRequestButtonCommand => new AsyncCommand(OnOfferToRequest);

        private event EventHandler Starting = delegate { };
        private IList<Image> photos;
        private Image icon;
        private IEnumerable<Tag> tags;
        private string terminationDateTime;
        private bool isVisible;
        
        public PostViewModel(INavigation navigation, Page view, Post post)
        {
            Title = Text.Text.ViewPost;
            //Options = Array.Empty;
            this.post = post;
            this.navigation = navigation;
            this.view = view;
            TerminationDateTime = post.TerminateAt.ToShortDateString();
            Name = post.Name;
            var type = post.GetType();
            
            if (type == typeof(Offer))
            {
                PostType = "Offer";
            }
            else if (type == typeof(Request))
            {
                PostType = "Request";
            }
            else
            {
                PostType = "";
            }
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
            Starting += OnStart;
            Starting(this, EventArgs.Empty);
            
        }
        private async void OnStart(object sender, EventArgs args)
        {
            
            IsBusy = true;
            var session = await Auth.Instance().GetAuth();
            if (session.Item1 == Config.Config.Instance().AdminId)
            {
                IsEditButtonVisible = false;
                IsRequestButtonVisible = false;
                IsOfferButtonVisible = false;
                IsDeactivateButtonVisible = true;
                IsEditButtonVisible = false;
            }
            else if (session.Item1 != post.OwnerId)
            {
                IsEditButtonVisible = false;
                if (PostType == "Offer")
                {
                    IsRequestButtonVisible = true;
                    IsOfferButtonVisible = false;
                }
                else
                {
                    IsRequestButtonVisible = false;
                    IsOfferButtonVisible = true;
                }
                IsReportButtonVisible = true;
            }
            else
            {
                IsEditButtonVisible = true;
                DeactivateButtons();
            }
            IsBusy = false;
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
        private bool isEditButtonVisible;
        private bool isReportButtonVisible;
        private bool isOfferButtonVisible;
        private bool isRequestButtonVisible;
        private bool isDeactivateButtonVisible;

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
        public string TerminationDateTime
        {
            get => terminationDateTime;
            set => SetProperty(ref terminationDateTime, value);
        }        
        public bool IsVisible
        {
            get => isVisible;
            set => SetProperty(ref isVisible, value);
        }

        public bool IsEditButtonVisible
        {
            get => isEditButtonVisible;
            set => SetProperty(ref isEditButtonVisible, value);
        }

        public bool IsReportButtonVisible
        {
            get => isReportButtonVisible;
            set => SetProperty(ref isReportButtonVisible, value);
        }
        public bool IsRequestButtonVisible
        {
            get => isRequestButtonVisible;
            set => SetProperty(ref isRequestButtonVisible, value);
        }
        public bool IsDeactivateButtonVisible
        {
            get => isDeactivateButtonVisible;
            set => SetProperty(ref isDeactivateButtonVisible, value);
        }
        public bool IsOfferButtonVisible
        {
            get => isOfferButtonVisible;
            set => SetProperty(ref isOfferButtonVisible, value);
        }

        private async Task OnEdit()
        {
            IsBusy = true;
            var view = new EditPost(post);
            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            view.Disappearing += OnDisappear;
            await navigation.PushModalAsync(view); 
            IsBusy = false;
        }
        private async Task OnDeactivate()
        {
            IsBusy = true;
            if (await PostSender.Instance().DeactivatePost(post.Id, PostType))
            {
                IsBusy = false;

                await view.DisplayAlert(Text.Text.PostDeactivatedSuccesfully, Text.Text.NowPeopleCantSeeYourPost, "OK");
                await navigation.PopModalAsync();

            }
            IsBusy = false;
        }        
        private void DeactivateButtons()
        {
            IsReportButtonVisible = false;
            IsRequestButtonVisible = false;
            IsOfferButtonVisible = false;
        }
        private async void OnDisappear(object sender, EventArgs e)
        {
            await navigation.PopModalAsync();
        }
        private async Task OnReport()
        {
            DeactivateButtons();
            IsBusy = true;
            var view = new ReportPage(typeof(Post),post.Id);
            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            view.Disappearing += OnDisappear;
            await navigation.PushModalAsync(view);
            IsBusy = false;
        }
        private async Task OnRequestToOffer()
        {
            IsBusy = true;
            int id = await PostSender.Instance().PostRequest(post.Name, "Reply to " + post.Name, post.TerminateAt, new Location(), post.Tags);
            if (id != -1)
            {
                if (await OfferRequestInteraction.Instance().RequestAnOffer(post.Id, id))
                {
                    IsBusy = false;
                    await view.DisplayAlert(Text.Text.OfferRequestedSuccessfully, Text.Text.PleaseCheckYourOutgoingInteractionsToSeeItsStatus, "OK");
                    await navigation.PopModalAsync();
                }
            }
            IsBusy = false;
        }
        private async Task OnOfferToRequest()
        {
            IsBusy = true;

            FileResult photo = null;
            while (photo is null)
            {
                photo = await MediaPicker.PickPhotoAsync();
            }
            var photoStream = await photo.OpenReadAsync();
            byte[] icon = new byte[photoStream.Length];
            await photoStream.ReadAsync(icon, 0, (int)photoStream.Length);
            int id = await PostSender.Instance().PostOffer(post.Name, "Reply to " + post.Name, post.TerminateAt, new Location(), post.Tags, new List<byte[]>(), icon);
            if (id != -1)
            {
                if (await OfferRequestInteraction.Instance().OfferARequest(id, post.Id))
                {
                    IsBusy = false;
                    await view.DisplayAlert(Text.Text.OfferRequestedSuccessfully, Text.Text.PleaseCheckYourOutgoingInteractionsToSeeItsStatus, "OK");
                    await navigation.PopModalAsync();
                }
            }
            IsBusy = false;
        }

        public string PostType
        {
            get => postType;
            set
            {
                switch (value)
                {
                    case "Offer":
                        IsVisible = true;                        
                        break;
                    case "Request":
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
