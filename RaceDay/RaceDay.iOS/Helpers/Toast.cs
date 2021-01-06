using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(RaceDay.iOS.Toast))]
namespace RaceDay.iOS
{
    [Preserve(AllMembers = true)]
    public class Toast : IToast
    {
        const double LONG_DELAY = 3.5;
        const double SHORT_DELAY = 2.0;

        NSTimer alertDelay;
        UIAlertController alert;

        int duration;

        public int Duration()
        {
            return duration;
        }

        public void Show(ToastOptions options)
        {
            if (options.Duration == ToastDuration.Long)
                duration = Convert.ToInt32(LONG_DELAY * 1000);
            else
                duration = Convert.ToInt32(SHORT_DELAY * 1000);

            ShowAlert(options.Text, (options.Duration == ToastDuration.Long ? LONG_DELAY : SHORT_DELAY));
        }

        void ShowAlert(string message, double seconds)
        {
            alertDelay = NSTimer.CreateScheduledTimer(seconds, (obj) =>
            {
                DismissMessage();
            });
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        void DismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }

    }
}
