using RaceDay.Model;
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
    public partial class AddEvent : ContentPage
    {
        public AddEvent(Event selectedEvent, EventsViewModel evm)
        {
            InitializeComponent();

            txtDate.MinimumDate = DateTime.Now.Date;

            evm.EventInfo = selectedEvent;
            BindingContext = evm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            btnCancel.Clicked += BtnCancel_Clicked;
            btnOk.Clicked += BtnOk_Clicked;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            btnCancel.Clicked -= BtnCancel_Clicked;
            btnOk.Clicked -= BtnOk_Clicked;
        }

        private async void BtnOk_Clicked(object sender, EventArgs e)
        {
            EventsViewModel vm = BindingContext as EventsViewModel;

            // Validation must occur here as we can't wait on the command
            //
            if (string.IsNullOrEmpty(vm.EventInfo.Name.Trim()))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Event Name is required. Please specify the name of the event.", "Ok");
                txtEventName.Focus();
                return;
            }

            if (vm.EventInfo.Date < DateTime.Now.Date)
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Event Date must be an upcoming date.", "Ok");
                txtDate.Focus();
                return;
            }

            // Validation passed, call the command
            //
            if (vm.AddEventCommand.CanExecute(this))
                vm.AddEventCommand.Execute(this);
        }

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}