using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Facebook.LoginKit;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(RaceDay.iOS.FacebookHandler))]
namespace RaceDay.iOS
{
    public class FacebookHandler : IFacebook
    {
        public void Logout()
        {
            new LoginManager().LogOut();

            NSHttpCookieStorage storage = NSHttpCookieStorage.SharedStorage;
            foreach(var cookie in storage.Cookies)
            {
                string domainName = cookie.Domain.ToLower();
                if (domainName.Contains("facebook"))
                    storage.DeleteCookie(cookie);
            }
            var appDomain = NSBundle.MainBundle.BundleIdentifier;
            NSUserDefaults.StandardUserDefaults.RemovePersistentDomain(appDomain);
            NSUserDefaults.StandardUserDefaults.Synchronize();
        }
    }
}