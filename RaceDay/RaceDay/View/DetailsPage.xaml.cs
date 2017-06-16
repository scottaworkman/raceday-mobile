using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Connectivity;
using RaceDay.Model;
using RaceDay.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RaceDay.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsPage : ContentPage
    {
        public DetailsPage(Event selectedEvent, EventsViewModel evm)
        {
            InitializeComponent();

            evm.EventInfo = selectedEvent;
            BindingContext = evm;

            this.Title = selectedEvent.Name;

            LocationLabel.IsVisible = !string.IsNullOrEmpty(selectedEvent.Location);
            DescriptionLabel.IsVisible = !string.IsNullOrEmpty(selectedEvent.Description);
            UrlLabel.IsVisible = !string.IsNullOrEmpty(selectedEvent.Url);
            UrlLabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Device.OpenUri(new Uri(UrlLabel.Text));
                })
            });
            if (!CrossConnectivity.Current.IsConnected)
                AttendingSwitch.IsEnabled = false;

            if (evm.GetParticipantsCommand.CanExecute(this))
                evm.GetParticipantsCommand.Execute(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            AttendingSwitch.Toggled += AttendingSwitch_Toggled;
            RacersListView.ItemSelected += RacersListView_ItemSelected;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            AttendingSwitch.Toggled -= AttendingSwitch_Toggled;
            RacersListView.ItemSelected += RacersListView_ItemSelected;
        }

        /// <summary>
        /// Attending toggle has been changed.  Send event to server and if the response matches, then change necessary properties to 
        /// viewmodel in order to be reflected on screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void AttendingSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            var vm = BindingContext as EventsViewModel;
            vm.EventInfo.Attending = e.Value;

            if (vm.GetAttendingCommand.CanExecute(this))
                vm.GetAttendingCommand.Execute(this);
        }

        private void RacersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // MUST turn off the selected item or will crash app when returning to page on another event
            RacersListView.SelectedItem = null;
        }

    }
}