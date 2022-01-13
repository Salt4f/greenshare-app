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
using System.Threading;
using greenshare_app.Text;

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
        private bool isAdmin;

        private event EventHandler Starting = delegate { };

        //public AsyncCommand<object> SelectedCommand { get; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand OnCreateButtonCommand => new AsyncCommand(OnCreate);
        public AsyncCommand OnExchangeButtonCommand => new AsyncCommand(OnExchange);

        public ObservableRangeCollection<Reward> Rewards
        {
            get => rewards;
            set => SetProperty(ref rewards, value);
        }
        public bool IsAdmin
        {
            get => isAdmin;
            set => SetProperty(ref isAdmin, value);
        }
        public RewardsPageViewModel(INavigation navigation, Page view, User user)
        {
            Title = Text.Text.RewardsPage;
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
            IsAdmin = await Auth.Instance().IsAdmin();
            AvailableGreenCoins = Text.Text.YourGreenCoins + user.CurrentGreenCoins;

            Rewards = new ObservableRangeCollection<Reward>();
            try
            {
                var cards = await RewardsUtil.Instance().GetAllRewards(navigation, view, user.CurrentGreenCoins);
                Rewards.Clear();
                Rewards.AddRange(cards);
                if (Rewards.Count == 0) await view.DisplayAlert(Text.Text.NoRewardsFound, Text.Text.WeAreStillLookingForSponsors, "OK");
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert(Text.Text.NoRewardsFound, Text.Text.SomethingWentWrong, "OK");
            }
        }
        private async void OnDisappear(object sender, EventArgs args)
        {
            await Refresh();
        }
        private async Task OnCreate()
        {
            var view = new SponsorsFormPage();
            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            view.Disappearing += OnDisappear;
            await navigation.PushModalAsync(view);
        }
        private async Task OnExchange()
        {
            IsBusy = true;
            try
            {
                if (await RewardsUtil.Instance().ExchangeEcoPoints())
                {
                    await view.DisplayAlert("GreenCoins exchanged successfully", "all users' ecoPoints have been exchanged for greenCoins", "OK");
                }
                else
                {
                    await view.DisplayAlert("Error while exchanging your ecoPoints to GreenCoins", Text.Text.SomethingWentWrong, "OK");
                }
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert("Error while exchanging your ecoPoints to GreenCoins", Text.Text.SomethingWentWrong, "OK");
            }

            IsBusy = false;
            return;
        }

        private async Task UpdateGreenCoins()
        {
            user = await UserInfoUtil.Instance().GetUserInfo();
            AvailableGreenCoins = Text.Text.YourGreenCoins + user.CurrentGreenCoins;
            return;
        }
        public async Task Refresh()
        {
            try
            {
                IsBusy = true;
                await UpdateGreenCoins();
                var cards = await RewardsUtil.Instance().GetAllRewards(navigation, view, user.CurrentGreenCoins);
                Rewards.Clear();
                Rewards.AddRange(cards);
                if (Rewards.Count == 0) await view.DisplayAlert(Text.Text.NoRewardsFound, Text.Text.WeAreStillLookingForSponsors, "OK");
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                await view.DisplayAlert(Text.Text.NoRewardsFound, Text.Text.SomethingWentWrong, "OK");
            }
        }
    }
}
