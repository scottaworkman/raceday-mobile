﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Facebook.Login;

[assembly: Xamarin.Forms.Dependency(typeof(RaceDay.Droid.Facebook))]
namespace RaceDay.Droid
{
    public class Facebook : IFacebook
    {
        public void Logout()
        {
            if (LoginManager.Instance != null)
                LoginManager.Instance.LogOut();
        }
    }
}