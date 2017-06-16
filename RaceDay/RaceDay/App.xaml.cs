using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaceDay.Model;
using RaceDay.View;
using Xamarin.Forms;

namespace RaceDay
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            var content = new EventTabs();
            MainPage = new NavigationPage(content);

            //fab.Hide();
            //var toast = DependencyService.Get<IToast>();
            //toast.Show(new ToastOptions { Text = "Button Pressed", Duration = ToastDuration.Short });
            //await Task.Delay(toast.Duration());
            //fab.Show();

            //var snack = DependencyService.Get<ISnackbar>();
            //await snack.Show(new SnackbarOptions { Text = "Snackbar option", Duration = SnackbarDuration.Short, FloatingButton = fab });
            //int snackHeight = snack.GetHeight();

            //var result = await snack.Notify();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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
