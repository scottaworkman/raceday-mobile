using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(RaceDay.Droid.Snackbar))]
namespace RaceDay.Droid
{
    public class SnackbarResult : ISnackbarResult
    {
        public SnackbarAction Action { get; set; }
    }

    public class Snackbar : ISnackbar
    {
        private ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private SnackbarResult _eventResult;
        private VisualElement _floatingButton;

        private object _lock = new object();

        private Android.Support.Design.Widget.Snackbar snackbar;

        public Snackbar() {}

        public async Task<ISnackbarResult> Show(SnackbarOptions options)
        {
            return await Task.Run(() =>
            {
                Activity activity = CrossCurrentActivity.Current.Activity;
                Android.Views.View activityRootView = activity.FindViewById(Android.Resource.Id.Content);

                snackbar = Android.Support.Design.Widget.Snackbar.Make(activityRootView, options.Text, (options.Duration == SnackbarDuration.Short ? Android.Support.Design.Widget.Snackbar.LengthShort : Android.Support.Design.Widget.Snackbar.LengthLong));
                snackbar.AddCallback(new SnackbarCallback(options.FloatingButton, (result) => { SnackbarClosed(result); }));
                _floatingButton = options.FloatingButton;

                snackbar.Show();
                _resetEvent = new ManualResetEvent(false);
                _resetEvent.WaitOne();

                return _eventResult;
            });
        }

        public async Task<ISnackbarResult> Notify()
        {
            return await Task.Run(() =>
            {
                _resetEvent = new ManualResetEvent(false);
                _resetEvent.WaitOne();

                return _eventResult;
            });
        }

        public int GetHeight()
        {
            return snackbar.View.Height;
        }

        private void SnackbarClosed(SnackbarResult result)
        {
            lock(_lock)
            {
                _eventResult = result;
                _resetEvent.Set();
            }
        }
    }

    internal class SnackbarCallback : Android.Support.Design.Widget.BaseTransientBottomBar.BaseCallback
    {
        private VisualElement _fab;
        private Action<SnackbarResult> _callback;

        public SnackbarCallback(VisualElement button, Action<SnackbarResult> callback)
        {
            _fab = button;
            _callback = callback;
        }

        public override void OnShown(Java.Lang.Object transientBottomBar)
        {
            if (_fab != null)
                _fab.TranslateTo(0, -(((Android.Support.Design.Widget.Snackbar)transientBottomBar).View.Height / 4));

            _callback(new SnackbarResult() { Action = SnackbarAction.Create });
        }

        public override void OnDismissed(Java.Lang.Object transientBottomBar, int evt)
        {
            if (_fab != null)
                _fab.TranslateTo(0, 0);

            switch (evt)
            {
                case DismissEventAction:
                    _callback(new SnackbarResult() { Action = SnackbarAction.Clicked });
                    break;
                case DismissEventConsecutive:
                case DismissEventManual:
                case DismissEventSwipe:
                    _callback(new SnackbarResult() { Action = SnackbarAction.Dismissed });
                    break;

                case DismissEventTimeout:
                default:
                    _callback(new SnackbarResult() { Action = SnackbarAction.Timeout });
                    break;
            }
        }
    }
}