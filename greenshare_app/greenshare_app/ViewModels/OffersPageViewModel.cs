using greenshare_app.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using greenshare_app.Utils;
using greenshare_app.Views.MainViewPages;
using Xamarin.Essentials;
using System.Text;

namespace greenshare_app.ViewModels
{
    public class OffersPageViewModel : BaseViewModel
    {
        private ObservableRangeCollection<PostCard> postCardList;
        private PostCard selectedPostCard;
        private INavigation navigation;
        private Page view;

        public AsyncCommand<object> SelectedCommand { get; }
        public AsyncCommand RefreshCommand { get; }

        private event EventHandler Starting = delegate { };
        public OffersPageViewModel(INavigation navigation, Page view)
        {
            Title = "Ofertes";
            
            IsBusy = true;
            RefreshCommand = new AsyncCommand(Refresh);
            SelectedCommand = new AsyncCommand<object>(Selected);
            this.navigation = navigation;
            this.view = view;
            selectedPostCard = new PostCard();
            postCardList = new ObservableRangeCollection<PostCard>();

            Starting += OnStart;
            Starting(this, EventArgs.Empty);
        }

        private async void OnStart(object sender, EventArgs args)
        {
            try
            {
                IsBusy = true;
                var loc = await Geolocation.GetLastKnownLocationAsync();
                var cards = await PostRetriever.Instance().GetOffers(loc);
                PostCardList.AddRange(cards);
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert("No offers found in your surrounding area", "Refresh to check if there are any new offers around you", "OK");
            }
            IsBusy = false;
        }

        private async Task Refresh()
        {
            try
            {
                IsBusy = true;
                await navigation.PopToRootAsync();
                var loc = await Geolocation.GetLastKnownLocationAsync();
                var cards = await PostRetriever.Instance().GetOffers(loc/*, int.MaxValue*/);
                PostCardList.Clear();
                postCardList.AddRange(cards);
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert("No offers found in your surrounding area", "Refresh to check if there are any new offers around you", "OK");
            }
        }

        public ObservableRangeCollection<PostCard> PostCardList
        {
            get => postCardList;
            set => SetProperty(ref postCardList, value);
        }

        public PostCard SelectedPostCard
        {
            get => selectedPostCard;
            set {
                SetProperty(ref selectedPostCard, value);

            }
        }

        async Task Selected(object args)
        {
            var card = args as PostCard;
            if (card == null)
                return;

            Offer offer = await PostRetriever.Instance().GetOffer(SelectedPostCard.Id);
            if (offer == null) await view.DisplayAlert("Error while retrieving Selected Offer", "Offer not found", "OK");
            else await navigation.PushModalAsync(new ViewPost(offer));
            //await Application.Current.MainPage.DisplayAlert("Selected", coffee.Name, "OK");

        }







    }
}