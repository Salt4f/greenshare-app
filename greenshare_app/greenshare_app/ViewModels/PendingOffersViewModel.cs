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
    internal class PendingOffersViewModel : BaseViewModel
    {
        public PendingOffersViewModel(INavigation navigation, Page view)
        {
            Title = "Pending Offers";

        }
    }
}
