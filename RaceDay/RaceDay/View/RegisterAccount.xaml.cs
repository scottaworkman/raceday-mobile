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
    public partial class RegisterAccount : ContentPage
    {
        public RegisterAccount()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            var vm = new RegisterViewModel(RegisterViewModel.ModelType.Register);
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            RegisterButton.Clicked += RegisterButton_Clicked;
        }

        private void RegisterButton_Clicked(object sender, EventArgs e)
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
                else if (vm.Password.IsValid == false)
                    PasswordEntry.Focus();
                else if (vm.ConfirmPassword.IsValid == false)
                    ConfirmPasswordEntry.Focus();
                else if (vm.GroupCode.IsValid == false)
                    GroupCodeEntry.Focus();

                return;
            }

            if (vm.RegisterAccountCommand.CanExecute(this))
            {
                vm.RegisterAccountCommand.Execute(this);
            }
        }
    }
}