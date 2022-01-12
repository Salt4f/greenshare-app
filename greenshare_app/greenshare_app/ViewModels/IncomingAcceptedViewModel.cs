using greenshare_app.Models;
using greenshare_app.Utils;
using greenshare_app.Views.MainViewPages;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using greenshare_app.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace greenshare_app.ViewModels
{
    public class IncomingAcceptedViewModel : BaseViewModel
    {
        private event EventHandler Starting = delegate { };
        private Page view;
        private INavigation navigation;
        private ObservableRangeCollection<AcceptedPostInteraction> acceptedPostInteractions;
        private AcceptedPostInteraction selectedPostInteraction;


        //public AsyncCommand<object> SelectedCommand => new AsyncCommand<object>(Selected);
        public AsyncCommand RefreshCommand => new AsyncCommand(Refresh);

        public IncomingAcceptedViewModel(INavigation navigation, Page view)
        {
            Title = Text.Text.IncomingAcceptedInteractions;
            this.navigation = navigation;
            this.view = view;
            AcceptedPostInteractions = new ObservableRangeCollection<AcceptedPostInteraction>();
            IsBusy = true;
            Starting += OnStart;
            Starting(this, EventArgs.Empty);
        }

        public ObservableRangeCollection<AcceptedPostInteraction> AcceptedPostInteractions
        {
            get => acceptedPostInteractions;
            set => SetProperty(ref acceptedPostInteractions, value);
        }

        public AcceptedPostInteraction SelectedPostInteraction
        {
            get => selectedPostInteraction;
            set => SetProperty(ref selectedPostInteraction, value);
        }

        private async void OnStart(object sender, EventArgs args)
        {
            try
            {
                List<AcceptedPostInteraction> acceptedInteractions = new List<AcceptedPostInteraction>();
                acceptedInteractions = await OfferRequestInteraction.Instance().GetAcceptedPosts("incoming", navigation, view);
                AcceptedPostInteractions.Clear();
                AcceptedPostInteractions.AddRange(acceptedInteractions);
                if (AcceptedPostInteractions.Count == 0)
                {
                    await view.DisplayAlert(Text.Text.NoAcceptedInteractionsleft, "", "OK");
                    //PendingPostInteractions.Add(pendingTest);                   
                }
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert(Text.Text.ErrorWhileRetrievingAcceptedInteractions, Text.Text.SomethingWentWrong, "OK");
            }
        }

        public async Task Refresh()
        {
            try
            {
                IsBusy = true;
                List<AcceptedPostInteraction> acceptedInteractions = new List<AcceptedPostInteraction>();
                acceptedInteractions = await OfferRequestInteraction.Instance().GetAcceptedPosts("incoming", navigation, view);
                AcceptedPostInteractions.Clear();
                AcceptedPostInteractions.AddRange(acceptedInteractions);
                IsBusy = false;
                if (AcceptedPostInteractions.Count == 0)
                {
                    await view.DisplayAlert(Text.Text.NoAcceptedInteractionsleft, "", "OK");
                }
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert(Text.Text.ErrorWhileRetrievingAcceptedInteractions, Text.Text.SomethingWentWrong, "OK");
            }
        }

       

    }
}
