using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using RaceDay.Droid;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;
using RaceDay.Helpers;

[assembly: ExportRenderer(typeof(FontDatePicker), typeof(FontDatePickerRenderer))]

namespace RaceDay.Droid
{
   public class FontDatePickerRenderer : DatePickerRenderer
    {

        public FontDatePickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);

            var element = this.Element as FontDatePicker;
            Control.TextSize = element.FontSize;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == FontDatePicker.FontSizeProperty.PropertyName)
            {
                var element = this.Element as FontDatePicker;
                Control.TextSize = element.FontSize;
            }

        }
    }
}