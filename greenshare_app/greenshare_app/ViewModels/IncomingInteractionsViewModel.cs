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
    internal class IncomingInteractionsViewModel : BaseViewModel
    {

        private event EventHandler Starting = delegate { };
        public AsyncCommand<object> SelectedCommand { get; }

        public IncomingInteractionsViewModel(INavigation navigation, Page view)
        {
            Title = "Pending Offers";
            PendingOffers = new ObservableRangeCollection<PendingPost>();
            SelectedCommand = new AsyncCommand<object>(Selected);
            Starting += OnStart;
            this.navigation = navigation;
            this.view = view;
            Starting(this, EventArgs.Empty);
        }

        private ObservableRangeCollection<PendingPost> pendingOffers;
        private INavigation navigation;
        private Page view;
        public ObservableRangeCollection<PendingPost> PendingOffers
        {
            get => pendingOffers;
            set => SetProperty(ref pendingOffers, value);
        }       
        private async void OnStart(object sender, EventArgs args)
        {
            try
            {
                IsBusy = true;
                //var cards = await PostRetriever.Instance().GetPendingOffers();
                //PendingOffers.AddRange(cards);
                //if (PendingOffers.Count == 0) await view.DisplayAlert("No pending offers found", "", "OK");
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                //await view.DisplayAlert("Error while retrieving offers", "Please make sure location is enabled on your device", "OK");
            }
            IsBusy = false;
        }
        async Task Selected(object args)
        {
            var post = args as PendingPost;
            if (post == null)
                return;

            Offer offer = await PostRetriever.Instance().GetOffer(post.PostId);
            if (offer == null) await view.DisplayAlert("Error while retrieving Selected Offer", "Offer not found", "OK");
            else await navigation.PushModalAsync(new ViewPost(offer));
            //await Application.Current.MainPage.DisplayAlert("Selected", coffee.Name, "OK");
        }

    }
}
