﻿using Android.Graphics.Drawables;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Application = Android.App.Application;

[assembly: ExportRenderer(typeof(greenshare_app.Controls.BorderlessDatePicker), typeof(greenshare_app.Droid.BorderlessDatePickerRenderer))]

namespace greenshare_app.Droid
{

    public class BorderlessDatePickerRenderer : DatePickerRenderer
    {
        public BorderlessDatePickerRenderer()
            : base(Application.Context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;

                var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                layoutParams.SetMargins(0, 0, 0, 0);
                LayoutParameters = layoutParams;
                Control.LayoutParameters = layoutParams;
                Control.SetPadding(0, 0, 0, 0);
                SetPadding(0, 0, 0, 0);
            }
        }
    }
}