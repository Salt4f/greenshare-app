using MvvmHelpers;
using greenshare_app.Utils;
using greenshare_app.Views;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;
using greenshare_app.Views.MainViewPages.ProfileViewPages;

namespace greenshare_app.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel
    {
        private event EventHandler Starting = delegate { };
        public ProfilePageViewModel(INavigation navigation, Page view)
        {
            Title = "Perfil";
            this.navigation = navigation;
            this.view = view;
            userName = string.Empty;
            IsBusy = true;
            Starting += OnStart;
            Starting(this, EventArgs.Empty);
        }

        private string userName;
        public string UserName {
            get => userName;
            set => SetProperty(ref userName, value);
        }


        private async void OnStart(object sender, EventArgs args)
        {
            try
            {

            }
            catch (Exception e)
            {
                var type = e.GetType();
                var error = e.Message;
                IsBusy = false;
                await view.DisplayAlert("Internal Server Error", "Something went wrong", "OK");
            }
            IsBusy = false;
        }

        public Command UserInfoCommand => new Command(OnUserInfoClicked);
        private INavigation navigation;
        private Page view;
        private async void OnUserInfoClicked()
        {
            await navigation.PushModalAsync(new UserInfoPage());
        }
    }
}