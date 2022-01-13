using greenshare_app.Utils;
using greenshare_app.Views.MainViewPages;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using greenshare_app.Text;

namespace greenshare_app.Models
{
    public class Reward
    {
        public AsyncCommand OnDeactivateButtonCommand { get; }
        public AsyncCommand OnEditButtonCommand { get; }
        public AsyncCommand OnExchangeFrameCommand { get; }
        public Reward(INavigation navigation, Page view)
        {
            Navigation = navigation;
            View = view;
            if (View.BindingContext.GetType() == typeof(ViewModels.RewardsPageViewModel)) IsAdmin = ((ViewModels.RewardsPageViewModel)View.BindingContext).IsAdmin;
            else IsAdmin = true;
            OnDeactivateButtonCommand = new AsyncCommand(OnDeactivate);
            OnEditButtonCommand = new AsyncCommand(OnEdit);
            OnExchangeFrameCommand = new AsyncCommand(OnExchange);
        }
        public int Id { get; set; }
        public string  Name { get; set; }
        public bool IsAdmin { get; set; }
        public int GreenCoinsAvailable { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }       
        public string SponsorName { get; set; }        
        public int GreenCoins { get; set; }
        public string GreenCoinsText { get; set; }
        public Page View { get; set; }
        public INavigation Navigation { get; set; }
        private async Task OnExchange()
        {
            ((ViewModels.RewardsPageViewModel) View.BindingContext).IsBusy = true;
            if (GreenCoinsAvailable >= GreenCoins)
            {
                if (await RewardsUtil.Instance().ExchangeReward(Id))
                {
                    await ((ViewModels.RewardsPageViewModel)View.BindingContext).Refresh();
                    await View.DisplayAlert(Text.Text.RewardExchangedSuccessfully,Text.Text.SuccessfullyExchangeThePromotion +SponsorName, "OK");
                    ((ViewModels.RewardsPageViewModel)View.BindingContext).IsBusy = false;
                    return;
                }
                await View.DisplayAlert(Text.Text.ErrorWhileExchangingTheReward, Text.Text.SomethingWentWrong, "OK");                
            }
            else
            {
                await View.DisplayAlert(Text.Text.ErrorWhileExchangingTheReward, Text.Text.YouDontHaveEnoughGreenCoins, "OK");
            }
            ((ViewModels.RewardsPageViewModel)View.BindingContext).IsBusy = false;
            return;

        }
        private async Task OnDeactivate()
        {
            ((ViewModels.RewardsPageViewModel)View.BindingContext).IsBusy = true;
            if (await RewardsUtil.Instance().DeactivateReward(Id))
            {
                await View.DisplayAlert(Text.Text.RewardDeletedSuccessfully, "", "OK");
                await ((ViewModels.RewardsPageViewModel)View.BindingContext).Refresh();
            }
            else await View.DisplayAlert(Text.Text.CouldNotDeleteReward, Text.Text.SomethingWentWrong, "OK");
            ((ViewModels.RewardsPageViewModel)View.BindingContext).IsBusy = false;

        }
        private async void OnDisappear(object sender, EventArgs args)
        {
            await ((ViewModels.RewardsPageViewModel)View.BindingContext).Refresh();
            ((ViewModels.RewardsPageViewModel)View.BindingContext).IsBusy = false;
        }
        private async Task OnEdit()
        {
            ((ViewModels.RewardsPageViewModel)View.BindingContext).IsBusy = true;
            var view = new SponsorsFormPage(this);
            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            view.Disappearing += OnDisappear;
            await Navigation.PushModalAsync(view);
        }


    }
}
