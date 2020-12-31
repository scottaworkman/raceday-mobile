using Xamarin.Forms;
using Android.OS;
using Plugin.CurrentActivity;
using RaceDay.Helpers;

[assembly: Dependency(typeof(RaceDay.Droid.Helpers.Statusbar))]
namespace RaceDay.Droid.Helpers
{
    public class Statusbar : IStatusBarPlatformSpecific
    {
        public Statusbar()
        {
        }

        public void SetStatusBarColor(Color color)
        {
            // The SetStatusBarcolor is new since API 21
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                //Just use the plugin
                CrossCurrentActivity.Current.Activity.Window.SetStatusBarColor(Android.Graphics.Color.ParseColor(color.ToHex()));
            }
            else
            {
                // Here you will just have to set your 
                // color in styles.xml file as shown below.
            }
        }
    }
}