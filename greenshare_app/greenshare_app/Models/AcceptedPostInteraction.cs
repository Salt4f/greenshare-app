using System;
using System.Collections.Generic;
using System.Text;

namespace greenshare_app.Models
{
    public class AcceptedPostInteraction
    {
        public int OfferId { get; set; }
        public int RequestId { get; set; }
        public string OfferName { get; set; }
        public string OfferUserName { get; set; }
        public string UserName { get; set; }
    }
}
