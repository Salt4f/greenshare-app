using greenshare_app.Utils;
using greenshare_app.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace greenshare_app.ViewModels
{
    class NewPublicationViewModel : BaseViewModel
    {
        public NewPublicationViewModel(INavigation navigation, Page view)
        {
            Title = "New Publication";
            //Options = Array.Empty;

            this.navigation = navigation;
            this.view = view;
            

        }

        private INavigation navigation;
        private Page view;

        private string productName;
        private string description;
        private string category;
        private bool pickerValue;
        private int index;
        
        //private Array Options;

        public string ProductName
        {
            get => productName;
            set => SetProperty(ref productName, value);
        }
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        public string Category
        {
            get => category;
            set => SetProperty(ref category, value);
        }

        public int Index
        {
            get => index;
            set => SetProperty(ref index, value);
        }
        public bool PickerValue
        {
            get => pickerValue;
            set => SetProperty(ref pickerValue, value);
        }

      /*  
        public EventHandler Picker_OnSelectedIndex(object sender, EventArgs e)
        {
            if (Index == 1) PickerValue = false;
            else PickerValue = true;

        }
      */


    }
}
