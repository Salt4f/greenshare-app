using greenshare_app.Utils;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
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
            if (UserId == Config.Config.Instance().AdminId)
                IsAdmin = true;
            else IsAdmin = false;
            OnDeactivateButtonCommand = new AsyncCommand(OnDeactivate);
            OnEditButtonCommand = new AsyncCommand(OnEdit);
            OnExchangeFrameCommand = new AsyncCommand(OnExchange);
        }
        public int Id { get; set; }
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
                await View.DisplayAlert("Reward deleted successfully", "Please refresh to see the changes", "OK");
            }
            else await View.DisplayAlert("Could not delete reward", "Something went wrong", "OK");

        }
        private async Task OnEdit()
        {
            //navegación a vista de edit.
        }


    }
}
