using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace RaceDay.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            CookieReset reset = new CookieReset();
            reset.Clear();

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            // This sets the colors for the navigation bar to match our Material theme
            //
            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(255, 152, 0);
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes() { TextColor = UIColor.White });

            // The same material theme settings for Tab bar/switches. Rather than fight iOS themes, leave the background of the tab as white 
            // instead of blue as the gray tint color for unselected items can't be well seen and not overridable
            //
            UITabBar.Appearance.SelectedImageTintColor = UIColor.FromRGB(58, 169, 244);
            UISwitch.Appearance.OnTintColor = UIColor.FromRGB(58, 169, 244);

            return base.FinishedLaunching(app, options);
        }
    }
}
