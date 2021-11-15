using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace greenshare_app.Models
{
    public class Request : Post
    {
        public Request(string name, string description, DateTime createdAt, DateTime terminateAt, Location location, bool active, int ownerId, int ecoImpact) :
            base(name, description, createdAt, terminateAt, location, active, ownerId, ecoImpact)
        {
        }
    }
}
