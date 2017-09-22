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
using Xamarin.Facebook;
using RaceDay.Droid.Helpers;
using Xamarin.Facebook.Login;
using RaceDay.Helpers;

namespace RaceDay.Droid
{
    [Activity(Label = "RaceDay", Icon = "@drawable/icon", Name = "com.workmanfamily.jymfraceday.MainActivity", Theme = "@style/RaceDayTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public const string HOCKEY_APP_ID = @"05f78d8a53b546719b0e563dae9de3fe";
        ICallbackManager callbackManager;

        public event FacebookLoginHandler OnFacebookLogin;
        public event FacebookLoginHandler OnFacebookCancel;
        public event FacebookLoginHandler OnFacebookError;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            Android.Webkit.CookieManager.Instance.RemoveAllCookies(null);

            // Hockey App Metrics management initialization
            //
            MetricsManager.Register(Application, HOCKEY_APP_ID);
            MetricsManager.EnableUserMetrics();

            // Facebook SDK login callback actions.  Callback manager handled in a central location for
            // the control of login actions.  Well check to see who else has registered with us to bubble the
            // events to.
            //
            callbackManager = CallbackManagerFactory.Create();
            var loginCallback = new FacebookCallback<LoginResult>
            {
                HandleSuccess = loginResult =>
                {
                    var args = new FacebookLoginHandlerArgs
                    {
                        AccessToken = loginResult.AccessToken.Token,
                        UserId = loginResult.AccessToken.UserId,
                        ErrorMessage = string.Empty
                    };

                    OnFacebookLogin?.Invoke(this, args);
                },
                HandleCancel = () =>
                {
                    OnFacebookCancel?.Invoke(this, new FacebookLoginHandlerArgs());
                },
                HandleError = loginError =>
                {
                    OnFacebookError?.Invoke(this, new FacebookLoginHandlerArgs()
                    {
                        ErrorMessage = loginError.Message
                    });
                }
            };
            Xamarin.Facebook.Login.LoginManager.Instance.RegisterCallback(callbackManager, loginCallback);

            // Load up the Xamarin Forms application
            //
            LoadApplication(new App());
        }

        protected override void OnResume()
        {
            base.OnResume();
            CrashManager.Register(this, HOCKEY_APP_ID);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            callbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }
    }
}

