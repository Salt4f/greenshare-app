using greenshare_app.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using greenshare_app.Utils;
using System.Text;

namespace greenshare_app.ViewModels
{
    internal class MyPostsViewModel : BaseViewModel
    {
        public MyPostsViewModel(INavigation navigation, Page view)
        {
            Title = "My Posts";
        }
    }
}
