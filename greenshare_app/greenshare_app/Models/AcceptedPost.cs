﻿using System;
using System.Collections.Generic;
using System.Text;

namespace greenshare_app.Models
{
    public class AcceptedPost
    {
        public int OfferId { get; set; }
        public int RequestId { get; set; }
        public string OfferTitle { get; set; }
        public string OfferUserName { get; set; }
        public string UserName { get; set; }
    }
}
