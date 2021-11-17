using Newtonsoft.Json;
using Xamarin.Forms;

namespace greenshare_app.Models
{
    public class Tag
    {
        [JsonProperty(PropertyName = "color")]
        public Color Color { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
      
    }
}