using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;

namespace greenshare_app.Controls
{
    [Preserve(AllMembers = true)]
    public class CustomPin : Pin
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
