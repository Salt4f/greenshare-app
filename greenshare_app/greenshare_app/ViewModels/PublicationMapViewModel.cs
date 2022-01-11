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
        Geocoder geocoder = new Geocoder();

        public AsyncCommand<MapClickedEventArgs> OnMapClickedCommand => new AsyncCommand<MapClickedEventArgs>(OnMapClicked);

        private Task OnMapClicked(MapClickedEventArgs eventData)
        {
            Location l = new Location(eventData.Position.Latitude, eventData.Position.Longitude);
            throw new NotImplementedException();
        }

        public PublicationMapViewModel(INavigation navigation, Page view, Xamarin.Forms.Maps.Map MyMap)
        {
            this.navigation = navigation;
            this.view = view;
            PositionMap(MyMap);
        }
        private async void PositionMap(Xamarin.Forms.Maps.Map MyMap)
        {
            var loc = await Geolocation.GetLocationAsync();
            MyMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Position(loc.Latitude, loc.Longitude), Distance.FromKilometers(10)
                    ));
        }
    }
}
