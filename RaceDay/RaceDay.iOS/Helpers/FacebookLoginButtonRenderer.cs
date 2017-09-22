using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using Facebook.LoginKit;

using RaceDay.Helpers;
using RaceDay.iOS.Helpers;
using CoreGraphics;
using Facebook.CoreKit;

[assembly: ExportRenderer(typeof(FacebookLoginButton), typeof(FacebookLoginButtonRenderer))]
namespace RaceDay.iOS.Helpers
{
    // Custom Renderer for the Facebook login button.  Uses Facebook iOS SDK to render login button widget
    //
    public class FacebookLoginButtonRenderer : ViewRenderer<FacebookLoginButton, LoginButton>
    {
        LoginButton facebookLoginButton;
        event FacebookLoginHandler OnFacebookLogin;
        event FacebookLoginHandler OnFacebookCancel;
        event FacebookLoginHandler OnFacebookError;

        protected override void OnElementChanged(ElementChangedEventArgs<FacebookLoginButton> e)
        {
            base.OnElementChanged(e);
            if (Control == null || facebookLoginButton == null)
            {
                facebookLoginButton = new LoginButton();
                facebookLoginButton.Font = UIFont.FromName(facebookLoginButton.Font.Name, 18f);
                SetNativeControl(facebookLoginButton);
                if (e.OldElement != null)
                {
                    OnFacebookLogin -= e.OldElement.FacebookLogin;
                    OnFacebookCancel -= e.OldElement.FacebookCancel;
                    OnFacebookError -= e.OldElement.FacebookError;
                    facebookLoginButton.Completed -= FacebookLoginButton_Completed;
                }
                if (e.NewElement != null)
                {
                    OnFacebookLogin += e.NewElement.FacebookLogin;
                    OnFacebookCancel += e.NewElement.FacebookCancel;
                    OnFacebookError += e.NewElement.FacebookError;
                    facebookLoginButton.Completed += FacebookLoginButton_Completed;
                }
            }
        }

        private void FacebookLoginButton_Completed(object sender, LoginButtonCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                OnFacebookError?.Invoke(sender, new FacebookLoginHandlerArgs
                {
                    ErrorMessage = "Error Code: " + e.Error.Code.ToString() + ". " + e.Error.Description
                });
            }
            else if (e.Result != null && e.Result.IsCancelled)
            {
                OnFacebookCancel?.Invoke(sender, new FacebookLoginHandlerArgs());
            }
            else
            {
                OnFacebookLogin?.Invoke(sender, new FacebookLoginHandlerArgs
                {
                    AccessToken = AccessToken.CurrentAccessToken?.TokenString,
                    UserId = AccessToken.CurrentAccessToken?.UserID,
                    ErrorMessage = e.Error?.Description
                });
            }
        }
    }
}