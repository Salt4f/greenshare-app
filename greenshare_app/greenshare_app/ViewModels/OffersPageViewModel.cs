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
using System.Text;

namespace greenshare_app.ViewModels
{
    public class OffersPageViewModel : BaseViewModel
    {
        private ObservableRangeCollection<PostCard> postCardList;
        private PostCard selectedPostCard;
        private string searchWord;
        private bool filterVisible;
        private int distanceValue;
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
            this.DistanceValue = 100;
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
                var loc = await Geolocation.GetLocationAsync();
                var cards = await PostRetriever.Instance().GetOffers(loc);
                PostCardList.AddRange(cards);
                if (PostCardList.Count == 0) await view.DisplayAlert("No offers found", "please change your location and refresh", "OK");
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert("Error while retrieving offers", "Please make sure location is enabled on your device", "OK");
            }
            IsBusy = false;
        }

        private async Task Refresh()
        {
            try
            {
                IsBusy = true;
                await navigation.PopToRootAsync();
                var loc = await Geolocation.GetLocationAsync();
                var cards = await PostRetriever.Instance().GetOffers(loc/*, int.MaxValue*/);
                PostCardList.Clear();
                PostCardList.AddRange(cards);
                if (PostCardList.Count == 0) await view.DisplayAlert("No offers found", "please change your location and refresh", "OK");
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert("Error while retrieving offers", "Please make sure location is enabled on your device", "OK");
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


        public String SearchWord
        {
            get => searchWord;
            set => SetProperty(ref searchWord, value);
        }
        public Boolean FilterVisible
        {
            get => filterVisible;
            set => SetProperty(ref filterVisible, value);
        }
        public int DistanceValue
        {
            get => distanceValue;
            set => SetProperty(ref distanceValue, value);
        }

        public AsyncCommand OnSearchButtonCommand => new AsyncCommand(OnSearch);
        public AsyncCommand OnFilterButtonCommand => new AsyncCommand(OnFilter);

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

        private async Task OnSearch()
        {
            //Esto filtra por tag y por distanceValue
            IsBusy = true;
            var loc = await Geolocation.GetLocationAsync();
            IEnumerable<PostCard> cards = new List<PostCard>();
            if (SearchWord != null)
            {                
                cards = await PostRetriever.Instance().SearchOffers(loc, DistanceValue, SearchWord);
            }
            else
            {
                cards = await PostRetriever.Instance().GetOffers(loc, DistanceValue);
            }
            PostCardList.Clear();
            PostCardList.AddRange(cards);
            if (PostCardList.Count == 0) await view.DisplayAlert("No offers found", "please change the introduced parameters, make sure location is enabled and refresh", "OK");
            IsBusy = false;
            return;
        }

        private async Task OnFilter()
        {
            if (FilterVisible == false) FilterVisible = true;
            else FilterVisible = false;
            return;
        }







    }
}