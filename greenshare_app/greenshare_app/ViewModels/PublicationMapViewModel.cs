using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using greenshare_app.Models;
using greenshare_app.Utils;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Essentials;

namespace greenshare_app.ViewModels
{
    class PublicationMapViewModel : BaseViewModel
    {
        private INavigation navigation;
        private Page view;
        private static Position location;
        private static bool selectedLocation;
        private static Pin pin;



        public AsyncCommand<MapClickedEventArgs> OnMapClickedCommand => new AsyncCommand<MapClickedEventArgs>(OnMapClicked);
        public AsyncCommand OnAcceptedButtonCommand => new AsyncCommand(OnAcceptedButton);
        public AsyncCommand OnCancelButtonCommand => new AsyncCommand(OnCancelButton);

        private async Task OnMapClicked(MapClickedEventArgs eventData)
        {
            Position l = new Position(eventData.Position.Latitude, eventData.Position.Longitude);
            pin.Position = l;
            location = l;
        }
        public static Tuple<bool, Location> GetLocation()
        {
            var res = new Tuple<bool, Location>(selectedLocation, new Location(location.Latitude,location.Longitude));
            selectedLocation = false;
            return res;
        }
        public PublicationMapViewModel(INavigation navigation, Page view, Xamarin.Forms.Maps.Map MyMap)
        {
            this.navigation = navigation;
            this.view = view;
            selectedLocation = false;
            pin = new Pin() { 
                Label = "Location para la publicación"
            };
            location = new Position();
            PositionMap(MyMap);
        }

        public async Task OnAcceptedButton()
        {
            selectedLocation = true;
            await navigation.PopModalAsync();   //esto se carga la página
        }
        public async Task OnCancelButton()
        {
            selectedLocation = false;
            await navigation.PopModalAsync();   //esto se carga la página
        }
        private async void PositionMap(Xamarin.Forms.Maps.Map MyMap)
        {
            var loc = await Geolocation.GetLocationAsync();
            MyMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Position(loc.Latitude, loc.Longitude), Distance.FromKilometers(10)
                    ));
            pin.Position = new Position(loc.Latitude,loc.Longitude);
            location = new Position(loc.Latitude, loc.Longitude);
            MyMap.Pins.Add(pin);
            
        }

    }
}
