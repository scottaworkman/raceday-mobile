using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Android.Content;
using RaceDay.Helpers;

namespace RaceDay.Droid
{
    [Activity(Label = "RaceDay", Icon = "@drawable/icon", Name = "com.workmanfamily.jymfraceday.MainActivity", Theme = "@style/RaceDayTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");
            global::Xamarin.Forms.Forms.Init(this, bundle);

            Android.Webkit.CookieManager.Instance.RemoveAllCookies(null);

            // Load up the Xamarin Forms application
            //
            LoadApplication(new App());
        }

        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}

