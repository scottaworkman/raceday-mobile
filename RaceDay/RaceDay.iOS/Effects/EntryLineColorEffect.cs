using CoreAnimation;
using CoreGraphics;
using RaceDay.Behaviors;
using RaceDay.iOS.Effects;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("RaceDay")]
[assembly: ExportEffect(typeof(EntryLineColorEffect), "EntryLineColorEffect")]
namespace RaceDay.iOS.Effects
{
    /// <summary>
    /// Used with Custom Validation to allow for the drawing of the line of the Entry control in a custom color based on validation rules.
    /// </summary>
    /// 
    public class EntryLineColorEffect : PlatformEffect
    {
        UITextField control;

        public EntryLineColorEffect()
        {

        }

        protected override void OnAttached()
        {
            try
            {
                control = Control as UITextField;
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
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == LineColorBehavior.LineColorProperty.PropertyName ||
                args.PropertyName == "Height")
            {
                Initialize();
                UpdateLineColor();
            }
        }

        private void Initialize()
        {
            var entry = Element as Entry;
            if (entry != null)
            {
                Control.Bounds = new CGRect(0, 0, entry.Width, entry.Height);
            }
        }

        private void UpdateLineColor()
        {
            BorderLineLayer lineLayer = control.Layer.Sublayers.OfType<BorderLineLayer>()
                                                             .FirstOrDefault();

            if (lineLayer == null)
            {
                lineLayer = new BorderLineLayer
                {
                    MasksToBounds = true,
                    BorderWidth = 0.5f
                };
                control.Layer.AddSublayer(lineLayer);
                control.BorderStyle = UITextBorderStyle.None;

                UIView paddingView = new UIView(new RectangleF(0, 0, 8, 30));
                control.LeftView = paddingView;
                control.LeftViewMode = UITextFieldViewMode.Always;
            }

            lineLayer.Frame = new CGRect(0f, 0f, Control.Bounds.Width, Control.Frame.Height);
            lineLayer.BorderColor = LineColorBehavior.GetLineColor(Element).ToCGColor();
            lineLayer.CornerRadius = 4f;
            control.TintColor = control.TextColor;
        }

        private class BorderLineLayer : CALayer
        {
        }
    }
}