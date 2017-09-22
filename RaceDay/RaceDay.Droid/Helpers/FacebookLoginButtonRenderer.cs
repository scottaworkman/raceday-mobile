using RaceDay;
using RaceDay.Helpers;
using RaceDay.Droid;
using RaceDay.Droid.Helpers;

using Xamarin.Forms;
using Xamarin.Facebook;
using Xamarin.Forms.Platform.Android;
using Xamarin.Facebook.Login.Widget;
using System;
using Android.Runtime;
using Plugin.CurrentActivity;

[assembly: ExportRenderer(typeof(FacebookLoginButton), typeof(FacebookLoginButtonRenderer))]
namespace RaceDay.Droid.Helpers
{
    // Custom Renderer for the Facebook login button.  Uses Facebook Android SDK to render login button widget
    //
    public class FacebookLoginButtonRenderer : ViewRenderer<FacebookLoginButton, LoginButton>
    {
        LoginButton facebookLoginButton;

        protected override void OnElementChanged(ElementChangedEventArgs<FacebookLoginButton> e)
        {
            base.OnElementChanged(e);
            if (Control == null || facebookLoginButton == null)
            {
                facebookLoginButton = new LoginButton(Forms.Context);
                facebookLoginButton.SetTextSize(Android.Util.ComplexUnitType.Sp, 18);
                facebookLoginButton.SetPadding(36, 36, 36, 36);
                SetNativeControl(facebookLoginButton);

                MainActivity mainActivity = CrossCurrentActivity.Current.Activity as MainActivity;
                if (mainActivity != null && e.OldElement != null)
                {
                    mainActivity.OnFacebookLogin -= e.OldElement.FacebookLogin;
                    mainActivity.OnFacebookCancel -= e.OldElement.FacebookCancel;
                    mainActivity.OnFacebookError -= e.OldElement.FacebookError;
                }
                if (mainActivity != null && e.NewElement != null)
                {
                    mainActivity.OnFacebookLogin += e.NewElement.FacebookLogin;
                    mainActivity.OnFacebookCancel += e.NewElement.FacebookCancel;
                    mainActivity.OnFacebookError += e.NewElement.FacebookError;
                }
            }
        }
    }

    // Facebook Callback class.  Registered with callback manager so is the first event instance contacted by
    // Facebook SDK when a login result occurs.  The app MainActivity will register Action handlers for the
    // login result events below, so the task returns to MainActivity for processing.
    //
    class FacebookCallback<TResult> : Java.Lang.Object, IFacebookCallback where TResult : Java.Lang.Object
    {
        public Action HandleCancel { get; set; }
        public Action<FacebookException> HandleError { get; set; }
        public Action<TResult> HandleSuccess { get; set; }

        public void OnCancel()
        {
            HandleCancel?.Invoke();
        }

        public void OnError(FacebookException error)
        {
            HandleError?.Invoke(error);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            HandleSuccess?.Invoke(result.JavaCast<TResult>());
        }
    }
}