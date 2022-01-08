using greenshare_app.Models;
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
        public AsyncCommand OnDeactivateButtonCommand => new AsyncCommand(OnDeactivate);
        public AsyncCommand OnReportButtonCommand => new AsyncCommand(OnReport);
        public AsyncCommand OnRequestToOfferButtonCommand => new AsyncCommand(OnRequestToOffer);
        public AsyncCommand OnOfferToRequestButtonCommand => new AsyncCommand(OnOfferToRequest);


        private event EventHandler Starting = delegate { };
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
            this.TerminationDateTime = post.TerminateAt;
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
            if (session.Item1 != post.OwnerId)
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
        public bool IsOfferButtonVisible
        {
            get => isOfferButtonVisible;
            set => SetProperty(ref isOfferButtonVisible, value);
        }

        private async Task OnEdit()
        {            
            await navigation.PushModalAsync(new EditPost(post));            
        }
        private async Task OnDeactivate()
        {
            if (await PostSender.Instance().DeactivatePost(post.Id, PostType))
            {
                await view.DisplayAlert("Post deactivated successfully", "now people can't see your post", "OK");
            }
        }        
        private void DeactivateButtons()
        {
            IsReportButtonVisible = false;
            IsRequestButtonVisible = false;
            IsOfferButtonVisible = false;
        }
        private async Task OnReport()
        {
            DeactivateButtons();
            await navigation.PushModalAsync(new ReportPage(typeof(Post), post.Id));            
        }
        private async Task OnRequestToOffer()
        {
            List<Tag> tags = new List<Tag>();
            Tag tag = new Tag()
            {
                Name = "RequestToOffer",
                Color = Color.White,
            };
            tags.Add(tag);
            var id = await PostSender.Instance().PostRequest("Req-to-Offer-" + post.Id, "request to offer", post.TerminateAt, await Geolocation.GetLocationAsync(), tags);
            if (id != -1)
            {
                if (await OfferRequestInteraction.Instance().RequestAnOffer(post.Id, id))
                {
                    await view.DisplayAlert("Offer Requested successfully", "please check your Outgoing Interactions to see its Status", "OK");
                    DeactivateButtons();
                }
            }
        }
        private async Task OnOfferToRequest()
        {
            await view.DisplayAlert("Button WIP!", "missing way to create offer from here", "OK");
            //var id = await PostSender.Instance().PostOffer("Offer-to-Req-" + post.Id, "offer to request", post.TerminateAt, await Geolocation.GetLocationAsync(), new List<Tag>());
            //if (id != -1) await OfferRequestInteraction.Instance().OfferARequest(id, post.Id);
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
