using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Plugin.Connectivity;
using RaceDay.Model;
using RaceDay.Services;
using Xamarin.Forms;
using RaceDay.Helpers;

namespace RaceDay.ViewModel
{
    /// <summary>
    /// Shared View Model so that changes to detail pages can be shared back to the list of events
    /// </summary>
    /// 
    public class EventsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Event> Events { get; set; }
        public ObservableCollection<Event> MyEvents { get; set; }

        public Event EventInfo { get; set; }
        public ObservableCollection<Participant> Participants { get; set; }

        public Command GetEventsCommand { get; set; }
        public Command GetParticipantsCommand { get; set; }
        public Command GetAttendingCommand { get; set; }
        public Command AddEventCommand { get; set; }

        public FloatingActionButtonView FAButton { get; set; }

        /// <summary>
        /// Initialize the container for the list of events used as a binding source.
        /// Define the GetEventsCommand to retrieve the events when the ViewModel is not busy
        /// </summary>
        /// 
        public EventsViewModel()
        {
            Events = new ObservableCollection<Event>();
            MyEvents = new ObservableCollection<Event>();
            Participants = new ObservableCollection<Participant>();

            GetEventsCommand = new Command(
                async () => await GetEvents(),
                () => !IsBusy);
            GetParticipantsCommand = new Command(
                async () => await GetParticipants(),
                () => !IsBusy);
            GetAttendingCommand = new Command(
                async () => await ChangeAttendance(),
                () => !IsBusy);
            AddEventCommand = new Command<ContentPage>(
                async (page) => await AddEvent(page),
                (page) => !IsBusy);
        }

        /// <summary>
        /// Busy flag to prevent multiple calls while the async tasks are working
        /// </summary>
        /// 
        private bool busy = false;
        public bool IsBusy
        {
            get { return busy; }
            set
            {
                busy = value;
                OnPropertyChanged();

                GetEventsCommand.ChangeCanExecute();
                GetParticipantsCommand.ChangeCanExecute();
                GetAttendingCommand.ChangeCanExecute();
            }
        }

        /// <summary>
        /// Retrieve the list of events and add to collection used as binding source
        /// </summary>
        /// 
        async Task GetEvents()
        {
            if (IsBusy)
                return;

            if (!CrossConnectivity.Current.IsConnected)
            {
                if (Application.Current.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("No Connection!", "You do not have internet connectivity to retrieve data.", "OK");
                else
                {
                    if (FAButton != null)
                        FAButton.Hide();
                    var toast = DependencyService.Get<IToast>();
                    toast.Show(new ToastOptions { Text = "No internet connection", Duration = ToastDuration.Long });
                    await Task.Delay(toast.Duration());
                    if (FAButton != null)
                        FAButton.Show();
                }
                return;
            }

            Exception error = null;
            try
            {
                IsBusy = true;

                var items = await RaceDayClient.GetEvents();

                Events.Clear();
                MyEvents.Clear();
                foreach (var item in items)
                {
                    Events.Add(item);
                    if (item.Attending)
                        MyEvents.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                error = ex;
            }
            finally
            {
                IsBusy = false;
            }

            if (error != null)
                await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
        }

        /// <summary>
        /// Retrieve the list of participants for a given event
        /// </summary>
        /// 
        async Task GetParticipants()
        {
            if (IsBusy)
                return;

            if (!CrossConnectivity.Current.IsConnected)
            {
                var toast = DependencyService.Get<IToast>();
                toast.Show(new ToastOptions { Text = "No internet connection", Duration = ToastDuration.Long });
                await Task.Delay(toast.Duration());
                return;
            }

            Exception error = null;
            try
            {
                IsBusy = true;

                await UpdateParticipantsList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                error = ex;
            }
            finally
            {
                IsBusy = false;
            }

            if (error != null)
                await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
        }

        /// <summary>
        /// Send the change of attendance to the server and update the event with the change in participation
        /// </summary>
        /// <returns></returns>
        /// 
        async Task ChangeAttendance()
        {
            if (IsBusy)
                return;

            if (!CrossConnectivity.Current.IsConnected)
            {
                var toast = DependencyService.Get<IToast>();
                toast.Show(new ToastOptions { Text = "No internet connection", Duration = ToastDuration.Long });
                await Task.Delay(toast.Duration());
                return;
            }

            Exception error = null;
            try
            {
                IsBusy = true;

                var result = await RaceDayClient.Attending(EventInfo.EventId, EventInfo.Attending);
                if (result == false)
                    EventInfo.Attending = !EventInfo.Attending;
                else
                {
                    OnPropertyChanged(nameof(EventInfo));
                    UpdateEvent();
                    await UpdateParticipantsList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                error = ex;
            }
            finally
            {
                IsBusy = false;
            }

            if (error != null)
                await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
        }

        /// <summary>
        /// Command to add new event through API.  Data was validated from the view
        /// </summary>
        /// <returns></returns>
        /// 
        async Task AddEvent(ContentPage page)
        {
            if (IsBusy)
                return;

            if (!CrossConnectivity.Current.IsConnected)
            {
                var toast = DependencyService.Get<IToast>();
                toast.Show(new ToastOptions { Text = "No internet connection", Duration = ToastDuration.Long });
                await Task.Delay(toast.Duration());
                return;
            }

            Exception error = null;
            try
            {
                IsBusy = true;

                await page.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                error = ex;
            }
            finally
            {
                IsBusy = false;
            }

            if (error != null)
                await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
        }

        /// <summary>
        /// Shared method to update list of participants on an event detail page
        /// </summary>
        /// <returns></returns>
        /// 
        async Task UpdateParticipantsList()
        {
            var racers = await RaceDayClient.GetEventParticipants(EventInfo.EventId);

            Participants.Clear();
            foreach (var racer in racers)
            {
                racer.ImageUrl = String.Format("https://graph.facebook.com/{0}/picture?type=square", racer.UserId);
                Participants.Add(racer);
            }
        }

        /// <summary>
        /// Update the Event lists based on the change of event attendance
        /// </summary>
        /// 
        void UpdateEvent()
        {
            // Refresh the Events list to update the data binding
            //
            Event[] mirrorEvents = new Event[Events.Count];
            Events.CopyTo(mirrorEvents, 0);
            Events.Clear();
            foreach(var _event in mirrorEvents)
            {
                Events.Add(_event);
            }

            // Either add/remove from MyEvents
            //
            if (EventInfo.Attending)
            {
                MyEvents.AddEvent(EventInfo);
            }
            else
            {
                MyEvents.DeleteEvent(EventInfo.EventId);
            }
        }

        /// <summary>
        /// PropertyChanged event handler used to identify UI when binding context changes
        /// </summary>
        /// 
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = null)
        {
            var changed = PropertyChanged;

            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
