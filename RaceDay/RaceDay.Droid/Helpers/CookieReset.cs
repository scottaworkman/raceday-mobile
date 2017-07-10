using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RaceDay.Droid
{
    public class CookieReset : ICookieReset
    {
        public void Clear()
        {
            Android.Webkit.CookieManager.Instance.RemoveAllCookies(null);
        }
    }
}