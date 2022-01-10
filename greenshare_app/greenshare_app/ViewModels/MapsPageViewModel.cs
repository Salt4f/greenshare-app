using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using greenshare_app.Models;
using greenshare_app.Utils;
using MvvmHelpers;
using Xamarin.Essentials;
using MvvmHelpers.Commands;
using greenshare_app.Controls;
using Xamarin.Forms.Maps;
using Xamarin.Forms;

namespace greenshare_app.ViewModels
{
    class MapsPageViewModel : BaseViewModel
    {
        private INavigation navigation;
        private Page view;

        public AsyncCommand<MapClickedEventArgs> OnMapClickedCommand => new AsyncCommand<MapClickedEventArgs>(OnMapClicked);

        private Task OnMapClicked(MapClickedEventArgs eventData)
        {
            Location l = new Location(eventData.Position.Latitude, eventData.Position.Longitude);
            throw new NotImplementedException();
        }

        public MapsPageViewModel(INavigation navigation, Page view, CustomMap MyMap)
        {
            this.navigation = navigation;
            this.view = view;
            MyMap.CustomPins = new List<CustomPin>();
            PositionMap(MyMap);
            AddPins(MyMap);
        }
        private async void PositionMap(CustomMap MyMap)
        {
            var loc = await Geolocation.GetLocationAsync();
            MyMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Position(loc.Latitude, loc.Longitude), Distance.FromKilometers(10)
                    ));
        }
        private async void AddPins(CustomMap MyMap)
        {
            var loc = await Geolocation.GetLocationAsync();
            var cards = await PostRetriever.Instance().GetOffers(loc, 100, quantity:50);
            if (cards != null)
            {
                foreach (var card in cards)
                {
                    var offer = await PostRetriever.Instance().GetOffer(card.Id);
                    var pin = new CustomPin();
                    pin.Position = new Position(offer.Location.Latitude, offer.Location.Longitude);
                    pin.Label = offer.Name;

                    MyMap.CustomPins.Add(pin);
                }
            }

            var cards2 = await PostRetriever.Instance().GetRequests(loc, 100, quantity:50);
            if (cards2 != null)
            {
                foreach (var card in cards2)
                {
                    var offer = await PostRetriever.Instance().GetRequest(card.Id);
                    var pin = new CustomPin();
                    pin.Position = new Position(offer.Location.Latitude, offer.Location.Longitude);
                    pin.Label = offer.Name;

                    MyMap.CustomPins.Add(pin);
                }
            }
        }
    }
}
