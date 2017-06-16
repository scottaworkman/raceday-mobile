using Android.Widget;
using Plugin.CurrentActivity;

namespace RaceDay.Droid
{
    public class Toast : IToast
    {
        Android.Widget.Toast toast;
        public Toast() {}

        public void Show(ToastOptions options)
        {
            toast = Android.Widget.Toast.MakeText(CrossCurrentActivity.Current.Activity, options.Text, (options.Duration == ToastDuration.Short ? ToastLength.Short : ToastLength.Long));
            toast.Show();
        }

        public int Duration()
        {
            return (toast.Duration == ToastLength.Long ? 3500 : 2000);
        }
    }
}