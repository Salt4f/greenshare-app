﻿using greenshare_app.Views.MainViewPages.ProfileViewPages.InteractionsPages;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace greenshare_app.Models
{
    public class AcceptedPostInteraction
    {
        public int OfferId { get; set; }
        public int RequestId { get; set; }
        public string OfferName { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public Page View { get; set; }
        public INavigation Navigation { get; set; }
        public AsyncCommand OnRateButtonCommand => new AsyncCommand(OnRate);
        private async void OnDisappear(object sender, EventArgs args)
        {
            await ((ViewModels.IncomingAcceptedViewModel)View.BindingContext).Refresh();
        }
        private async Task OnRate()
        {
            var view = new RatePage(this);
            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            view.Disappearing += OnDisappear;
            await Navigation.PushModalAsync(view);
        }

        public AcceptedPostInteraction(INavigation navigation, Page view)
        {
            Navigation = navigation;
            View = view;
        }
    }
}
