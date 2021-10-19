using Xamarin.Forms;
using greenshare_app.ViewModels;

namespace greenshare_app
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

        }

        protected override void OnStart()
        {
            MainPage = new Views.LoginView();            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
