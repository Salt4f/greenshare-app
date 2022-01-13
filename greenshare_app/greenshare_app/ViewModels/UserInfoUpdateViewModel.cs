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
using greenshare_app.Text;

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
                await view.DisplayAlert(Text.Text.NicknameTooLong + "/" + Text.Text.NicknameTooShort, Text.Text.PleaseEnterALongerNickname + "/" + Text.Text.PleaseEnterAShorterNickname, "OK");
                IsBusy = false;
                return;
            }
            try
            {
                user.NickName = NickName;
                user.Description = Description;
                if (await UserInfoUtil.Instance().EditUser(user))
                {
                    await view.DisplayAlert(Text.Text.UserEditedSuccessfully, "", "OK");
                    IsBusy=false;
                    await navigation.PopModalAsync();
                }
            }
            catch (Exception)
            {
                await view.DisplayAlert(Text.Text.ErrorWhileEditingUser, Text.Text.SomethingWentWrong, "OK");
                IsBusy = false;
            }
        }
    }
}
