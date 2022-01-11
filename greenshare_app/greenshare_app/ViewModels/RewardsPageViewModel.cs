using greenshare_app.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using greenshare_app.Utils;
using greenshare_app.Views.MainViewPages;
using Xamarin.Essentials;
using System.Text;

namespace greenshare_app.ViewModels
{
    public class RewardsPageViewModel : BaseViewModel
    {
        private INavigation navigation;
        private Page view;

        private event EventHandler Starting = delegate { };
        public RewardsPageViewModel(INavigation navigation, Page view)
        {
            this.navigation = navigation;
            this.view = view;
            Starting += OnStart;
            Starting(this, EventArgs.Empty);
        }

        private async void OnStart(object sender, EventArgs e)
        {

        }
    }
}
