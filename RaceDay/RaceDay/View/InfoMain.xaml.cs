using Microsoft.AppCenter.Analytics;
using Plugin.Connectivity;
using RaceDay.Helpers;
using RaceDay.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RaceDay.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoMain : ContentPage
    {
        Boolean isStartup = true;
        public InfoMain()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (isStartup)
            {
                ResetNavigationStack();
                isStartup = false;
            }
        }

        protected async void Facebook_Login(object sender, FacebookLoginHandlerArgs args)
        {
            loginButton.IsVisible = false;
            FacebookAccess.IsVisible = true;

            // Use Facebook graph api to get user information just logged in and make sure they are a member of the group
            //
            FacebookClient fb = new FacebookClient(args.AccessToken);
            await fb.GetUserProfile();
            if (string.IsNullOrEmpty(fb.Id))
            {
                await DisplayAlert("Facebook Error", "Unable to obtain Facebook information", "Ok");
                loginButton.IsVisible = true;
                FacebookAccess.IsVisible = false;
                DependencyService.Get<IFacebook>()?.Logout();
                return;
            }

            if (await fb.UserInGroup(Settings.GroupFacebookId) == false)
            {
                await DisplayAlert("Facebook Group Required", "It does not look as if you are a member of the " + Settings.GroupName + " group.  Access to RaceDay for this group is limited to group members.", "Ok");
                loginButton.IsVisible = true;
                FacebookAccess.IsVisible = false;
                DependencyService.Get<IFacebook>()?.Logout();
                return;
            }

            // Store the User Id for future use and move on to the main page
            //
            Settings.UserId = fb.Id;
            Settings.UserName = fb.Name;
            Settings.UserEmail = fb.Email;

            // Make sure this user is in the server API
            //
            await RaceDayClient.AddUser(fb.Id, fb.Name, fb.Email);

            // Login Custom Event
            //
            Analytics.TrackEvent("Login",
                    new Dictionary<string, string> {
                            { "UID", Settings.UserId } }
                    );

            await Navigation.PushAsync(new EventTabs(), false);
        }

        protected async void Facebook_Error(object sender, FacebookLoginHandlerArgs args)
        {
            await DisplayAlert("Facebook Login Error", args.ErrorMessage, "OK");
            return;
        }

        private void ResetNavigationStack()
        {
            if (Navigation != null && Navigation.NavigationStack.Count() > 1)
            {
                while (Navigation.NavigationStack.Count() > 1)
                {
                    Navigation.RemovePage(Navigation.NavigationStack[0]);
                }
            }
        }
    }
}