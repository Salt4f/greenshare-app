using greenshare_app.Models;
using greenshare_app.Utils;
using greenshare_app.Views.MainViewPages;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace greenshare_app.ViewModels
{
    public class OutgoingPendingViewModel : BaseViewModel
    {
        private event EventHandler Starting = delegate { };
        private Page view;
        private INavigation navigation;
        private ObservableRangeCollection<PendingPostInteraction> pendingPostInteractions;
        private PendingPostInteraction selectedPostInteraction;


        //public AsyncCommand<object> SelectedCommand => new AsyncCommand<object>(Selected);
        public AsyncCommand RefreshCommand => new AsyncCommand(Refresh);

        public OutgoingPendingViewModel(INavigation navigation, Page view)
        {
            Title = "Incoming Pending Interactions";
            this.navigation = navigation;
            this.view = view;
            PendingPostInteractions = new ObservableRangeCollection<PendingPostInteraction>();
            IsBusy = true;
            Starting += OnStart;
            Starting(this, EventArgs.Empty);
        }

        public ObservableRangeCollection<PendingPostInteraction> PendingPostInteractions
        {
            get => pendingPostInteractions;
            set => SetProperty(ref pendingPostInteractions, value);
        }

        public PendingPostInteraction SelectedPostInteraction
        {
            get => selectedPostInteraction;
            set => SetProperty(ref selectedPostInteraction, value);
        }

        private async void OnStart(object sender, EventArgs args)
        {
            try
            {
                List<PendingPostInteraction> pendingInteractions = new List<PendingPostInteraction>();
                pendingInteractions = await OfferRequestInteraction.Instance().GetPendingPosts("outgoing", navigation, view);
                PendingPostInteractions.Clear();
                PendingPostInteractions.AddRange(pendingInteractions);
                if (PendingPostInteractions.Count == 0)
                {
                    await view.DisplayAlert("No Pending Interactions left", "", "OK");
                    //PendingPostInteractions.Add(pendingTest);                   
                }
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert("Error while retrieving Pending Interactions", "Something went wrong", "OK");
            }
        }

        private async Task Refresh()
        {
            try
            {
                IsBusy = true;
                var pendingInteractions = await OfferRequestInteraction.Instance().GetPendingPosts("outgoing", navigation, view);
                PendingPostInteractions.Clear();
                PendingPostInteractions.AddRange(pendingInteractions);
                IsBusy = false;
                if (PendingPostInteractions.Count == 0)
                {
                    await view.DisplayAlert("No Pending Interactions left", "", "OK");
                }
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert("Error while retrieving Pending Interactions", "Something went wrong", "OK");
            }
        }

        private async Task Selected(object args)
        {
            IsBusy = true;
            var card = args as PendingPostInteraction;
            if (card == null)
            {
                IsBusy = false;
                return;
            }
            if (SelectedPostInteraction.PostType == "request")
            {
                Offer offer = await PostRetriever.Instance().GetOffer(SelectedPostInteraction.PostId);
                IsBusy = false;
                if (offer == null) await view.DisplayAlert("Error while retrieving Selected Offer", "Offer not found", "OK");
                else await navigation.PushModalAsync(new ViewPost(offer));
            }
            else
            {
                User user = await UserInfoUtil.Instance().GetUserInfo(SelectedPostInteraction.UserId);
                IsBusy = false;
                if (user == null) await view.DisplayAlert("Error while retrieving Selected user", "user not found", "OK");
                //else await navigation.PushModalAsync(new ViewPost(offer));
            }
        }
    }
}
