using Newtonsoft.Json;
using System;
using Xamarin.Essentials;

namespace greenshare_app.Converters
{
    public class LocationConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Location);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string[] loc = ((string) reader.Value).Split(';');
            return new Location(double.Parse(loc[0]), double.Parse(loc[1]));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Location loc = (Location) value;

            writer.WriteValue(loc.Latitude.ToString() + ";" + loc.Longitude.ToString() );
        }
    }

}