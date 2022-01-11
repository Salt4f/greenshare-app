using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using greenshare_app.Models;
using greenshare_app.Utils;
using MvvmHelpers;
using Xamarin.Essentials;
using MvvmHelpers.Commands;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using System.Linq;

namespace greenshare_app.ViewModels
{
    class PublicationMapViewModel : BaseViewModel
    {
        private INavigation navigation;
        private Page view;
        private static Location location;
        private static bool selectedLocation;

        Geocoder geocoder = new Geocoder();

        public AsyncCommand<MapClickedEventArgs> OnMapClickedCommand => new AsyncCommand<MapClickedEventArgs>(OnMapClicked);

        private Task OnMapClicked(MapClickedEventArgs eventData)
        {
            Location l = new Location(eventData.Position.Latitude, eventData.Position.Longitude);
            
            throw new NotImplementedException();
        }
        public static Tuple<bool, Location> GetLocation()
        {
            var res = new Tuple<bool, Location>(selectedLocation, location);
            selectedLocation = false;
            return res;
        }
        public PublicationMapViewModel(INavigation navigation, Page view, Xamarin.Forms.Maps.Map MyMap)
        {
            this.navigation = navigation;
            this.view = view;
            selectedLocation = false;
            PositionMap(MyMap);
        }
        
        public async Task<bool> OnAcceptedButton()
        {
            selectedLocation = true;
            await navigation.PopModalAsync();   //esto se carga la página
        }
        public async Task<bool> OnCancelButton()
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
        }

        internal Task GetSelectedLocation()
        {
            throw new NotImplementedException();
        }

    }
}
