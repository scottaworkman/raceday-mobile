using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using TTGSnackBar;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(RaceDay.iOS.Snackbar))]
namespace RaceDay.iOS
{
    public class SnackbarResult : ISnackbarResult
    {
        public SnackbarAction Action { get; set; }
    }

    public class Snackbar : ISnackbar
    {
        public Snackbar() { }

        public int GetHeight()
        {
            var snackbar = new TTGSnackbar("Test");
            return Convert.ToInt32(snackbar.SnapshotView(false).Bounds.Size.Height);
        }

        public async Task<ISnackbarResult> Notify()
        {
            return await Task.Run(() =>
            {
                return new SnackbarResult() { Action = SnackbarAction.Timeout };
            });
        }

        public async Task<ISnackbarResult> Show(SnackbarOptions options)
        {
            return await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var snackbar = new TTGSnackbar("  " + options.Text + "  ");
                    snackbar.Height = 54;
                    snackbar.LeftMargin = snackbar.RightMargin = snackbar.BottomMargin = 0;
                    snackbar.CornerRadius = 0;
                    snackbar.Duration = TimeSpan.FromMilliseconds((options.Duration == SnackbarDuration.Long ? 3500 : 2000));
                    snackbar.Show();
                });

                return new SnackbarResult() { Action = SnackbarAction.Timeout };
            });
        }
    }
}