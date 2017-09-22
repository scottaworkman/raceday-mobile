using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceDay.Helpers
{
    public delegate void FacebookLoginHandler(object sender, FacebookLoginHandlerArgs args);

    public class FacebookLoginButton : Xamarin.Forms.View
    {
        public event FacebookLoginHandler OnFacebookLogin;
        public event FacebookLoginHandler OnFacebookCancel;
        public event FacebookLoginHandler OnFacebookError;

        public void FacebookLogin(object sender, FacebookLoginHandlerArgs args)
        {
            OnFacebookLogin?.Invoke(sender, args);
        }

        public void FacebookCancel(object sender, FacebookLoginHandlerArgs args)
        {
            OnFacebookCancel?.Invoke(sender, args);
        }

        public void FacebookError(object sender, FacebookLoginHandlerArgs args)
        {
            OnFacebookError?.Invoke(sender, args);
        }
    }

    public class FacebookLoginHandlerArgs
    {
        public string AccessToken { get; set; }
        public string UserId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
