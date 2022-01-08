using greenshare_app.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using greenshare_app.Utils;
using System.Text;
using greenshare_app.Views.MainViewPages;

namespace greenshare_app.ViewModels
{
    internal class MyPostsViewModel : BaseViewModel
    {
        private INavigation navigation;
        private Page view;
        private ObservableRangeCollection<PostStatus> myPosts;
        private PostStatus selectedPost;

        private event EventHandler Starting = delegate { };
        public MyPostsViewModel(INavigation navigation, Page view)
        {
            Title = "My Posts";
            this.navigation = navigation;
            this.view = view;
            RefreshCommand = new AsyncCommand(Refresh);
            SelectedCommand = new AsyncCommand<object>(Selected);
            Starting += OnStart;
            Starting(this, EventArgs.Empty);

        }
        public AsyncCommand<object> SelectedCommand { get; }
        public AsyncCommand RefreshCommand { get; }

        private async void OnStart(object sender, EventArgs args)
        {
            try
            {
                IsBusy = true;
                MyPosts.Clear();
                var offers = await PostRetriever.Instance().GetPostsByUserId("offers");
                if (offers != null) MyPosts.AddRange(offers);
                var requests = await PostRetriever.Instance().GetPostsByUserId("requests");
                if (requests != null) MyPosts.AddRange(requests);
                if (MyPosts.Count == 0) await view.DisplayAlert("No Posts Found", "Please create a post first", "OK");
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert("Error while retrieving your posts", "Please make sure location is enabled on your device", "OK");
            }
            IsBusy = false;
        }
        private async Task Refresh()
        {
            try
            {
                IsBusy = true;
                await navigation.PopToRootAsync();
                MyPosts.Clear();
                var offers = await PostRetriever.Instance().GetPostsByUserId("offers");
                if (offers != null) MyPosts.AddRange(offers);
                var requests = await PostRetriever.Instance().GetPostsByUserId("requests");
                if (requests != null) MyPosts.AddRange(requests);
                if (MyPosts.Count == 0) await view.DisplayAlert("No Posts Found", "Please create a post first", "OK");
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert("Error while retrieving your posts", "Please make sure location is enabled on your device", "OK");
            }
        }

        public PostStatus SelectedPost
        {
            get => selectedPost;
            set => SetProperty(ref selectedPost, value);
        }
        async Task Selected(object args)
        {
            var card = args as PostStatus;
            if (card == null)
                return;
            if (SelectedPost.IsOffer)
            {
                Offer offer = await PostRetriever.Instance().GetOffer(SelectedPost.Id);
                if (offer == null) await view.DisplayAlert("Error while retrieving Selected Offer", "Offer not found", "OK");
                else await navigation.PushModalAsync(new ViewPost(offer));
            }
            else
            {
                Request request = await PostRetriever.Instance().GetRequest(SelectedPost.Id);
                if (request == null) await view.DisplayAlert("Error while retrieving Selected Request", "Request not found", "OK");
                else await navigation.PushModalAsync(new ViewPost(request));
            }
            //await Application.Current.MainPage.DisplayAlert("Selected", coffee.Name, "OK");

        }
        public ObservableRangeCollection<PostStatus> MyPosts
        {
            get => myPosts;
            set => SetProperty(ref myPosts, value);
        }
    }
}
