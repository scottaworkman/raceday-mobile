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
using RaceDay.ViewModel.Base;
using Microsoft.AppCenter.Analytics;

namespace RaceDay.ViewModel
{
    /// <summary>
    /// Shared View Model so that changes to detail pages can be shared back to the list of events
    /// </summary>
    /// 
    public class EventsViewModel : ExtendedBindableObject, INotifyPropertyChanged
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
        public Command DeleteEventItemCommand { get; set; }
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
            DeleteEventItemCommand = new Command<int>(
                async (eventId) => await DeleteEventItem(eventId),
                (eventId) => !IsBusy);
            UpdateEventCommand = new Command<ContentPage>(
                async (page) => await UpdateEvent(page),
                (page) => !IsBusy);
        }

        /// <summary>
        /// Busy flag to prevent multiple calls while the async tasks are working
        /// </summary>
        /// 
        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);

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
                var items = await RaceDayV2Client.GetAllEventsForCurrentUser();

                Events.Clear();
                MyEvents.Clear();
                foreach (var item in items)
                {
                    var racedayEvent = new Event
                    {
                        EventId = item.EventId,
                        GroupId = item.GroupId,
                        Name = item.Name,
                        Date = item.Date,
                        Url = item.Url,
                        Location = item.Location,
                        Description = item.Description,
                        CreatorId = item.CreatorId,
                        UserId = item.UserId,
                        Attending = (item.Attending != 0),
                        AttendanceCount = item.AttendanceCount
                    };

                    Events.Add(racedayEvent);
                    if (racedayEvent.Attending)
                        MyEvents.Add(racedayEvent);
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
                if (EventInfo.Attending)
                {
                    if (await RaceDayV2Client.AddUserToEvent(EventInfo.EventId) == true)
                    {
                        EventInfo.AttendanceCount++;
                    }
                }
                else
                {
                    await RaceDayV2Client.RemoveUserFromEvent(EventInfo.EventId);
                    if (EventInfo.AttendanceCount > 0 )
                    {
                        EventInfo.AttendanceCount--;
                    }
                }

                OnPropertyChanged(nameof(EventInfo));
                UpdateEvent();
                await UpdateParticipantsList();

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
                var newEvent = await RaceDayV2Client.AddEvent(
                                            EventInfo.Name,
                                            EventInfo.Date,
                                            EventInfo.Url,
                                            EventInfo.Location,
                                            EventInfo.Description,
                                            EventInfo.CreatorId);

                OnPropertyChanged(nameof(EventInfo));

                var addEvent = new Event
                {
                    EventId = newEvent.eventinfo.EventId,
                    Name = newEvent.eventinfo.Name,
                    Date = newEvent.eventinfo.Date,
                    Url = newEvent.eventinfo.Url,
                    Location = newEvent.eventinfo.Location,
                    Description = newEvent.eventinfo.Description,
                    CreatorId = newEvent.eventinfo.CreatorId,
                    AttendanceCount = 1,
                    Attending = true
                };

                Events.AddEvent(addEvent);
                MyEvents.AddEvent(addEvent);

                Analytics.TrackEvent("Event Added",
                    new Dictionary<string, string> {
                        { "Name", newEvent.eventinfo.Name },
                        { "Date", newEvent.eventinfo.Date.ToString("MM/dd/yyyy") },
                        { "UID", newEvent.eventinfo.CreatorId } }
                    );

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

                if (await RaceDayV2Client.DeleteEvent(EventInfo.EventId) == false)
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

        async Task DeleteEventItem(int eventId)
        {
            await ExecuteCommand(async () =>
            {
                if (await RaceDayV2Client.DeleteEvent(eventId) == false)
                {
                    await App.Current.MainPage.DisplayAlert("Application Error", "Unable to delete event", "Ok");
                    return;
                }

                Events.DeleteEvent(eventId);
                MyEvents.DeleteEvent(eventId);

                var snack = DependencyService.Get<ISnackbar>();
                await snack.Show(new SnackbarOptions { Text = "Event deleted", Duration = SnackbarDuration.Short });
                await Task.Delay(1500);
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


                if (await RaceDayV2Client.UpdateEvent(
                                            EventInfo.EventId,
                                            EventInfo.Name,
                                            EventInfo.Date,
                                            EventInfo.Url,
                                            EventInfo.Location,
                                            EventInfo.Description,
                                            EventInfo.CreatorId) == false)
                {
                    await page.DisplayAlert("Application Error", "Unable to update the event information", "Ok");
                    return;
                }

                OnPropertyChanged(nameof(EventInfo));
                Events.RefreshEvents();
                OnPropertyChanged(nameof(Events));
                MyEvents.RefreshEvents();
                OnPropertyChanged(nameof(MyEvents));

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
            var eventDetail = await RaceDayV2Client.GetEventDetail(EventInfo.EventId);

            Participants.Clear();
            foreach (var attendee in eventDetail.attendees)
            {
                var racer = new Participant
                {
                    FirstName = attendee.FirstName,
                    LastName = attendee.LastName,
                    Name = attendee.Name,
                    EMail = attendee.Email,
                    UserId = attendee.UserId,
                    Initials = attendee.FirstName.Left(1).ToUpper() + attendee.LastName.Left(1).ToUpper(),
                    Color = attendee.Name.ToColor(),
                };
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
    }
}
