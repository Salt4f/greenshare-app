using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace greenshare_app.Models
{
    class PostCard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public Image Icon { get; set; }
        public string Author { get; set; }

      
        
    }
}
