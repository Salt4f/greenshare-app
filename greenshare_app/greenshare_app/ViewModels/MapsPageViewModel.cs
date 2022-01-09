using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using greenshare_app.Models;
using greenshare_app.Utils;
using MvvmHelpers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using MvvmHelpers.Commands;

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

        public MapsPageViewModel(INavigation navigation, Page view, Xamarin.Forms.Maps.Map MyMap)
        {
            this.navigation = navigation;
            this.view = view;
            PositionMap(MyMap);
            AddPins(MyMap);
        }
        private async void PositionMap(Xamarin.Forms.Maps.Map MyMap) {
            var loc = await Geolocation.GetLocationAsync();
            MyMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Position(loc.Latitude, loc.Longitude), Distance.FromKilometers(10)
                    ));
        }
        private async void AddPins(Xamarin.Forms.Maps.Map MyMap)
        {
            var loc = await Geolocation.GetLocationAsync();
            var cards = await PostRetriever.Instance().GetOffers(loc, 1000);

            foreach (var card in cards)
            {
                var offer = await PostRetriever.Instance().GetOffer(card.Id);
                var pin = new Pin();
                pin.Position = new Position(offer.Location.Latitude, offer.Location.Longitude);
                pin.Label = offer.Name;

                MyMap.Pins.Add(pin);
            }

            cards = await PostRetriever.Instance().GetRequests(loc, 1000);

            foreach (var card in cards)
            {
                var offer = await PostRetriever.Instance().GetRequest(card.Id);
                var pin = new Pin();
                pin.Position = new Position(offer.Location.Latitude, offer.Location.Longitude);
                pin.Label = offer.Name;

                MyMap.Pins.Add(pin);
            }
        }
    }
}
