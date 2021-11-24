using greenshare_app.Models;
using greenshare_app.Utils;
using greenshare_app.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Command = MvvmHelpers.Commands.Command;
using System.IO;

namespace greenshare_app.ViewModels
{
    class NewPostViewModel : BaseViewModel
    {
        public NewPostViewModel(INavigation navigation, Page view)
        {
            Title = "New Publication";
            //Options = Array.Empty;

            this.navigation = navigation;
            this.view = view;
            

        }

        private INavigation navigation;
        private Page view;

        private string name;
        private string description;
        private string category;
        private bool pickerValue;
        private int index;
        private DateTime minDate;
        private DateTime maxDate;
        private DateTime selectedDate;


        private IList<Image> photos;
        private Image postIcon;
        private IEnumerable<Tag> tags;
        private DateTime terminateAt;

        //private Array Options;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        public IEnumerable<Tag> Tags
        {
            get => tags;
            set => SetProperty(ref tags, value);
        }        
        public Image PostIcon
        {
            get => postIcon;
            set => SetProperty(ref postIcon, value);
        }
        public IList<Image> Photos
        {
            get => photos;
            set => SetProperty(ref photos, value);
        }
        public DateTime TerminateAt
        {
            get => terminateAt;
            set => SetProperty(ref terminateAt, value);
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
        public DateTime MinDate
        {
            get => minDate;
            set => SetProperty(ref minDate, value);
        }

        public DateTime MaxDate
        {
            get => maxDate;
            set => SetProperty(ref maxDate, value);
        }
        public DateTime SelectedDate
        {
            get => selectedDate;
            set => SetProperty(ref selectedDate, value);
        }

        
        /*  
          public EventHandler Picker_OnSelectedIndex(object sender, EventArgs e)
          {
              if (Index == 1) PickerValue = false;
              else PickerValue = true;

          }
        */

        /* WIP RAUL
        public async Task<bool> OnAddPhotoButton()
        {
            var photo = await MediaPicker.CapturePhotoAsync();

            if (photo is null) return false;


            Image photoImage = new Image() { Source=ImageSource.FromStream(() => { return new MemoryStream(photo); }) };
            photos.Add(photo);
        }
        */
    }
}
