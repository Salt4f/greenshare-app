using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace greenshare_app.Models
{
    public class Offer : Post
    {
        public Image Icon { get; set; }
        public IEnumerable<Image> Photos { get; set; }

        public Offer(string name, string description, DateTime createdAt, DateTime terminateAt, Location location, bool active, int ownerId, int ecoImpact, Image icon, IEnumerable<Image> photos) :
            base(name, description, createdAt, terminateAt, location, active, ownerId, ecoImpact)
        {
            Icon = icon;
            Photos = photos;
        }
    }
}
