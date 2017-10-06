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
        private bool isNewEvent;
        private EventsViewModel eventListView;
        private Event eventDetail;

        public AddEvent(Event selectedEvent, EventsViewModel evm)
        {
            InitializeComponent();

            txtDate.MinimumDate = DateTime.Now.Date;

            isNewEvent = string.IsNullOrEmpty(selectedEvent.Name);
            Title = (isNewEvent ? "Add New Event" : "Update Event");

            EditViewModel vm = new EditViewModel();
            if (!isNewEvent)
            {
                vm.EventName.Value = selectedEvent.Name;
                vm.EventDate.Value = selectedEvent.Date;
                vm.EventLocation.Value = selectedEvent.Location;
                vm.EventUrl.Value = selectedEvent.Url;
                vm.EventDescription.Value = selectedEvent.Description;
            }

            eventListView = evm;
            eventDetail = selectedEvent;

            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            btnCancel.Clicked += BtnCancel_Clicked;
            btnOk.Clicked += BtnOk_Clicked;

            txtEventName.Focus();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            btnCancel.Clicked -= BtnCancel_Clicked;
            btnOk.Clicked -= BtnOk_Clicked;
        }

        private void BtnOk_Clicked(object sender, EventArgs e)
        {
            EditViewModel vm = BindingContext as EditViewModel;

            // Data Cleanup
            //
            if (!string.IsNullOrEmpty(vm.EventUrl.Value.Trim()) && vm.EventUrl.Value.ToLower().StartsWith("http://") == false && vm.EventUrl.Value.ToLower().StartsWith("https://") == false)
                vm.EventUrl.Value = "http://" + vm.EventUrl.Value;

            // Check for placeholder text in the description from the iOS renderer
            //
            if (vm.EventDescription.Value == txtDescription.Placeholder)
                vm.EventDescription.Value = string.Empty;

            // Validation must occur here as we can't wait on the command
            //
            if (vm.Validate() == false)
            {
                if (vm.EventName.IsValid == false)
                    txtEventName.Focus();
                else if (vm.EventDate.IsValid == false)
                    txtDate.Focus();
                else if (vm.EventUrl.IsValid == false)
                    txtUrl.Focus();

                return;
            }

            // Setup the parent list view with the information from th edit view model
            //
            eventDetail.Name = vm.EventName.Value.Trim();
            eventDetail.Date = vm.EventDate.Value;
            eventDetail.Location = vm.EventLocation.Value.Trim();
            eventDetail.Url = vm.EventUrl.Value.Trim();
            eventDetail.Description = vm.EventDescription.Value.Trim();

            eventListView.EventInfo = eventDetail;

            // Validation passed, call the command
            //
            if (isNewEvent)
            {
                if (eventListView.AddEventCommand.CanExecute(this))
                    eventListView.AddEventCommand.Execute(this);
            }
            else
            {
                if (eventListView.UpdateEventCommand.CanExecute(this))
                    eventListView.UpdateEventCommand.Execute(this);
            }
        }

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}