using RaceDay.Helpers;
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
    public partial class LoginView : ContentPage
    {
        private static string FacebookLoginUrl = "https://www.facebook.com/dialog/oauth/?display=popup&response_type=token&client_id={0}&redirect_uri=https://www.facebook.com/connect/login_success.html";

        public LoginView()
        {
            InitializeComponent();

            // Set content of page to a Web view with the Facebook login URL
            //
            var webView = new WebView
            {
                Source = string.Format(FacebookLoginUrl, Settings.FacebookAppId),
                HeightRequest = 1
            };
            webView.Navigated += WebView_Navigated;

            Content = webView;
        }

        /// <summary>
        /// Facebook has authorized the user and redirected to the specified redirect url.  Access Token
        /// should be in the url as a parameter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            string[] urlFragments = e.Url.Split('?');
            if ((urlFragments.Length == 2) && (urlFragments[0].ToLower() == "https://www.facebook.com/connect/login_success.html"))
            {
                var accessToken = ExtractAccessTokenFromUrl(urlFragments[1]);
                if (string.IsNullOrEmpty(accessToken))
                {
                    Navigation.PopAsync();
                }
            }

            //if (Settings.HideInformation == false)
            //{
            //    await Navigation.PushAsync(new InfoTips());
            //}
            //else
            //{
            //    await Navigation.PushAsync(new EventTabs());
            //}
            //Navigation.RemovePage(this);
        }

        private string ExtractAccessTokenFromUrl(string url)
        {
            return string.Empty;
        }
    }
}