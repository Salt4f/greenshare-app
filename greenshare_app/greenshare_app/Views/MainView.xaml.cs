using greenshare_app.ViewModels;
using Xamarin.Forms;

namespace greenshare_app.Views
{
    public partial class MainView : Shell
    {
        public MainView()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

    }
}
