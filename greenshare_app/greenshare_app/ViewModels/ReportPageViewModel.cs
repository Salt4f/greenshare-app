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
    public class ReportPageViewModel : BaseViewModel
    {
        private INavigation navigation;
        private Page view;
        private Type type;
        private int id;
        private string message;

        public AsyncCommand OnReportButtonCommand => new AsyncCommand(OnReport);

        public ReportPageViewModel(INavigation navigation, Page view, Type type, int id)
        {
            Title = "Report page";
            this.navigation = navigation;
            this.view = view;
            Id = id;
            Type = type;
        }
        public Type Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }
        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }
        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }
        private async Task OnReport()
        {
            if (Message != null)
            {
                IsBusy = true;
                await ReportUtil.Instance().PostReport(Message, Type, Id);
                IsBusy = false;
                await navigation.PopModalAsync();
            }
            else
            {
                await view.DisplayAlert("Please enter a report message first", "", "OK");
            }
        }


    }
}
