using greenshare_app.Models;
using greenshare_app.Utils;
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
        public UserInfoPageViewModel(INavigation navigation, Page view)
        {
            Title = "Perfil";
            this.navigation = navigation;
            this.view = view;
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

        public AsyncCommand UserInfoCommand => new AsyncCommand(OnUserInfoClicked);
        private INavigation navigation;
        private Page view;
        private string description;
        private double averageValoration;
        private DateTime birthDate;
        private int totalEcoPoints;
        private int totalGreenCoins;

        private async Task OnUserInfoClicked()
        {
            await navigation.PushModalAsync(new UserInfoPage());
        }
    }
}
