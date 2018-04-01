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
using RaceDay.Helpers;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(ExpandableEditor), typeof(ExpandableEditorRenderer))]

namespace RaceDay.Droid
{
   public class ExpandableEditorRenderer : EditorRenderer
   {
        public ExpandableEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var element = e.NewElement as ExpandableEditor;
                if (!string.IsNullOrEmpty(element.Placeholder))
                {
                    Control.Hint = element.Placeholder;
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ExpandableEditor.PlaceholderProperty.PropertyName)
            {
                var element = this.Element as ExpandableEditor;
                if (!string.IsNullOrEmpty(element.Placeholder))
                {
                    Control.Hint = element.Placeholder;
                }
            }
        }
    }
}