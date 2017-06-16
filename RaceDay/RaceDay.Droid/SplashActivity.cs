using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace RaceDay.Droid
{
    [Activity(Theme = "@style/RaceDayTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();

            Task startupWork = new Task(() => { Startup(); });
            startupWork.Start();
        }

        async void Startup()
        {
            await Task.Delay(200);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }

    }
}