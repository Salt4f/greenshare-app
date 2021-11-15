using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace greenshare_app.Models
{
    public class Post
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime TerminateAt { get; set; }
        public Location Location { get; set; }
        public bool Active { get; set; }
        public int OwnerId { get; set; }
        public int EcoImpact { get; set; }

       public Post(string name, string description, DateTime createdAt, DateTime terminateAt, Location location, bool active, int ownerId, int ecoImpact)
       {
            Name = name;
            Description = description;
            CreatedAt = createdAt;
            TerminateAt = terminateAt;
            Location = location;
            Active = active;
            OwnerId = ownerId;
            EcoImpact = ecoImpact;
       }
    }
}
