using Xamarin.Forms;

namespace greenshare_app.Models
{
    class Tag
    {
        private Color color;
        private string name;

        public Tag(Color color, string name)
        {
            this.color = color;
            this.name = name;
        }
    }
}