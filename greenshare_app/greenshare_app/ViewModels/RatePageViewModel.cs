using greenshare_app.Models;
using greenshare_app.Utils;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace greenshare_app.ViewModels
{
    public class RatePageViewModel : BaseViewModel
    {
        private int ratingValue;
        private INavigation navigation;
        private Page view;
        private AcceptedPostInteraction acceptedPost;
        private string message;

        public AsyncCommand OnCompleteButtonCommand => new AsyncCommand(OnComplete);
        public RatePageViewModel(INavigation navigation, Page view, AcceptedPostInteraction acceptedPost)
        {
            this.navigation = navigation;
            this.view = view;
            this.acceptedPost = acceptedPost;
            RatingValue = 5;
        }
        public int RatingValue
        {
            get => ratingValue;
            set => SetProperty(ref ratingValue, value);
        }
        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }
        private async Task OnComplete()
        {
            try
            {
                IsBusy = true;
                await OfferRequestInteraction.Instance().CompletePostFromOffer(acceptedPost.OfferId, acceptedPost.RequestId, RatingValue, Message);
                IsBusy = false;
                await view.DisplayAlert("Post completed successfully", "", "OK");
            }
            catch (Exception)
            {
                await view.DisplayAlert("Error while completing the post", "something went wrong", "OK");
            }
            await navigation.PopModalAsync();
        }

    }
}
