using System;
using System.Collections.Generic;
using System.Text;

namespace greenshare_app.Models
{
    class Report
    {
        public int Id { get; set; }
        public string UserOrPost { get; set; }
        public string UserOrPostReported { get; set; }
        public string UserReporting { get; set; }
        public string Description { get; set; }

    }
}
