using Newtonsoft.Json;
using System;
using Xamarin.Forms;

namespace greenshare_app.Converters
{
    public class ColorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Color);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string col = (string) reader.Value;
            return Color.FromHex(col);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Color col = (Color) value;
            writer.WriteValue(col.ToHex());
        }
    }

}