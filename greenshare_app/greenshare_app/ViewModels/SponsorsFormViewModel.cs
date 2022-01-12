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

namespace greenshare_app.ViewModels
{
    public class SponsorsFormViewModel : BaseViewModel
    {
        private Reward reward;
        private INavigation navigation;
        private Page view;
        private string sponsorName;
        private string description;
        private int? greenCoins;
        private string name;
        private bool newReward;
        private event EventHandler Starting = delegate { };
        public SponsorsFormViewModel(INavigation navigation, Page view, Reward reward)
        {
            IsBusy = true;
            this.reward = reward;
            newReward = false;
            this.navigation = navigation;
            this.view = view;
            GreenCoins = reward.GreenCoins;
            Description = reward.Description;
            SponsorName = reward.SponsorName;
            IsBusy = false;
        }
        public SponsorsFormViewModel(INavigation navigation, Page view)
        {
            IsBusy = true;
            newReward = true;
            this.navigation = navigation;
            this.view = view;
            IsBusy = false;
        }
        public int? GreenCoins
        {
            get => greenCoins;
            set => SetProperty(ref greenCoins, value);
        }
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        public string SponsorName
        {
            get => sponsorName;
            set => SetProperty(ref sponsorName, value);
        }        
        public AsyncCommand OnSubmitButtonCommand => new AsyncCommand(OnSubmit);

        private async Task OnSubmit()
        {
            IsBusy = true;
            if (Description == null)
            {
                await view.DisplayAlert("Description field empty!", "please enter a value first", "OK");
                IsBusy = false;
                return;
            }
            if (Name == null)
            {
                await view.DisplayAlert("Name field empty!", "please enter a value first", "OK");
                IsBusy = false;
                return;
            }
            if (SponsorName == null)
            {
                await view.DisplayAlert("SponsorName field empty!", "please enter a value first", "OK");
                IsBusy = false;
                return;
            }
            if (GreenCoins == null || GreenCoins < 0)
            {
                await view.DisplayAlert("GreenCoins field negative or empty!", "please enter a value first", "OK");
                IsBusy = false;
                return;
            }
            try
            {                
                if (newReward)
                {                   
                    reward = new Reward(navigation,view)
                    {
                        Description = Description,
                        SponsorName = SponsorName,
                        GreenCoins = (int) GreenCoins,
                        Name = Name
                    };
                    if (await RewardsUtil.Instance().CreateReward(reward))
                    {
                        await view.DisplayAlert("Reward created successfully", "", "OK");
                        IsBusy=false;
                        await navigation.PopModalAsync();
                        return;
                    }
                    await view.DisplayAlert("Error while creating reward","something went wrong", "OK");
                    IsBusy = false;
                }
                else
                {
                    reward.SponsorName=SponsorName;
                    reward.Description = Description;
                    reward.Name = Name;
                    reward.GreenCoins = (int)GreenCoins;
                    if (await RewardsUtil.Instance().EditReward(reward))
                    {
                        await view.DisplayAlert("Reward edited successfully", "", "OK");
                        IsBusy = false;
                        await navigation.PopModalAsync();
                        await view.DisplayAlert("Error while editing reward", "something went wrong", "OK");
                        return;
                    }                    
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await view.DisplayAlert("Error while editing or creating reward", "something went wrong", "OK");
                IsBusy = false;
            }
        }

    }
}

