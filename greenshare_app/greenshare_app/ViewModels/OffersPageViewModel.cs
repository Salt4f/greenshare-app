using greenshare_app.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using greenshare_app.Utils;
using Xamarin.Essentials;

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
            SelectedCommand = new AsyncCommand<object>(Selected);
            RefreshCommand = new AsyncCommand(Refresh);

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
                postCardList.AddRange(await PostRetriever.Instance().GetOffers(await Geolocation.GetLastKnownLocationAsync()));
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert("Internal Server Error", "Something went wrong", "OK");
            }
            IsBusy = false;
        }

        private async Task Refresh()
        {
            IsBusy = true;
            PostCardList.Clear();
            postCardList.AddRange(await PostRetriever.Instance().GetOffers(await Geolocation.GetLastKnownLocationAsync()));
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
            set => SetProperty(ref selectedPostCard, value);
        }

        private async Task Selected(object args)
        {
            var postCard = args as PostCard;
            if (postCard == null)
                return;

            SelectedPostCard = null;


            await view.DisplayAlert("Selected", postCard.Name, "OK");
            //await Application.Current.MainPage.DisplayAlert("Selected", coffee.Name, "OK");

        }





    }
}