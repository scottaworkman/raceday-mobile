using RaceDay.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RaceDay.Services;

namespace RaceDay.View
{
    /// <summary>
    /// Manual Facebook Login process using a webview.  While not recommended by Facebook, I chose this method as the Facebook SDK seemed way 
    /// too heavy of an overhead for the little I needed to interact with Facebook. Additionally, this keeps the process within the PCL without
    /// platform specific SDK being required and was much easier to setup than the SDK which I never did get to work reliably.
    /// </summary>
    /// 
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        private static string FacebookLoginUrl = "https://www.facebook.com/dialog/oauth/?display=popup&response_type=token&client_id={0}&redirect_uri=https://www.facebook.com/connect/login_success.html";
        private WebView webView;

        public LoginView()
        {
            InitializeComponent();

            // Clear cookies on displaying the web view to remove any previous logins
            //
            var cookie = DependencyService.Get<ICookieReset>();
            cookie.Clear();

            // Set content of page to a Web view with the Facebook login URL
            //
            webView = new WebView
            {
                Source = string.Format(FacebookLoginUrl, Settings.FacebookAppId),
                HeightRequest = 1
            };

            Content = webView;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            webView.Navigated += WebView_Navigated;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            webView.Navigated -= WebView_Navigated;
        }

        /// <summary>
        /// Facebook has authorized the user and redirected to the specified redirect url.  Access Token
        /// should be in the url as a parameter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            string[] urlFragments = e.Url.Split(new char[] { '?', '#' });
            if ((urlFragments.Length >= 2) && (urlFragments[0].ToLower() == "https://www.facebook.com/connect/login_success.html"))
            {
                var accessToken = ExtractAccessTokenFromUrl(urlFragments[1]);
                if (string.IsNullOrEmpty(accessToken))
                {
                    await Navigation.PopAsync();
                    return;
                }

                Content = new Label
                {
                    Text = "Checking Facebook Group Access",
                    HorizontalTextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0,20,0,20)
                };

                // Use Facebook graph api to get user information just logged in and make sure they are a member of the group
                //
                FacebookClient fb = new FacebookClient(accessToken);
                await fb.GetUserProfile();
                if (string.IsNullOrEmpty(fb.Id))
                {
                    await DisplayAlert("Facebook Error", "Unable to obtain Facebook information", "Ok");
                    await Navigation.PopAsync();
                    return;
                }
                if (await fb.UserInGroup(Settings.GroupFacebookId) == false)
                {
                    await DisplayAlert("Facebook Group Required", "It does not look as if you are a member of the " + Settings.GroupName + " group.  Access to RaceDay for this group is limited to group members.", "Ok");
                    await Navigation.PopAsync();
                    return;
                }

                // Store the User Id for future use and move on to the main page
                //
                Settings.UserId = fb.Id;
                Settings.UserName = fb.Name;
                Settings.UserEmail = fb.Email;

                // Login Custom Event
                //
                HockeyApp.MetricsManager.TrackEvent("Login",
                    new Dictionary<string, string> {
                        { "UID", Settings.UserId } },
                    new Dictionary<string, double>());

                await Navigation.PushAsync(new EventTabs());
            }
        }

        /// <summary>
        /// Find the access token in the query string
        /// </summary>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        /// 
        private string ExtractAccessTokenFromUrl(string queryParameters)
        {
            string[] parameters = queryParameters.Split('&');
            foreach(string parameter in parameters)
            {
                string[] keyvalue = parameter.Split('=');
                if ((keyvalue.Length == 2) && (keyvalue[0].ToLower() == "access_token"))
                {
                    return WebUtility.UrlDecode(keyvalue[1]);
                }
            }

            return string.Empty;
        }
    }
}