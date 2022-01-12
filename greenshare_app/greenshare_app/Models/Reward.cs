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
            if (GreenCoinsAvailable >= GreenCoins)
            {
                if (await RewardsUtil.Instance().ExchangeReward(Id))
                {
                    await View.DisplayAlert("Reward exchanged successfully", "successfully exchanged the promotion from "+SponsorName, "OK");
                }
                await View.DisplayAlert("Error while exchanging the reward", "something went wrong", "OK");
            }
            else
            {
                await View.DisplayAlert("Error while exchanging the reward", "you don't have enough GreenCoins!", "OK");
            }

        }
        private async Task OnDeactivate()
        {
            if (await RewardsUtil.Instance().DeactivateReward(Id))
            {
                await View.DisplayAlert("Reward deleted successfully", "", "OK");
                await ((ViewModels.RewardsPageViewModel)View.BindingContext).Refresh();
            }
            else await View.DisplayAlert("Could not delete reward", "Something went wrong", "OK");

        }
        private async void OnDisappear(object sender, EventArgs args)
        {
            await ((ViewModels.RewardsPageViewModel)View.BindingContext).Refresh();
        }
        private async Task OnEdit()
        {
            var view = new SponsorsFormPage(this);
            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            view.Disappearing += OnDisappear;
            await Navigation.PushModalAsync(view);
        }


    }
}
