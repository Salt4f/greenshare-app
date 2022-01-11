using greenshare_app.Models;
using greenshare_app.Utils;
using greenshare_app.Views.MainViewPages;
using greenshare_app.Views.MainViewPages.ProfileViewPages;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace greenshare_app.ViewModels
{
    public class UserInfoPageViewModel : BaseViewModel
    {
        private event EventHandler Starting = delegate { };

        private bool ownPage;
        public UserInfoPageViewModel(INavigation navigation, Page view, int userId, bool ownPage)
        {
            Title = "Perfil";
            this.userId = userId;
            this.navigation = navigation;
            this.view = view;
            OwnPage = ownPage;
            IsReportable = !OwnPage;
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

        public bool IsReportable
        {
            get => isReportable;
            private set => SetProperty(ref isReportable, value);
        }
        public bool OwnPage
        {
            get => ownPage;
            private set => SetProperty(ref ownPage, value);
        }
        private async void OnStart(object sender, EventArgs args)
        {
            try
            {
                User user = await UserInfoUtil.Instance().GetUserInfo();
                NickName = user.NickName;
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
                await view.DisplayAlert("Internal Server Error", "Something went wrong", "OK");
            }
            IsBusy = false;
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
    }
}
