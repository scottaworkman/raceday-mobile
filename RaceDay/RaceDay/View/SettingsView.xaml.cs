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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            var vm = BindingContext as SettingsViewModel;
            Settings.NotifyNewRace = vm.NotifyNewRace;
            Settings.NotifyParticipantJoins = vm.NotifyParticipantJoins;
        }
    }
}