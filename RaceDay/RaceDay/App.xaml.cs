using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using RaceDay.Helpers;
using RaceDay.View;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace RaceDay
{
    public partial class App : Application
    {
        public const string ANDROID_APP_ID = @"ec5de463-324c-4c44-b753-24e6a1da190d";
        public const string IOS_APP_ID = @"741b6b8c-81c1-4a46-983b-c2d532efdcd2";

        public App()
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(Settings.UserId))
            {
                MainPage = new NavigationPage(new InfoMain());
            }
            else
            {
                MainPage = new NavigationPage(new EventTabs());
            }
        }

        protected override void OnStart()
        {
            AppCenter.Start(
                string.Format("android={0};ios={1}", ANDROID_APP_ID, IOS_APP_ID), 
                typeof(Analytics), 
                typeof(Crashes));

            Analytics.TrackEvent("App Start",
                    new Dictionary<string, string>
                    {
                        { "UID", Settings.UserId }
                    });
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
