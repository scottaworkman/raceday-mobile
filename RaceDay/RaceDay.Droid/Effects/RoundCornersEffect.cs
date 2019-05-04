using System.ComponentModel;
using Android.Graphics;
using Android.Views;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using RaceDay.Effects;
using RaceDay.Droid.Effects;

[assembly: ExportEffect(typeof(RoundCornersEffectDroid), nameof(RoundCornersEffect))]
namespace RaceDay.Droid.Effects
{
    public class RoundCornersEffectDroid : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                PrepareContainer();
                SetCornerRadius();
            }
            catch { }
        }

        protected override void OnDetached()
        {
            try
            {
                if (Container != null)
                {
                    Container.OutlineProvider = ViewOutlineProvider.Background;
                }
                else if (Control != null)
                {
                    Control.OutlineProvider = ViewOutlineProvider.Background;
                }
            }
            catch { }
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            if (args.PropertyName == RoundCornersEffect.CornerRadiusProperty.PropertyName)
                SetCornerRadius();
        }

        private void PrepareContainer()
        {
            if (Container != null)
            {
                Container.ClipToOutline = true;
            }
            else if (Control != null)
            {
                Control.ClipToOutline = true;
            }
        }

        private void SetCornerRadius()
        {
            var cornerRadius = RoundCornersEffect.GetCornerRadius(Element) * GetDensity();
            if (Container != null)
            {
                Container.OutlineProvider = new RoundedOutlineProvider(cornerRadius);
            }
            else if (Control != null)
            {
                Control.OutlineProvider = new RoundedOutlineProvider(cornerRadius);
            }
        }

        private static float GetDensity() =>
            CrossCurrentActivity.Current.Activity.Resources.DisplayMetrics.Density;

        private class RoundedOutlineProvider : ViewOutlineProvider
        {
            private readonly float _radius;

            public RoundedOutlineProvider(float radius)
            {
                _radius = radius;
            }

            public override void GetOutline(Android.Views.View view, Outline outline)
            {
                outline?.SetRoundRect(0, 0, view.Width, view.Height, _radius);
            }
        }
    }
}