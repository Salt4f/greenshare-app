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
    public class RequestsPageViewModel : BaseViewModel
    {
        private ObservableRangeCollection<PostCard> postCardList;
        private PostCard selectedPostCard;
        private INavigation navigation;
        private Page view;
        private event EventHandler Starting = delegate { };
        public RequestsPageViewModel(INavigation navigation, Page view)
        {
            Title = "Peticions";
            IsBusy = true;
            SelectedCommand = new AsyncCommand<object>(Selected);
            this.navigation = navigation;
            this.view = view;
            selectedPostCard = new PostCard();
            postCardList = new ObservableRangeCollection<PostCard>();           
            Starting += OnStart;
            Starting(this, EventArgs.Empty);
            
        }

        private async Task<Location> UpdateLocation()
        {
            return await Geolocation.GetLastKnownLocationAsync();
        }

        private async void OnStart(object sender, EventArgs args)
        {
            try
            {
                CurrentLocation = await UpdateLocation();
                //postCardList = await PostRetriever.Instance().GetRequests();
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert("Internal Server Error", "Something went wrong", "OK");
            }
            IsBusy = false;
        }

        public Location CurrentLocation { get; set; }
       
        public ObservableRangeCollection<PostCard> PostCardList
        {
            get => postCardList;
            set => SetProperty(ref postCardList, value);
        }

        public AsyncCommand<object> SelectedCommand { get; }
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