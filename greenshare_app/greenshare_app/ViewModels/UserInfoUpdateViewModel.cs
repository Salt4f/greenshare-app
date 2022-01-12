using greenshare_app.Models;
using greenshare_app.Utils;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace greenshare_app.ViewModels
{
    public class UserInfoUpdateViewModel : BaseViewModel
    {
        private INavigation navigation;
        private Page view;
        private User user;
        private string nickName;
        private string description;

        public AsyncCommand OnSubmitButtonCommand => new AsyncCommand(OnSubmit);
        public string NickName
        {
            get => nickName;
            set => SetProperty(ref nickName, value);
        }
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public UserInfoUpdateViewModel(INavigation navigation, Page view, User user)
        {
            this.navigation = navigation;
            this.view = view;
            this.user = user;
            NickName = this.user.NickName;
            Description = this.user.Description;
        }

        public async Task OnSubmit()
        {
            IsBusy = true;
            if (NickName.Length <= 5 || NickName.Length > 30)
            {
                await view.DisplayAlert("NickName field not valid", "please make sure you entered a nickname with more than 5 and less than 30 characters", "OK");
                IsBusy = false;
                return;
            }
            try
            {
                if (await UserInfoUtil.Instance().EditUser(user))
                {
                    await view.DisplayAlert("user edited successfully", "", "OK");
                    IsBusy=false;
                    await navigation.PopModalAsync();
                }
            }
            catch (Exception)
            {
                await view.DisplayAlert("error while editing user", "something went wrong", "OK");
                IsBusy = false;
            }
        }
    }
}
