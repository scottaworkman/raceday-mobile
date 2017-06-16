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
                Control.TextColor = UIColor.LightGray;
                Control.Text = Placeholder;

                Control.ShouldBeginEditing += (UITextView textView) =>
                {
                    if (textView.Text == Placeholder)
                    {
                        textView.Text = "";
                        textView.TextColor = UIColor.Black;
                    }

                    return true;
                };

                Control.ShouldEndEditing += (UITextView textView) =>
                {
                    if (textView.Text == "")
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