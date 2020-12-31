using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using RaceDay.Helpers;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(RaceDay.iOS.Helpers.Statusbar))]
namespace RaceDay.iOS.Helpers
{
    public class Statusbar : IStatusBarPlatformSpecific
    {
        public Statusbar()
        {
        }

        public void SetStatusBarColor(Color color)
        {
            UIView statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;

            if (statusBar != null && statusBar.RespondsToSelector(
            new ObjCRuntime.Selector("setBackgroundColor:")))
            {
                // change to your desired color 
                statusBar.BackgroundColor = UIColor.FromRGB(new nfloat(color.R), new nfloat(color.G), new nfloat(color.B));
            }
        }
    }
}