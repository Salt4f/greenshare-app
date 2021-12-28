﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using greenshare_app.Models;
using greenshare_app.Utils;
using MvvmHelpers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace greenshare_app.ViewModels
{
    class MapsPageViewModel : BaseViewModel
    {
        private INavigation navigation;
        private Page view;
        public MapsPageViewModel(INavigation navigation, Page view, Xamarin.Forms.Maps.Map MyMap)
        {
            this.navigation = navigation;
            this.view = view;
            AddPins(MyMap);
        }
        private async void AddPins(Xamarin.Forms.Maps.Map MyMap)
        {
            var loc = await Geolocation.GetLocationAsync();
            var cards = await PostRetriever.Instance().GetOffers(loc);

            foreach (var card in cards)
            {
                var offer = await PostRetriever.Instance().GetOffer(card.Id);
                var pin = new Pin();
                pin.Position = new Position(offer.Location.Latitude, offer.Location.Longitude);
                pin.Label = offer.Name;

                MyMap.Pins.Add(pin);
            }
        }
    }
}
