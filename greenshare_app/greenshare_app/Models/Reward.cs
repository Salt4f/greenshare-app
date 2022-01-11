using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace greenshare_app.Models
{
    public class Reward
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "sponsorName")]
        public string SponsorName { get; set; }

        [JsonProperty(PropertyName = "greenCoins")]
        public int GreenCoins { get; set; }

        public string GreenCoinsText { get; set; }

        
    }
}
