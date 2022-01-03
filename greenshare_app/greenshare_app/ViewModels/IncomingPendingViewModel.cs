using greenshare_app.Models;
using greenshare_app.Utils;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace greenshare_app.ViewModels
{
    public class IncomingPendingViewModel : BaseViewModel
    {
        private event EventHandler Starting = delegate { };
        private Page view;
        private INavigation navigation;
        private ObservableRangeCollection<PendingPostInteraction> pendingPostInteractions;

        public IncomingPendingViewModel(INavigation navigation, Page view)
        {
            Title = "Ofertes";
            this.navigation = navigation;
            this.view = view;
            IsBusy = true;
            Starting += OnStart;
            Starting(this, EventArgs.Empty);
        }

        public ObservableRangeCollection<PendingPostInteraction> PendingPostInteractions
        {
            get => pendingPostInteractions;
            set => SetProperty(ref pendingPostInteractions, value);
        }

        private async void OnStart(object sender, EventArgs args)
        {
            try
            {
                var interactions = await OfferRequestInteraction.Instance().GetPendingPosts("Incoming");
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert("Error while retrieving Pending Interactions", "Something went wrong", "OK");
            }
        }
    }
}
