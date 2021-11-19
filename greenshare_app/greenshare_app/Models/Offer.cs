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

    }
}
