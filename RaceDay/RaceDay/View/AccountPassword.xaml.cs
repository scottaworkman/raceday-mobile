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
    public partial class AccountPassword: ContentPage
    {
        public AccountPassword()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            var vm = new RegisterViewModel(RegisterViewModel.ModelType.Password);
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
                if (vm.Password.IsValid == false)
                    PasswordEntry.Focus();
                else if (vm.ConfirmPassword.IsValid == false)
                    ConfirmPasswordEntry.Focus();

                return;
            }

            if (vm.UpdatePasswordCommand.CanExecute(this))
            {
                vm.UpdatePasswordCommand.Execute(this);
            }
        }
    }
}