using greenshare_app.Utils;
using greenshare_app.Views.MainViewPages;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace greenshare_app.Models
{
    public class PendingPostInteraction
    {
        public PendingPostInteraction(INavigation navigation, Page view)
        {
            OnAcceptButtonCommand = new AsyncCommand(OnAccept);
            Navigation = navigation;
            View = view;
            OnRejectButtonCommand = new AsyncCommand(OnReject);
            OnCancelButtonCommand = new AsyncCommand(OnCancel);
            OnUserNameLabelCommand = new AsyncCommand(OnUser);           
            OnTitleLabelCommand = new AsyncCommand(OnTitle);
        }
        public string UserName { get; set; }
        public INavigation Navigation { get; set; }
        public Page View { get; set; }
        public int UserId { get; set; }
        public string PostType { get; set; }
        public int PostId { get; set; }
        public string PostName { get; set; }
        public string InteractionText { get; set; }
        //sera el text que indicarà que està succeint en aquella interacció
        //ex: en guillem t'està oferint a la teva solicitud d'una bicicleta
        //ex: en guillem t'està sol·licitant el teu ordinador
        public int OwnPostId { get; set; }
        public AsyncCommand OnAcceptButtonCommand { get; set; }
        public AsyncCommand OnRejectButtonCommand { get; set; }
        public AsyncCommand OnCancelButtonCommand { get; set; }
        public AsyncCommand OnUserNameLabelCommand { get; set; }
        public AsyncCommand OnTitleLabelCommand { get; set; }

        private async Task OnCancel()
        {
            ((ViewModels.OutgoingPendingViewModel)View.BindingContext).IsBusy = true;
            if (PostType == "request")
            {
                await OfferRequestInteraction.Instance().CancelRequest(PostId, OwnPostId);
                await ((ViewModels.OutgoingPendingViewModel)View.BindingContext).Refresh();
            }
            else
            {
                await OfferRequestInteraction.Instance().CancelOffer(OwnPostId, PostId);
                await ((ViewModels.OutgoingPendingViewModel)View.BindingContext).Refresh();
            }
            ((ViewModels.OutgoingPendingViewModel)View.BindingContext).IsBusy = false;
        }
        private async Task OnUser()
        {            
            await Navigation.PushModalAsync(new ProfilePage(UserId));
        }

        private async Task OnTitle()
        {

            if (PostType == "offer")
            {
                Offer offer = await PostRetriever.Instance().GetOffer(OwnPostId); 
                await Navigation.PushModalAsync(new ViewPost(offer));
            }
            else
            {
                Offer offer = await PostRetriever.Instance().GetOffer(PostId);
                await Navigation.PushModalAsync(new ViewPost(offer));
            }
        }
        private async Task OnAccept()
        {
            ((ViewModels.IncomingPendingViewModel)View.BindingContext).IsBusy = true;
            if (PostType == "offer")
            {
                if(await OfferRequestInteraction.Instance().AcceptRequest(OwnPostId, PostId))
                await View.DisplayAlert("Request accepted", "", "OK");
                await ((ViewModels.IncomingPendingViewModel)View.BindingContext).Refresh();
            }
            else
            {
                await OfferRequestInteraction.Instance().AcceptOffer(PostId, OwnPostId);
                await ((ViewModels.IncomingPendingViewModel)View.BindingContext).Refresh();
            }
            ((ViewModels.IncomingPendingViewModel)View.BindingContext).IsBusy = false;
        }

        private async Task OnReject()
        {
            ((ViewModels.IncomingPendingViewModel)View.BindingContext).IsBusy = true;
            if (PostType == "offer")
            {
                if (await OfferRequestInteraction.Instance().RejectRequest(OwnPostId, PostId))
                await View.DisplayAlert("Request rejected", "", "OK");
                await ((ViewModels.IncomingPendingViewModel)View.BindingContext).Refresh();
            }
            else
            {
                await OfferRequestInteraction.Instance().RejectOffer(PostId, OwnPostId);
                await View.DisplayAlert("Offer rejected", "", "OK");
                await ((ViewModels.IncomingPendingViewModel)View.BindingContext).Refresh();

            }
            ((ViewModels.IncomingPendingViewModel)View.BindingContext).IsBusy = false;
        }
    }
}
