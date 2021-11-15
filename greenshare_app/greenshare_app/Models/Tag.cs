using Xamarin.Forms;

namespace greenshare_app.Models
{
    class Tag
    {
        public Color Color { get; set; }
        public string Name { get; set; }

        public Tag(Color color, string name)
        {
            Color = color;
            Name = name;
        }
    }
}