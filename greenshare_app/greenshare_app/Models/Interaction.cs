using System;
using System.Collections.Generic;
using System.Text;

namespace greenshare_app.Models
{
    public class Interaction
    {
        public string PostName { get; set; }
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string Status { get; set; }
        public bool IsVisible { get; set; }
    }
}
