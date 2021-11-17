using MvvmHelpers;
using greenshare_app.Utils;
using greenshare_app.Views;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace greenshare_app.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public ProfilePageViewModel(INavigation navigation, Page view)
        {
            Title = "Perfil";
            this.navigation = navigation;
            this.view = view;
        }

        public Command UserInfoCommand => new Command(OnUserInfoClicked);
        private INavigation navigation;
        private Page view;
        private async void OnUserInfoClicked()
        {
            await navigation.PushModalAsync(new Views.MainViewPages.RequestsPage());
        }
    }
}