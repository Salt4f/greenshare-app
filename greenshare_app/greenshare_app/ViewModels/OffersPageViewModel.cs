using greenshare_app.Utils;
using greenshare_app.Views;
using greenshare_app.Views.MainViewPages;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Threading.Tasks;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace greenshare_app.ViewModels
{
    public class OffersPageViewModel : BaseViewModel
    {
        public OffersPageViewModel(INavigation navigation, Page view)
        {
            Title = "Ofertes";
            this.navigation = navigation;
            this.view = view;
        }

        private Page view;
        private INavigation navigation;

        public AsyncCommand ProfileButtonCommand => new AsyncCommand(OnProfileButton);
        public AsyncCommand LogoutButtonCommand => new AsyncCommand(OnLogoutButton);

        public async Task OnProfileButton()
        {
            await navigation.PushModalAsync(new ProfilePage());
        }

        public async Task OnLogoutButton()
        {
            await Auth.Instance().Logout();
            Application.Current.MainPage = new LoginView();
        }

    }
}