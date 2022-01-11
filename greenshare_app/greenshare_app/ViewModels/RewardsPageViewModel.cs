using greenshare_app.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using greenshare_app.Utils;
using greenshare_app.Views.MainViewPages;
using System.Text;

namespace greenshare_app.ViewModels
{
    public class RewardsPageViewModel : BaseViewModel
    {
        private INavigation navigation;
        private Page view;

        //Hablar con Marc Plans sobre mostrar las GreenCoins disponibles del user.
        private User user;
        private ObservableRangeCollection<Reward> rewards;
        private string availableGreenCoins;

        private event EventHandler Starting = delegate { };

        //public AsyncCommand<object> SelectedCommand { get; }
        public AsyncCommand RefreshCommand { get; }
        public ObservableRangeCollection<Reward> Rewards
        {
            get => rewards;
            set => SetProperty(ref rewards, value);
        }
        public RewardsPageViewModel(INavigation navigation, Page view, User user)
        {
            Title = "Rewards Page";
            this.navigation = navigation;
            this.user = user;
            this.view = view;
            RefreshCommand = new AsyncCommand(Refresh);
            Starting += OnStart;           
            Starting(this, EventArgs.Empty);
        }
        public string AvailableGreenCoins
        {
            get => availableGreenCoins;
            set => SetProperty(ref availableGreenCoins, value);
        }
        private async void OnStart(object sender, EventArgs e)
        {
            IsBusy = true;
            AvailableGreenCoins = "Your GreenCoins: " + user.TotalGreenCoins;

            Rewards = new ObservableRangeCollection<Reward>();
            try
            {
                var cards = await RewardsUtil.Instance().GetAllRewards(navigation, view, user.TotalGreenCoins);
                Rewards.Clear();
                Rewards.AddRange(cards);
                if (Rewards.Count == 0) await view.DisplayAlert("No rewards found", "we are still looking for sponsors!", "OK");
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert("No rewards found", "something went wrong while retrieving rewards", "OK");
            }
        }

        private async Task UpdateGreenCoins()
        {
            user = await UserInfoUtil.Instance().GetUserInfo();
            AvailableGreenCoins = "Your GreenCoins: " + user.TotalGreenCoins;
        }
        private async Task Refresh()
        {
            try
            {
                IsBusy = true;
                await UpdateGreenCoins();
                var cards = await RewardsUtil.Instance().GetAllRewards(navigation, view, user.TotalGreenCoins);
                Rewards.Clear();
                Rewards.AddRange(cards);
                if (Rewards.Count == 0) await view.DisplayAlert("No rewards found", "we are still looking for sponsors!", "OK");
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert("No rewards found", "something went wrong while retrieving rewards", "OK");
            }
        }
    }
}
