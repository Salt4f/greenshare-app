using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace greenshare_app.Models
{
    class PostCardModel
    {
        private string name;
        private IEnumerable<Tag> tags;
        private Image icon;
        private string author;
        public PostCardModel(string name, IEnumerable<Tag> tags, Image icon, string author)
        {
            this.name = name;
            this.tags = tags;
            this.icon = icon;
            this.author = author;
        }
        
        
    }
}
