using RaceDay.Helpers;
using RaceDay.ViewModel;
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
    public partial class SettingsView : ContentPage
    {
        public SettingsView()
        {
            InitializeComponent();

            var vm = new SettingsViewModel
            {
                NotifyNewRace = Settings.NotifyNewRace,
                NotifyParticipantJoins = Settings.NotifyParticipantJoins,
                UserName = Settings.UserName
            };
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LogoutButton.Clicked += LogoutButton_Clicked;
            HelpButton.Clicked += HelpButton_Clicked;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            LogoutButton.Clicked -= LogoutButton_Clicked;
            HelpButton.Clicked -= HelpButton_Clicked;

            var vm = BindingContext as SettingsViewModel;
            Settings.NotifyNewRace = vm.NotifyNewRace;
            Settings.NotifyParticipantJoins = vm.NotifyParticipantJoins;
        }

        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            Settings.UserId = string.Empty;
            Settings.UserName = "Your Account";
            Settings.UserEmail = string.Empty;

            await Navigation.PushAsync(new InfoMain());
        }

        private async void HelpButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new InfoTips());
        }
    }
}