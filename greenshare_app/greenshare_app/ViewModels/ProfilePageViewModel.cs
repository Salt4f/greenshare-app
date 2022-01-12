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
using greenshare_app.Text;

namespace greenshare_app.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel
    {
        private event EventHandler Starting = delegate { };
        private int userId;
        private User user;
        private bool ownPage;
        private bool isReportable;
        public ProfilePageViewModel(INavigation navigation, Page view)
        {
            Title = Text.Text.Profile;
            this.navigation = navigation;
            this.view = view;
            OwnPage = true;
            nickName = string.Empty;
            IsBusy = true;
            Starting += OnStart;
            Starting(this, EventArgs.Empty);
        }
        public ProfilePageViewModel(INavigation navigation, Page view, int userId)
        {
            Title = Text.Text.Profile;
            this.navigation = navigation;
            this.view = view;            
            this.userId = userId;
            OwnPage = false;
            nickName = string.Empty;
            IsBusy = true;
            Starting += OnStart;
            Starting(this, EventArgs.Empty);
            
        }

        private string nickName;
        private double rating;
        public string NickName {
            get => nickName;
            set => SetProperty(ref nickName, value);
        }
        public bool IsReportable
        {
            get => isReportable;
            private set => SetProperty(ref isReportable, value);
        }

        public double Rating
        {
            get => rating;
            set => SetProperty(ref rating, value);

        }
        public bool OwnPage
        {
            get => ownPage;
            private set => SetProperty(ref ownPage, value);
        }

        public bool NotAdminNotOwnPage
        {
            get => !IsAdmin && !OwnPage;
        }
        public bool IsAdminOwnPage
        {
            get => IsAdmin && OwnPage;
        }
        public bool IsAdminNotOwnPage
        {
            get => IsAdmin && !OwnPage;
        }
        public bool IsAdmin
        {
            get => isAdmin;
            private set => SetProperty(ref isAdmin, value);
        }

        private async void OnStart(object sender, EventArgs args)
        {
            IsReportable = !OwnPage;
            try
            {
                IsAdmin = await Auth.Instance().IsAdmin();
                if (OwnPage) user = await UserInfoUtil.Instance().GetUserInfo();
                else user = await UserInfoUtil.Instance().GetUserInfo(userId);
                NickName = user.NickName;
                Rating = user.AverageValoration;
            }
            catch (Exception e)
            {
                var type = e.GetType();
                var error = e.Message;
                IsBusy = false;
                await view.DisplayAlert(Text.Text.InternalServerError, Text.Text.SomethingWentWrong, "OK");
            }
            IsBusy = false;
        }
        
        public AsyncCommand OnAdminFrameCommand => new AsyncCommand(OnAdmin);

        public AsyncCommand UserInfoCommand => new AsyncCommand(OnUserInfo);
        public AsyncCommand OnRewardsButtonCommand => new AsyncCommand(OnRewards);
        public AsyncCommand UserPostsCommand => new AsyncCommand(OnUserPostsButton);
        public AsyncCommand UserLogOutCommand => new AsyncCommand(OnLogOutButton);
        public AsyncCommand UserIncomingInteractionsCommand => new AsyncCommand(OnIncomingInteractionsButton);
        public AsyncCommand UserOutgoingInteractionsCommand => new AsyncCommand(OnOutgoingInteractionsButton);
        public AsyncCommand OnReportButtonCommand => new AsyncCommand(OnReportButton);
        public AsyncCommand OnBanButtonCommand => new AsyncCommand(OnBanButton);


        //TODO: RATE PAGE PER USER
        //public AsyncCommand OnRateButtonCommand => new AsyncCommand(OnRateButton);

        private INavigation navigation;
        private Page view;
        private bool isAdmin;

        private async Task OnUserInfo()
        {
            IsBusy = true;
            if (OwnPage)
            {
                Tuple<int, string> session = await Auth.Instance().GetAuth();
                await navigation.PushModalAsync(new UserInfoPage(session.Item1, OwnPage));
            }
            else await navigation.PushModalAsync(new UserInfoPage(userId, OwnPage));
            IsBusy = false;
        }
        private async Task OnRewards()
        {
            IsBusy = true;
            await navigation.PushModalAsync(new RewardsPage(user));
            IsBusy = false;
        }
        private async Task OnUserPostsButton()
        {
            IsBusy = true;
            await navigation.PushModalAsync(new UserPublicationsPage());
            IsBusy = false;
        }
        private async Task OnLogOutButton()
        {
            await Auth.Instance().Logout();
            Application.Current.MainPage = new LoginView();
        }
        private async Task OnIncomingInteractionsButton()
        {
            IsBusy = true;
            await navigation.PushModalAsync(new IncomingInteractionsPage());
            IsBusy = false;
        }
        private async Task OnOutgoingInteractionsButton()
        {   
            IsBusy = true;
            await navigation.PushModalAsync(new OutgoingInteractionsPage());
            IsBusy = false;
        }

        private async Task OnAdmin()
        {
            IsBusy = true;
            await navigation.PushModalAsync(new AdminPage());
            IsBusy = false;
        }

        private async Task OnReportButton()
        {
            IsBusy = true;
            await navigation.PushModalAsync(new ReportPage(typeof(User), userId));
            IsBusy = false;
        }

        private async Task OnBanButton()
        {
            //TODO funcion de baneo
        }

    }
}