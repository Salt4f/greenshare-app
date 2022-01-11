using System;
using System.Collections.Generic;
using System.Text;

namespace greenshare_app.Models
{
    public class PostStatus : PostCard
    {
        public bool IsOffer { get; set; }
        public string Status { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }

    }
}
