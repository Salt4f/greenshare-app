﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace greenshare_app.Models
{
    public class Post
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime TerminateAt { get; set; }
        public Location Location { get; set; }
        public bool Active { get; set; }
        public int OwnerId { get; set; }
        public int EcoImpact { get; set; }
        public IList<Tag> Tags { get; set; }
    }
    public enum Status
    {
        Idle,
        Pending,
        Accepted,
        Rejected
    };
}
