﻿using greenshare_app.Models;
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
using greenshare_app.Text;

namespace greenshare_app.ViewModels
{
    public class AdminPageViewModel : BaseViewModel
    {
        private ObservableRangeCollection<Report> reportList;
        private INavigation navigation;
        private Page view;

        public AsyncCommand<object> SelectedCommand { get; }
        public AsyncCommand RefreshCommand { get; }

        private event EventHandler Starting = delegate { };
        public AdminPageViewModel(INavigation navigation, Page view)
        {
            Title = Text.Text.AdminPage;

            IsBusy = true;
            RefreshCommand = new AsyncCommand(Refresh);
            this.navigation = navigation;
            this.view = view;
            ReportList = new ObservableRangeCollection<Report>();

            Starting += OnStart;
            Starting(this, EventArgs.Empty);
        }

        private async void OnStart(object sender, EventArgs args)
        {
            try
            {
                IsBusy = true;                
                var cards = await ReportUtil.Instance().GetAllReports(navigation, view);
                ReportList.Clear();
                ReportList.AddRange(cards);
                if (ReportList.Count == 0) await view.DisplayAlert(Text.Text.NoReportsFound, Text.Text.ItsAPeacefulDayInTheApp , "OK");
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert(Text.Text.ErrorWhileRetrievingReports, "", "OK");
            }
            IsBusy = false;
        }

        public async Task Refresh()
        {
            try
            {
                IsBusy = true;
                var cards = await ReportUtil.Instance().GetAllReports(navigation, view);
                ReportList.Clear();
                ReportList.AddRange(cards);
                if (ReportList.Count == 0) await view.DisplayAlert(Text.Text.NoReportsFound, Text.Text.ItsAPeacefulDayInTheApp, "OK");
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert(Text.Text.ErrorWhileRetrievingReports, "", "OK");
            }
        }

        public ObservableRangeCollection<Report> ReportList
        {
            get => reportList;
            set => SetProperty(ref reportList, value);
        }
    }
}
