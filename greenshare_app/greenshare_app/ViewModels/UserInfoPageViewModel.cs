using greenshare_app.Models;
using greenshare_app.Utils;
using greenshare_app.Views.MainViewPages;
using greenshare_app.Views.MainViewPages.ProfileViewPages;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using greenshare_app.Text;

namespace greenshare_app.ViewModels
{
    public class UserInfoPageViewModel : BaseViewModel
    {
        private event EventHandler Starting = delegate { };

        private User user;
        public AsyncCommand OnEditFrameCommand => new AsyncCommand(OnEdit);
        private bool ownPage;
        public UserInfoPageViewModel(INavigation navigation, Page view, int userId, bool ownPage)
        {
            Title =Text.Text.Profile;
            this.userId = userId;
            this.navigation = navigation;
            this.view = view;
            OwnPage = ownPage;
            nickName = string.Empty;
            IsBusy = true;
            Starting += OnStart;
            Starting(this, EventArgs.Empty);
        }

        private string nickName;
        public string NickName
        {
            get => nickName;
            set => SetProperty(ref nickName, value);
        }
        public string FullName
        {
            get => fullName;
            set => SetProperty(ref fullName, value);
        }
        public string Description 
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        public Image ProfilePicture { get; set; }
        public double AverageValoration
        {
            get => averageValoration;
            set => SetProperty(ref averageValoration, value);
        }
        public DateTime BirthDate
        {
            get => birthDate;
            set => SetProperty(ref birthDate, value);
        }
        public int TotalEcoPoints
        {
            get => totalEcoPoints;
            set => SetProperty(ref totalEcoPoints, value);
        }
        public int TotalGreenCoins
        {
            get => totalGreenCoins;
            set => SetProperty(ref totalGreenCoins, value);
        }        
        public bool OwnPage
        {
            get => ownPage;
            private set => SetProperty(ref ownPage, value);
        }
        private async void OnStart(object sender, EventArgs args)
        {
            IsBusy = true;
            try
            {
                user = await UserInfoUtil.Instance().GetUserInfo();
                NickName = user.NickName;
                FullName = user.FullName;
                Description = user.Description;
                ProfilePicture = user.ProfilePicture;
                AverageValoration = user.AverageValoration;
                BirthDate = user.BirthDate;
                TotalEcoPoints = user.TotalEcoPoints;
                TotalGreenCoins = user.TotalGreenCoins;

            }
            catch (Exception e)
            {
                var type = e.GetType();
                var error = e.Message;
                IsBusy = false;
                await view.DisplayAlert(Text.Text.InternalServerError, Text.Text.SomethingWentWrong, "OK");
            }
            IsBusy = false;
        }
        private void OnDisappear(object sender, EventArgs args)
        {
            OnStart(this, EventArgs.Empty);
        }
        private async Task OnEdit()
        {
            var view = new UserInfoUpdatePage(this.user);
            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            view.Disappearing += OnDisappear;
            await navigation.PushModalAsync(view);
        }
        private int userId;
        private INavigation navigation;
        private Page view;
        private string description;
        private double averageValoration;
        private DateTime birthDate;
        private int totalEcoPoints;
        private int totalGreenCoins;
        private bool isReportable;
        private string fullName;
    }
}
