using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(RaceDay.iOS.CookieReset))]
namespace RaceDay.iOS
{
    [Preserve(AllMembers = true)]
    public class CookieReset : ICookieReset
    {
        public void Clear()
        {
            NSHttpCookieStorage CookieStorage = NSHttpCookieStorage.SharedStorage;
            foreach( var cookie in CookieStorage.Cookies)
            {
                CookieStorage.DeleteCookie(cookie);
            }
        }
    }
}