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
    public partial class AccountProfile: ContentPage
    {
        public AccountProfile()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            var vm = new RegisterViewModel(RegisterViewModel.ModelType.Profile);
            vm.FirstName.Value = Settings.UserFirstName;
            vm.LastName.Value = Settings.UserLastName;
            vm.Email.Value = Settings.UserEmail;
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateButton.Clicked += UpdateButton_Clicked;
            CancelButton.Clicked += CancelButton_Clicked;
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void UpdateButton_Clicked(object sender, EventArgs e)
        {
            var vm = BindingContext as RegisterViewModel;
            
            if (vm.Validate() == false)
            {
                if (vm.FirstName.IsValid == false)
                    FirstNameEntry.Focus();
                else if (vm.LastName.IsValid == false)
                    LastNameEntry.Focus();
                else if (vm.Email.IsValid == false)
                    EmailEntry.Focus();

                return;
            }

            if (vm.UpdateProfileCommand.CanExecute(this))
            {
                vm.UpdateProfileCommand.Execute(this);
            }
        }
    }
}