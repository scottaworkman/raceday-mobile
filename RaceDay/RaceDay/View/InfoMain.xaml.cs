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
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using RaceDay.ViewModel;

namespace RaceDay.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoMain : ContentPage
    {
        Boolean isStartup = true;
        public InfoMain()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            var vm = new LoginViewModel();
            BindingContext = vm;

            ForgotLabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Navigation.PushAsync(new ForgotPassword());
                })
            });
            RegisterLabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Navigation.PushAsync(new RegisterAccount());
                })
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (isStartup)
            {
                ResetNavigationStack();
                isStartup = false;
            }

            LoginButton.Clicked += LoginButton_Clicked;
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            LoginViewModel vm = BindingContext as LoginViewModel;

            if (vm.Validate() == false)
            {
                if (vm.Email.IsValid == false)
                    EmailEntry.Focus();
                else if (vm.Password.IsValid == false)
                    PasswordEntry.Focus();

                return;
            }

            if (CrossConnectivity.Current.IsConnected == false)
            {
                vm.ErrorMessage = "No connection";
                return;
            }

            // Attempt  to login using the API
            //
            vm.IsBusy = true;
            var login = await RaceDayV2Client.Login(vm.Email.Value.Trim(), vm.Password.Value.Trim());
            if (login != null)
            {
                Settings.UserId = login.userid;
                Settings.UserEmail = login.email;
                Settings.UserPassword = vm.Password.Value.Trim();
                Settings.UserFirstName = login.firstname;
                Settings.UserLastName = login.lastname;
                Settings.UserName = login.name;

                Settings.Token = new Model.AccessToken
                {
                    Token = login.token,
                    Expiration = login.expiration,
                    Role = login.role
                };

                Analytics.TrackEvent("Login",
                    new Dictionary<string, string>
                    {
                    { "UID", Settings.UserId },
                    { "Email", Settings.UserEmail }
                    });

                await Navigation.PushAsync(new EventTabs(), false);
            }
            else
            {
                vm.ErrorMessage = "Unable to login with email/password";
                vm.Email.IsValid = false;
                vm.Password.IsValid = false;
            }
            vm.IsBusy = false;
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