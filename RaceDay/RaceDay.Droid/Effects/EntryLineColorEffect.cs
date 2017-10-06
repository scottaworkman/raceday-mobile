using Xamarin.Forms;
using RaceDay.Droid.Effects;
using Xamarin.Forms.Platform.Android;
using System;
using Android.Widget;
using RaceDay.Behaviors;
using System.ComponentModel;
using System.Diagnostics;

[assembly: ResolutionGroupName("RaceDay")]
[assembly: ExportEffect(typeof(EntryLineColorEffect), "EntryLineColorEffect")]
namespace RaceDay.Droid.Effects
{
    /// <summary>
    /// Used with Custom Validation to allow for the drawing of the line of the Entry control in a custom color based on validation rules.
    /// </summary>
    /// 
    public class EntryLineColorEffect : PlatformEffect
    {
        EditText control;

        protected override void OnAttached()
        {
            try
            {
                control = Control as EditText;
                UpdateLineColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
            control = null;
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            if (args.PropertyName == LineColorBehavior.LineColorProperty.PropertyName)
            {
                UpdateLineColor();
            }
        }

        private void UpdateLineColor()
        {
            try
            {
                if (control != null)
                {
                    control.Background.SetColorFilter(LineColorBehavior.GetLineColor(Element).ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcAtop);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
