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
            if (session.Item1 != post.OwnerId) IsEditButtonVisible = false;
            else IsEditButtonVisible = true;
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


        private async Task OnEdit()
        {            
            await navigation.PushModalAsync(new EditPost(post));            
        }
        private async Task OnDeactivate()
        {
            await view.DisplayAlert("Button WIP!", "espera bro", "OK");
        }
        private async Task OnReport()
        {
            await view.DisplayAlert("Button WIP!", "espera bro", "OK");
        }
        private async Task OnRequestToOffer()
        {
            await view.DisplayAlert("Button WIP!", "espera bro", "OK");
        }
        private async Task OnOfferToRequest()
        {
            await view.DisplayAlert("Button WIP!", "espera bro", "OK");
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
