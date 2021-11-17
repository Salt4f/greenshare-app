
using greenshare_app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace greenshare_app.Views.MainViewPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestsPage : ContentPage
    {
        public RequestsPage()
        {
            InitializeComponent();
            BindingContext = new RequestsPageViewModel(Navigation, this);
        }
    }
}