using Xamarin.Forms;
using System;

namespace greenshare_app
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //MainPage = new Views.LoginView();
            MainPage = new Views.MainViewPages.NewPublication();
        }

        protected override void OnStart()
        { 
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
