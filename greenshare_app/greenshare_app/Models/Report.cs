﻿using greenshare_app.Utils;
using greenshare_app.Views.MainViewPages;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace greenshare_app.Models
{
    public class Report
    {
        public Report(INavigation navigation, Page view)
        {
            Navigation = navigation;
            View = view;
            OnItemNameFrameCommand = new AsyncCommand(OnItemName);
            OnReporterIdFrameCommand = new AsyncCommand(OnReporterId);
            OnSolveButtonCommand = new AsyncCommand(OnSolve);
        }
        public INavigation Navigation { get; set; }
        public Page View { get; set; }
        public string ItemName { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }
        public int ItemId { get; set; }
        public string Message { get; set; }
        public int ReporterId { get; set; }
        public bool Solved { get; set; }
        public AsyncCommand OnItemNameFrameCommand { get; set; }
        public AsyncCommand OnReporterIdFrameCommand { get; set; }
        public AsyncCommand OnSolveButtonCommand { get; set; }

        private async Task OnItemName()
        {           
            if (Type == "offer")
            {
                Offer offer = await PostRetriever.Instance().GetOffer(ItemId);
                var view = new ViewPost(offer);
                view.Disappearing += OnDisappear;
                await Navigation.PushModalAsync(view);
                return;
            }
            else if (Type == "request")
            {
                Request request = await PostRetriever.Instance().GetRequest(ItemId);
                var view = new ViewPost(request);
                view.Disappearing += OnDisappear;
                await Navigation.PushModalAsync(view);
                return;
            }
            else
            {
                var view = new ProfilePage(ItemId);
                view.Disappearing += OnDisappear;
                await Navigation.PushModalAsync(view);
                return;
            }
        }
        private async Task OnSolve()
        {
            await ReportUtil.Instance().SolveReport(Id);
            await ((ViewModels.AdminPageViewModel)View.BindingContext).Refresh();
        }
        private async Task OnReporterId()
        {
            var view = new ProfilePage(ReporterId);
            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            view.Disappearing += OnDisappear;
            await Navigation.PushModalAsync(view);
        }

        private async void OnDisappear(object sender, EventArgs e)
        {
            await ((ViewModels.AdminPageViewModel)View.BindingContext).Refresh();
        }
    }
}
