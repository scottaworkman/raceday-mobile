using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Android.Content;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;

namespace RaceDay.Droid
{
    [Activity(Label = "RaceDay", Icon = "@drawable/icon", Theme = "@style/RaceDayTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public const string HOCKEY_APP_ID = @"05f78d8a53b546719b0e563dae9de3fe";

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            DependencyService.Register<Toast>();
            DependencyService.Register<Snackbar>();
            DependencyService.Register<CookieReset>();

            Android.Webkit.CookieManager.Instance.RemoveAllCookies(null);

            MetricsManager.Register(Application, HOCKEY_APP_ID);

            LoadApplication(new App());
        }

        protected override void OnResume()
        {
            base.OnResume();
            CrashManager.Register(this, HOCKEY_APP_ID);
        }
    }
}

