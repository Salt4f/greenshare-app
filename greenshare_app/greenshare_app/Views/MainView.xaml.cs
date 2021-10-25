using greenshare_app.ViewModels;
using Xamarin.Forms;

namespace greenshare_app.Views
{
    public partial class MainView : TabbedPage
    {
        public MainView()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

    }
}
