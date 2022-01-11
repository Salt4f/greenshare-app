﻿using MvvmHelpers;
using greenshare_app.Utils;
using greenshare_app.Views;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;
using greenshare_app.Views.MainViewPages.ProfileViewPages;
using greenshare_app.Models;
using greenshare_app.Views.MainViewPages;

namespace greenshare_app.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel
    {
        private event EventHandler Starting = delegate { };
        private int userId;
        private User user;
        private bool ownPage;
        private bool isReportable;
        public ProfilePageViewModel(INavigation navigation, Page view)
        {
            Title = "Perfil";
            this.navigation = navigation;
            this.view = view;
            OwnPage = true;
            nickName = string.Empty;
            IsBusy = true;
            Starting += OnStart;
            Starting(this, EventArgs.Empty);
        }
        public ProfilePageViewModel(INavigation navigation, Page view, int userId)
        {
            Title = "Perfil";
            this.navigation = navigation;
            this.view = view;            
            this.userId = userId;
            OwnPage = false;
            nickName = string.Empty;
            IsBusy = true;
            Starting += OnStart;
            Starting(this, EventArgs.Empty);
            
        }

        private string nickName;
        private double rating = 3.5;
        public string NickName {
            get => nickName;
            set => SetProperty(ref nickName, value);
        }
        public bool IsReportable
        {
            get => isReportable;
            private set => SetProperty(ref isReportable, value);
        }

        public double Rating
        {
            get => rating;
            set => SetProperty(ref rating, value);

        }
        public bool OwnPage
        {
            get => ownPage;
            private set => SetProperty(ref ownPage, value);
        }

        public bool NotAdminNotOwnPage
        {
            get => !IsAdmin && !OwnPage;
        }
        public bool IsAdminOwnPage
        {
            get => IsAdmin && OwnPage;
        }
        public bool IsAdminNotOwnPage
        {
            get => IsAdmin && !OwnPage;
        }
        public bool IsAdmin
        {
            get => isAdmin;
            private set => SetProperty(ref isAdmin, value);
        }

        private async void OnStart(object sender, EventArgs args)
        {
            IsReportable = !OwnPage;
            try
            {
                IsAdmin = await Auth.Instance().IsAdmin();
                if (OwnPage) user = await UserInfoUtil.Instance().GetUserInfo();
                else user = await UserInfoUtil.Instance().GetUserInfo(userId);
                NickName = user.NickName;
            }
            catch (Exception e)
            {
                var type = e.GetType();
                var error = e.Message;
                IsBusy = false;
                await view.DisplayAlert("Internal Server Error", "Something went wrong", "OK");
            }
            IsBusy = false;
        }
        
        public AsyncCommand OnAdminFrameCommand => new AsyncCommand(OnAdmin);

        public AsyncCommand UserInfoCommand => new AsyncCommand(OnUserInfo);
        public AsyncCommand OnRewardsButtonCommand => new AsyncCommand(OnRewards);
        public AsyncCommand UserPostsCommand => new AsyncCommand(OnUserPostsButton);
        public AsyncCommand UserLogOutCommand => new AsyncCommand(OnLogOutButton);
        public AsyncCommand UserIncomingInteractionsCommand => new AsyncCommand(OnIncomingInteractionsButton);
        public AsyncCommand UserOutgoingInteractionsCommand => new AsyncCommand(OnOutgoingInteractionsButton);
        public AsyncCommand OnReportButtonCommand => new AsyncCommand(OnReportButton);
        public AsyncCommand OnBanButtonCommand => new AsyncCommand(OnBanButton);


        //TODO: RATE PAGE PER USER
        //public AsyncCommand OnRateButtonCommand => new AsyncCommand(OnRateButton);

        private INavigation navigation;
        private Page view;
        private bool isAdmin;

        private async Task OnUserInfo()
        {
            if (OwnPage)
            {
                Tuple<int, string> session = await Auth.Instance().GetAuth();
                await navigation.PushModalAsync(new UserInfoPage(session.Item1, OwnPage));
            }
            else await navigation.PushModalAsync(new UserInfoPage(userId, OwnPage));
        }
        private async Task OnRewards()
        {
            await navigation.PushModalAsync(new RewardsPage(user));
        }
        private async Task OnUserPostsButton()
        {
            await navigation.PushModalAsync(new UserPublicationsPage());
        }
        private async Task OnLogOutButton()
        {
            await Auth.Instance().Logout();
            Application.Current.MainPage = new LoginView();
        }
        private async Task OnIncomingInteractionsButton()
        {
            await navigation.PushModalAsync(new IncomingInteractionsPage());
        }
        private async Task OnOutgoingInteractionsButton()
        {
            await navigation.PushModalAsync(new OutgoingInteractionsPage());
        }

        private async Task OnAdmin()
        {
            await navigation.PushModalAsync(new AdminPage());
        }

        private async Task OnReportButton()
        {
            await navigation.PushModalAsync(new ReportPage(typeof(User), userId));
        }

        private async Task OnBanButton()
        {
            //TODO funcion de baneo
        }
        //TODO: RATE PAGE PER USER
        //private async Task OnRateButton()
        //{
        //    await navigation.PushModalAsync(new RatePage(typeof(User), userId));
        //}
    }
}