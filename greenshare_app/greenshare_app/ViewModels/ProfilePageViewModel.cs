using MvvmHelpers;
using greenshare_app.Utils;
using greenshare_app.Views;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;
using greenshare_app.Views.MainViewPages.ProfileViewPages;
using greenshare_app.Models;
using greenshare_app.Views.MainViewPages;

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
            nickName = string.Empty;
            IsBusy = true;
            Starting += OnStart;
            Starting(this, EventArgs.Empty);
        }

        private string nickName;
        public string NickName {
            get => nickName;
            set => SetProperty(ref nickName, value);
        }


        private async void OnStart(object sender, EventArgs args)
        {
            try
            {
                User user = await UserInfoUtil.Instance().GetUserInfo();
                NickName = user.NickName;
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

        public AsyncCommand UserInfoCommand => new AsyncCommand(OnUserInfoButton);
        public AsyncCommand UserPostsCommand => new AsyncCommand(OnUserPostsButton);
        public AsyncCommand UserLogOutCommand => new AsyncCommand(OnLogOutButton);
        public AsyncCommand UserPendingOffersCommand => new AsyncCommand(OnPendingOffersButton);
        public AsyncCommand UserPendingRequestsCommand => new AsyncCommand(OnPendingRequestsButton);

        private INavigation navigation;
        private Page view;
        private async Task OnUserInfoButton()
        {
            await navigation.PushModalAsync(new UserInfoPage());
        }
        private async Task OnUserPostsButton()
        {
            await navigation.PushModalAsync(new OffersPage());
        }
        private async Task OnLogOutButton()
        {
            await Auth.Instance().Logout();
            Application.Current.MainPage = new LoginView();
        }
        private async Task OnPendingOffersButton()
        {
            await navigation.PushModalAsync(new PendingOffersPage());
        }
        private async Task OnPendingRequestsButton()
        {
            await navigation.PushModalAsync(new PendingRequestsPage());
        }
    }
}