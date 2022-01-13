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

        public MapsPageViewModel(INavigation navigation, Page view, CustomMap MyMap)
        {
            this.navigation = navigation;
            this.view = view;
            this.distanceValue = 100;
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
            var cards = await PostRetriever.Instance().GetOffers(loc, distanceValue, quantity:50);
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

            var cards2 = await PostRetriever.Instance().GetRequests(loc, distanceValue, quantity:50);
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
    }
}
