using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(greenshare_app.Controls.BorderlessEntry), typeof(greenshare_app.iOS.BorderlessEntryRenderer))]

namespace greenshare_app.iOS.Renderers
{
    public class BorderlessEditorRenderer : EditorRenderer
    {
        public static void Init() { }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Control.Layer.BorderWidth = 0;
        }
    }
}