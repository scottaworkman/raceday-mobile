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
using System.Collections.Generic;

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
        public Command DeleteEventCommand { get; set; }
        public Command UpdateEventCommand { get; set; }

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
            DeleteEventCommand = new Command<ContentPage>(
                async (page) => await DeleteEvent(page),
                (page) => !IsBusy);
            UpdateEventCommand = new Command<ContentPage>(
                async (page) => await UpdateEvent(page),
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
        /// Common method to check for internet connection for each method
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task<bool> IsConnected(bool mainError = false)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (mainError && (Application.Current.MainPage != null))
                    await Application.Current.MainPage.DisplayAlert("No Connection!", "No internet connection.  Internet is required to load event list.", "OK");
                else
                {
                    if (FAButton != null && FAButton.Hide != null)
                        FAButton.Hide();
                    var toast = DependencyService.Get<IToast>();
                    toast.Show(new ToastOptions { Text = "No internet connection", Duration = ToastDuration.Long });
                    await Task.Delay(toast.Duration());
                    if (FAButton != null && FAButton.Show != null)
                        FAButton.Show();
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// Common Task handler for commands.  Wraps the logic in common checks and error handling
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        /// 
        private async Task ExecuteCommand(Func<Task> action, bool mainError = false)
        {
            if (IsBusy)
                return;

            if (!await IsConnected(mainError))
                return;

            Exception error = null;
            try
            {
                IsBusy = true;

                await action();
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
        /// Retrieve the list of events and add to collection used as binding source
        /// </summary>
        /// 
        async Task GetEvents()
        {
            await ExecuteCommand(async () =>
            {
                var items = await RaceDayClient.GetEvents();

                Events.Clear();
                MyEvents.Clear();
                foreach (var item in items)
                {
                    Events.Add(item);
                    if (item.Attending)
                        MyEvents.Add(item);
                }
                return;
            }, true);
        }

        /// <summary>
        /// Retrieve the list of participants for a given event
        /// </summary>
        /// 
        async Task GetParticipants()
        {
            await ExecuteCommand(async () =>
            {
                await UpdateParticipantsList();
                return;
            });
        }

        /// <summary>
        /// Send the change of attendance to the server and update the event with the change in participation
        /// </summary>
        /// <returns></returns>
        /// 
        async Task ChangeAttendance()
        {
            await ExecuteCommand(async () =>
            {
                var result = await RaceDayClient.Attending(EventInfo.EventId, EventInfo.Attending);
                if (result == false)
                    EventInfo.Attending = !EventInfo.Attending;
                else
                {
                    OnPropertyChanged(nameof(EventInfo));
                    UpdateEvent();
                    await UpdateParticipantsList();
                }
                return;
            });
        }

        /// <summary>
        /// Command to add new event through API.  Data was validated from the view
        /// </summary>
        /// <returns></returns>
        /// 
        async Task AddEvent(ContentPage page)
        {
            await ExecuteCommand(async () =>
            {
                var newEvent = await RaceDayClient.AddEvent(EventInfo);

                OnPropertyChanged(nameof(EventInfo));
                Events.AddEvent(newEvent.eventinfo);
                MyEvents.AddEvent(newEvent.eventinfo);

                HockeyApp.MetricsManager.TrackEvent("Event Added", 
                    new Dictionary<string, string> {
                        { "Name", newEvent.eventinfo.Name },
                        { "Date", newEvent.eventinfo.Date.ToString("MM/dd/yyyy") },
                        { "UID", newEvent.eventinfo.CreatorId } }, 
                    new Dictionary<string, double>());

                var snack = DependencyService.Get<ISnackbar>();
                await snack.Show(new SnackbarOptions { Text = "New event added", Duration = SnackbarDuration.Short });
                await Task.Delay(1500);

                await page.Navigation.PopAsync();
                return;
            });
        }

        /// <summary>
        /// Command to delete event through API.
        /// </summary>
        /// <returns></returns>
        /// 
        async Task DeleteEvent(ContentPage page)
        {
            await ExecuteCommand(async () =>
            {
                var answer = await page.DisplayAlert("Delete Event?", "Are you sure you want to delete this event", "Yes", "No");
                if (answer == false)
                    return;

                if (await RaceDayClient.DeleteEvent(EventInfo.EventId) == false)
                {
                    await page.DisplayAlert("Application Error", "Unable to delete event", "Ok");
                    return;
                }

                Events.DeleteEvent(EventInfo.EventId);
                MyEvents.DeleteEvent(EventInfo.EventId);

                var snack = DependencyService.Get<ISnackbar>();
                await snack.Show(new SnackbarOptions { Text = "Event deleted", Duration = SnackbarDuration.Short });
                await Task.Delay(1500);

                await page.Navigation.PopAsync();
                return;
            });
        }

        /// <summary>
        /// Command to update event through API.  Data was validated from the view
        /// </summary>
        /// <returns></returns>
        /// 
        async Task UpdateEvent(ContentPage page)
        {
            await ExecuteCommand(async () =>
            {
                if (await RaceDayClient.UpdateEvent(EventInfo) == false)
                {
                    await page.DisplayAlert("Application Error", "Unable to update the event information", "Ok");
                    return;
                }

                OnPropertyChanged(nameof(EventInfo));
                Events.RefreshEvents();
                MyEvents.RefreshEvents();

                var snack = DependencyService.Get<ISnackbar>();
                await snack.Show(new SnackbarOptions { Text = "Event information updated", Duration = SnackbarDuration.Short });
                await Task.Delay(1500);

                await page.Navigation.PopAsync();
                return;
            });
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
        /// Validate form fields.  Name and Date are required and Date must be in the future.
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task<Boolean> ValidateEvent()
        {
            if (string.IsNullOrEmpty(EventInfo.Name.Trim()))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Event Name is required. Please specify the name of the event.", "Ok");
                return false;
            }

            if (EventInfo.Date < DateTime.Now.Date)
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Event Date must be an upcoming date.", "Ok");
                return false;
            }

            // URL must start with http: or https:
            //
            if (!string.IsNullOrEmpty(EventInfo.Url.Trim()) && EventInfo.Url.ToLower().StartsWith("http://") == false && EventInfo.Url.ToLower().StartsWith("https://") == false)
                EventInfo.Url = "http://" + EventInfo.Url;

            return true;
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
