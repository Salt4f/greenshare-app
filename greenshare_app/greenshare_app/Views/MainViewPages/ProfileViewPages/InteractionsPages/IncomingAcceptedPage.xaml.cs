﻿using greenshare_app.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace greenshare_app.Views.MainViewPages.ProfileViewPages.InteractionsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IncomingAcceptedPage : ContentPage
    {
        public IncomingAcceptedPage()
        {
            InitializeComponent();
            BindingContext = new IncomingAcceptedViewModel(this.Navigation, this);
        }
    }
}