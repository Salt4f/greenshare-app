using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using MvvmHelpers;

namespace greenshare_app.Models
{
    public class PostCard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableRangeCollection<Tag> Tags { get; set; }
        public Image Icon { get; set; }
        public string Author { get; set; }

      
        
    }
}
