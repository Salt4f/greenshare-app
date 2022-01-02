using System;
using System.Collections.Generic;
using System.Text;

namespace greenshare_app.Models
{
    public class PendingPost
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public int PostId { get; set; }
        public string PostName { get; set; }
        public int OwnPostId { get; set; }

    }
}
