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
using System.Linq;
using greenshare_app.Views.MainViewPages;
using System.Threading;

namespace greenshare_app.ViewModels
{
    class MapsPageViewModel : BaseViewModel
    {
        private INavigation navigation;
        private Page view;
        private int distanceValue;
        private CustomMap MyMap;
        Geocoder geocoder = new Geocoder();

        public int DistanceValue
        {
            get => distanceValue;
            set => SetProperty(ref distanceValue, value);
        }

        public AsyncCommand<MapClickedEventArgs> OnMapClickedCommand => new AsyncCommand<MapClickedEventArgs>(OnMapClicked);

        private Task OnMapClicked(MapClickedEventArgs eventData)
        {
            Location l = new Location(eventData.Position.Latitude, eventData.Position.Longitude);
            throw new NotImplementedException();
        }

        public MapsPageViewModel(INavigation navigation, Page view, CustomMap myMap)
        {
            this.navigation = navigation;
            this.view = view;
            distanceValue = 100;
            MyMap = myMap;
            MyMap.CustomPins = new List<CustomPin>();
            PositionMap();
            AddPins();
        }
        private async void PositionMap()
        {
            var loc = await Geolocation.GetLocationAsync();
            MyMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Position(loc.Latitude, loc.Longitude), Distance.FromKilometers(10)
                    ));
        }
        private async void AddPins()
        {
            var loc = await Geolocation.GetLocationAsync();
            var cards = await PostRetriever.Instance().GetOffers(loc, DistanceValue, quantity: 50);
            if (cards != null)
            {
                foreach (var card in cards)
                {
                    var offer = await PostRetriever.Instance().GetOffer(card.Id);
                    var pin = new CustomPin();
                    pin.Position = new Position(offer.Location.Latitude, offer.Location.Longitude);
                    IEnumerable<string> addresses = await geocoder.GetAddressesForPositionAsync(pin.Position);
                    pin.Address = addresses.FirstOrDefault();
                    pin.Label = offer.Name;
                    pin.Name = offer.Name;
                    pin.Url = offer.Id;
                    pin.InfoWindowClicked += async (s, args) =>
                    {
                        Offer offer2 = await PostRetriever.Instance().GetOffer(pin.Url);
                        if (offer == null) await view.DisplayAlert(Text.Text.ErrorWhileRetrievingSelectedOffer, Text.Text.OfferNotFound, "OK");
                        else
                        {
                            var view = new ViewPost(offer2);
                            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
                            await navigation.PushModalAsync(view);
                        }
                    };

                    MyMap.CustomPins.Add(pin);
                    MyMap.Pins.Add(pin);
                }
            }

            var cards2 = await PostRetriever.Instance().GetRequests(loc, DistanceValue, quantity: 50);
            if (cards2 != null)
            {
                foreach (var card in cards2)
                {
                    var offer = await PostRetriever.Instance().GetRequest(card.Id);
                    var pin = new CustomPin();
                    pin.Position = new Position(offer.Location.Latitude, offer.Location.Longitude);
                    IEnumerable<string> addresses = await geocoder.GetAddressesForPositionAsync(pin.Position);
                    pin.Address = addresses.FirstOrDefault();

                    pin.Label = offer.Name;
                    pin.Name = offer.Name;
                    pin.Url = offer.Id;
                    pin.InfoWindowClicked += async (s, args) =>
                     {
                         Request offer2 = await PostRetriever.Instance().GetRequest(pin.Url);
                         if (offer == null) await view.DisplayAlert(Text.Text.ErrorWhileRetrievingSelectedOffer, Text.Text.OfferNotFound, "OK");
                         else
                         {
                             var view = new ViewPost(offer2);
                             var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
                             await navigation.PushModalAsync(view);
                         }
                     };

                    MyMap.CustomPins.Add(pin);
                    MyMap.Pins.Add(pin);
                }
            }
        }

        public AsyncCommand OnSearchButtonCommand => new AsyncCommand(OnSearch);

        private async Task OnSearch()
        {
            IsBusy = true;
            MyMap.CustomPins.Clear();
            MyMap.Pins.Clear();
            AddPins();
            IsBusy = false;
        }
    }
}
