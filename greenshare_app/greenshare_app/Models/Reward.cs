using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace greenshare_app.Models
{
    class Reward
    {
        public int Id { get; set; }
        public string SponsorName { get; set; }
        public int GreenCoins { get; set; }
        public string GreenCoinsText { get; set; }
        public string Description { get; set; }
    }
}
