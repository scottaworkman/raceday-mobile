using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaceDay.Model;
using RaceDay.View;
using Xamarin.Forms;
using RaceDay.Helpers;
using HockeyApp;

namespace RaceDay
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(Settings.UserId))
            {
                MainPage = new NavigationPage(new InfoMain());
            }
            else if (Settings.HideInformation == false)
            {
                MainPage = new NavigationPage(new InfoTips());
            }
            else
            {
                MainPage = new NavigationPage(new EventTabs());
            }
        }

        protected override void OnStart()
        {
            MetricsManager.TrackEvent("App Start",
                new Dictionary<string, string> {
                    { "UID", Settings.UserId } }, 
                new Dictionary<string, double>());
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
