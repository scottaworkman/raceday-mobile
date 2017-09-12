using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using RaceDay.iOS;
using UIKit;
using RaceDay.Helpers;

[assembly: ExportRenderer(typeof(ExpandableEditor), typeof(ExpandableEditorRenderer))]

namespace RaceDay.iOS
{
   public class ExpandableEditorRenderer : EditorRenderer
    {
        private string Placeholder { get; set; }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
                Control.ScrollEnabled = false;

            var element = e.NewElement as ExpandableEditor;
            if (Control != null && element != null)
            {
                Placeholder = element.Placeholder;
                if (element.Text == string.Empty)
                {
                    Control.TextColor = UIColor.LightGray;
                    element.Text = Placeholder;
                }

                Control.Layer.CornerRadius = 6;
                Control.Layer.BorderColor = Color.FromHex("F0F0F0").ToCGColor();
                Control.Layer.BorderWidth = 2;

                Control.ShouldBeginEditing += (UITextView textView) =>
                {
                    if (textView.Text == Placeholder)
                    {
                        textView.Text = string.Empty;
                        textView.TextColor = UIColor.Black;
                    }

                    return true;
                };

                Control.ShouldEndEditing += (UITextView textView) =>
                {
                    if (textView.Text == string.Empty)
                    {
                        textView.Text = Placeholder;
                        textView.TextColor = UIColor.LightGray;
                    }

                    return true;
                };
            }
        }
    }
}