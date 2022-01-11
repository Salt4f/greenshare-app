using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace greenshare_app.Models
{
    public class Offer : Post
    {
        public byte[] Icon { get; set; }
        public IList<byte[]> Photos { get; set; }

    }
}
