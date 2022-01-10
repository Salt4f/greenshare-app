using System;
using System.Collections.Generic;
using System.Text;

namespace greenshare_app.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ItemId { get; set; }
        public string Message { get; set; }
        public int ReporterId { get; set; }
        public bool Solved { get; set; }
    }
}
