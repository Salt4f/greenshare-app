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

namespace greenshare_app.ViewModels
{
    internal class MyPostsViewModel : BaseViewModel
    {
        private INavigation navigation;
        private Page view;
        private ObservableRangeCollection<PostStatus> myPosts;

        private event EventHandler Starting = delegate { };
        public MyPostsViewModel(INavigation navigation, Page view)
        {
            Title = "My Posts";
            this.navigation = navigation;
            this.view = view;

            Starting += OnStart;
            Starting(this, EventArgs.Empty);

        }

        private async void OnStart(object sender, EventArgs args)
        {
            await view.DisplayAlert("WIP", "Sorry not sorry", "Okay");
            try
            {
                IsBusy = true;
                var loc = await Geolocation.GetLocationAsync();
                var ownerId = (await Auth.Instance().GetAuth()).Item1;
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


        public ObservableRangeCollection<PostStatus> MyPosts
        {
            get => myPosts;
            set => SetProperty(ref myPosts, value);
        }
    }
}
