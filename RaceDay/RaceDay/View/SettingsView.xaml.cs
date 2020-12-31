using RaceDay.Helpers;
using RaceDay.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;


namespace RaceDay.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : ContentPage
    {
        public SettingsView()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            var vm = new SettingsViewModel
            {
                NotifyNewRace = Settings.NotifyNewRace,
                NotifyParticipantJoins = Settings.NotifyParticipantJoins,
                UserName = Settings.UserName,
                ThemeLight = Settings.AppTheme == OSAppTheme.Light,
                ThemeDark = Settings.AppTheme == OSAppTheme.Dark,
                ThemeSystem = Settings.AppTheme == OSAppTheme.Unspecified
            };
            BindingContext = vm;

            // toolbar items
            //
            ToolbarItems.Add(new ToolbarItem()
            {
                IconImageSource = "ic_profile.png",
                Text = "Profile",
                Order = ToolbarItemOrder.Primary,
                Command = new Command(() =>
                {
                    Navigation.PushModalAsync(new AccountProfile());
                })
            });
            ToolbarItems.Add(new ToolbarItem()
            {
                IconImageSource = "ic_password.png",
                Text = "Password",
                Order = ToolbarItemOrder.Primary,
                Command = new Command(() =>
                {
                    Navigation.PushModalAsync(new AccountPassword());
                })
            });
            ToolbarItems.Add(new ToolbarItem()
            {
                IconImageSource = "ic_logout.png",
                Text = "Logout",
                Order = ToolbarItemOrder.Primary,
                Command = new Command(async () =>
                {
                    if (await DisplayAlert("Logout", "Do you wish to logoout?", "Yes", "No"))
                    {
                        Settings.UserId = string.Empty;
                        Settings.UserName = "Your Account";
                        Settings.UserEmail = string.Empty;
                        Settings.UserPassword = string.Empty;

                        await Navigation.PushAsync(new InfoMain());
                    }
                })
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            HelpButton.Clicked += HelpButton_Clicked;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            HelpButton.Clicked -= HelpButton_Clicked;

            var vm = BindingContext as SettingsViewModel;
            Settings.NotifyNewRace = vm.NotifyNewRace;
            Settings.NotifyParticipantJoins = vm.NotifyParticipantJoins;
        }

        private async void HelpButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new InfoTips());
        }

        private void AppTheme_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var vm = BindingContext as SettingsViewModel;
            if (vm.ThemeLight)
            {
                Settings.AppTheme = OSAppTheme.Light;
            }
            else if (vm.ThemeDark)
            {
                Settings.AppTheme = OSAppTheme.Dark;
            }
            else
            {
                Settings.AppTheme = OSAppTheme.Unspecified;
            }
            Xamarin.Forms.Application.Current.UserAppTheme = Settings.AppTheme;
        }
    }
}