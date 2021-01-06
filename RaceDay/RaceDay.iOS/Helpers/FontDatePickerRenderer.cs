using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using RaceDay.iOS;
using UIKit;
using Foundation;

[assembly: ExportRenderer(typeof(RaceDay.Helpers.FontDatePicker), typeof(FontDatePickerRenderer))]

namespace RaceDay.iOS
{
    [Preserve(AllMembers = true)]
    public class FontDatePickerRenderer : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);

            if ((Control != null) && (e.NewElement != null))
            {
                UIFont font = Control.Font.WithSize(((RaceDay.Helpers.FontDatePicker)e.NewElement).FontSize);
                Control.Font = font;
            }
        }
    }
}