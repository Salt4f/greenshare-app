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
using System.Threading;
using greenshare_app.Text;

namespace greenshare_app.ViewModels
{
    public class RequestsPageViewModel : BaseViewModel
    {
        private ObservableRangeCollection<PostCard> postCardList;
        private PostCard selectedPostCard;
        private INavigation navigation;
        private Page view;
        private int distanceValue;
        private bool filterVisible;
        private string searchWord;

        public AsyncCommand<object> SelectedCommand { get; }
        public AsyncCommand RefreshCommand { get; }

        private event EventHandler Starting = delegate { };
        public RequestsPageViewModel(INavigation navigation, Page view)
        {
            Title = "Peticions";            
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
                var loc = await Geolocation.GetLocationAsync();
                var cards = await PostRetriever.Instance().GetRequests(loc);
                PostCardList.AddRange(cards);
                if (PostCardList.Count == 0) await view.DisplayAlert("No offers found", "please change your location and refresh", "OK");
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert("Error while retrieving requests", "Please make sure location is enabled on your device", "OK");
            }
            IsBusy = false;
        }

        private async Task Refresh()
        {
            try
            {
                IsBusy = true;
                //await navigation.PopToRootAsync();
                var loc = await Geolocation.GetLocationAsync();
                var cards = await PostRetriever.Instance().GetRequests(loc);
                PostCardList.Clear();
                PostCardList.AddRange(cards);
                if (PostCardList.Count == 0) await view.DisplayAlert("No requests found", "please change your location and refresh", "OK");

                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert("Error while retrieving requests", "Please make sure location is enabled on your device", "OK");
            }
            IsBusy = false;
        }

        public ObservableRangeCollection<PostCard> PostCardList
        {
            get => postCardList;
            set => SetProperty(ref postCardList, value);
        }

        public PostCard SelectedPostCard
        {
            get => selectedPostCard;
            set
            {
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


        private async void OnDisappear(object sender, EventArgs args)
        {
            await Refresh();
        }

        async Task Selected(object args)
        {
            var card = args as PostCard;
            if (card == null)
                return;
            try
            {
                IsBusy = true;
                Request request = await PostRetriever.Instance().GetRequest(SelectedPostCard.Id);
                if (request == null) await view.DisplayAlert(Text.Text.ErrorWhileRetrievingSelectedRequest, Text.Text.OfferNotFound, "OK");
                else
                {
                    var view = new ViewPost(request);
                    var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
                    view.Disappearing += OnDisappear;
                    await navigation.PushModalAsync(view);
                }
                IsBusy = false;
            }
            catch (Exception)
            {
                await view.DisplayAlert(Text.Text.ErrorWhileRetrievingSelectedRequest, Text.Text.SomethingWentWrong, "OK");
            }
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
                cards = await PostRetriever.Instance().SearchRequests(loc, DistanceValue, SearchWord);
            }
            else
            {
                cards = await PostRetriever.Instance().GetRequests(loc, DistanceValue);
            }
            PostCardList.Clear();
            PostCardList.AddRange(cards);
            if (PostCardList.Count == 0) await view.DisplayAlert(Text.Text.NoRequestsFound,Text.Text.PleaseChangeTheIntroducedParameters, "OK");
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