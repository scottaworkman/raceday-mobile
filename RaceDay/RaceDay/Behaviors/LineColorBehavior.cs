using System.Linq;
using Xamarin.Forms;
using RaceDay.Effects;

namespace RaceDay.Behaviors
{
    /// <summary>
    /// Used with the custom validation to update the line color of a text entry field based on validation rules.  The ApplyLineColor property
    /// is used to enable/disable this property (the behavior is added to the style) to display the normal line color.
    /// 
    /// The LineColorBehavior works in conjunction with the LineColorEffect to perform the drawing of the entry line based on LineColor
    /// </summary>
    /// 
    public static class LineColorBehavior
    {
        public static readonly BindableProperty ApplyLineColorProperty =
            BindableProperty.CreateAttached("ApplyLineColor", typeof(bool), typeof(LineColorBehavior), false, propertyChanged: OnApplyLineColorChanged);

        public static readonly BindableProperty LineColorProperty =
            BindableProperty.CreateAttached("LineColor", typeof(Color), typeof(LineColorBehavior), Color.Default);

        public static bool GetApplyLineColor(BindableObject view)
        {
            return (bool)view.GetValue(ApplyLineColorProperty);
        }

        public static void SetApplyLineColor(BindableObject view, bool value)
        {
            view.SetValue(ApplyLineColorProperty, value);
        }

        public static Color GetLineColor(BindableObject view)
        {
            return (Color)view.GetValue(LineColorProperty);
        }

        public static void SetLineColor(BindableObject view, Color value)
        {
            view.SetValue(LineColorProperty, value);
        }

        private static void OnApplyLineColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Xamarin.Forms.View;

            if (view == null)
            {
                return;
            }

            bool hasLine = (bool)newValue;

            if (hasLine)
            {
                view.Effects.Add(new EntryLineColorEffect());
            }
            else
            {
                var entryLineColorEffectToRemove = view.Effects.FirstOrDefault(e => e is EntryLineColorEffect);
                if (entryLineColorEffectToRemove != null)
                {
                    view.Effects.Remove(entryLineColorEffectToRemove);
                }
            }
        }
    }
}
