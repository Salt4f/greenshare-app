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
        private async Task OnComplete()
        {
            if (RatingValue > 5 || RatingValue < 1)
            {
                await view.DisplayAlert("Make sure you rate your experience first", "Rating is necessary", "OK");
            }
            else
            {
                try
                {
                    await OfferRequestInteraction.Instance().CompletePostFromOffer(acceptedPost.OfferId, acceptedPost.RequestId, RatingValue);
                }
                catch (Exception)
                {
                    await view.DisplayAlert("Error while completing the post", "something went wrong", "OK");
                }
            }
        }

    }
}
