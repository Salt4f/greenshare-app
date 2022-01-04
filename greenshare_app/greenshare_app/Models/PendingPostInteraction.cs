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
        public string UserName { get; set; }
        public INavigation navigation { get; set; }
        public int UserId { get; set; }
        public string PostType { get; set; }
        public int PostId { get; set; }
        public string PostName { get; set; }
        public string InteractionText { get; set; }
        //sera el text que indicarà que està succeint en aquella interacció
        //ex: en guillem t'està oferint a la teva solicitud d'una bicicleta
        //ex: en guillem t'està sol·licitant el teu ordinador
        public int OwnPostId { get; set; }
        public AsyncCommand OnAcceptButtonCommand => new AsyncCommand(OnAccept);
        public AsyncCommand OnRejectButtonCommand => new AsyncCommand(OnReject);
        public AsyncCommand OnCancelButtonCommand => new AsyncCommand(OnCancel);
        public AsyncCommand OnUserNameLabelCommand => new AsyncCommand(OnUser);
        public AsyncCommand OnTitleLabelCommand => new AsyncCommand(OnTitle);

        private async Task OnCancel()
        {
            if (PostType == "offer")
            {
                await OfferRequestInteraction.Instance().AcceptRequest(OwnPostId, PostId);
            }
            else
            {
                await OfferRequestInteraction.Instance().AcceptOffer(OwnPostId, PostId);
            }
        }
        private async Task OnUser()
        {            
            await navigation.PushModalAsync(new ProfilePage(UserId));
        }

        private async Task OnTitle()
        {

            if (PostType == "offer")
            {
                Request request = await PostRetriever.Instance().GetRequest(PostId); 
                await navigation.PushModalAsync(new ViewPost(request));
            }
            else
            {
                Offer offer = await PostRetriever.Instance().GetOffer(PostId);
                await navigation.PushModalAsync(new ViewPost(offer));
            }
        }
        private async Task OnAccept()
        {
            if (PostType == "offer")
            {
                await OfferRequestInteraction.Instance().AcceptRequest(OwnPostId, PostId);
            }
            else
            {
                await OfferRequestInteraction.Instance().AcceptOffer(OwnPostId, PostId);
            }
        }

        private async Task OnReject()
        {                           
            if (PostType == "offer")
            {
                await OfferRequestInteraction.Instance().RejectRequest(OwnPostId, PostId);
            }
            else
            {
                await OfferRequestInteraction.Instance().RejectOffer(OwnPostId, PostId);
            }
        }                       
    }
}
