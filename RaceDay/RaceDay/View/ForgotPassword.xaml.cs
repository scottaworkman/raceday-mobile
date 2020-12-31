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
using System.Net;

namespace RaceDay.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPassword : ContentPage
    {
        public ForgotPassword()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            var vm = new PasswordViewModel();
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            PasswordButton.Clicked += PasswordButton_Clicked;
        }

        private async void PasswordButton_Clicked(object sender, EventArgs e)
        {
            var vm = BindingContext as PasswordViewModel;

            if (vm.Validate() == false)
            {
                if (vm.Email.IsValid == false)
                    EmailEntry.Focus();

                return;
            }

            if (CrossConnectivity.Current.IsConnected == false)
            {
                vm.ErrorMessage = "No connection";
                return;
            }

            // Send password using the API
            //
            vm.IsBusy = true;
            var statusCode = await RaceDayV2Client.ForgotPassword(vm.Email.Value.Trim());
            if (statusCode == HttpStatusCode.OK)
            {
                vm.IsValid = false;
                vm.ErrorMessage = $"Password is being sent to {vm.Email.Value}";
                ErrorMessage.TextColor = (Xamarin.Forms.Application.Current.UserAppTheme == OSAppTheme.Light || (Xamarin.Forms.Application.Current.UserAppTheme == OSAppTheme.Unspecified && Xamarin.Forms.Application.Current.RequestedTheme == OSAppTheme.Light) ? Color.ForestGreen : Color.White);
            }
            else
            {
                vm.ErrorMessage = $"Unable to find password for {vm.Email.Value}";
                vm.Email.IsValid = false;
                ErrorMessage.TextColor = (Xamarin.Forms.Application.Current.UserAppTheme == OSAppTheme.Light || (Xamarin.Forms.Application.Current.UserAppTheme == OSAppTheme.Unspecified && Xamarin.Forms.Application.Current.RequestedTheme == OSAppTheme.Light) ? Color.FromHex("#ff5252") : Color.FromHex("#ff0000"));
            }
            vm.IsBusy = false;
        }
    }
}