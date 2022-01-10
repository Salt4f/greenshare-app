using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace greenshare_app.Controls
{
    public class CustomMap : Map
    {
        public IList<CustomPin> CustomPins { get; set; }
    }
}
