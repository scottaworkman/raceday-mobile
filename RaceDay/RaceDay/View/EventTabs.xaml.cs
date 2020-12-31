using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaceDay.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RaceDay.Model;
using RaceDay.Helpers;

namespace RaceDay.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventTabs : TabbedPage
    {
        EventsViewModel vm { get; set; }
        Boolean isStartup = true;
        DateTime lastRefresh = DateTime.MinValue;

        public EventTabs()
        {
            InitializeComponent();

            // Set Tabs to Accent color
            //
            if (Device.RuntimePlatform == Device.Android)
            {
                if (Application.Current.UserAppTheme == OSAppTheme.Light ||
                    (Application.Current.UserAppTheme == OSAppTheme.Unspecified && Application.Current.RequestedTheme == OSAppTheme.Light))
                {
                    BarBackgroundColor = Color.FromHex("ff03A9F4");
                }
                else
                {
                    BarBackgroundColor = Color.FromHex("ff757575");
                }
            }
            else
            {
                if (Application.Current.UserAppTheme == OSAppTheme.Light ||
                    (Application.Current.UserAppTheme == OSAppTheme.Unspecified && Application.Current.RequestedTheme == OSAppTheme.Light))
                {
                    BarBackgroundColor = Color.FromHex("ffEBEBEB");
                }
                else
                {
                    BarBackgroundColor = Color.FromHex("ff757575");
                }
            }

            // Shared ViewModel as both have same list as source (one with a filter)
            //
            vm = new EventsViewModel();

            // Toolbar Items
            //
            Title = "JYMF Events";
            if (Device.RuntimePlatform != Device.Android)
            {
                ToolbarItems.Add(new ToolbarItem()
                {
                    IconImageSource = "ic_add_circle.png",
                    Text = "New Event",
                    Order = ToolbarItemOrder.Primary,
                    Command = new Command(() =>
                    {
                        Event emptyEvent = new Event()
                        {
                            Name = string.Empty,
                            Date = DateTime.Now.Date,
                            Location = string.Empty,
                            Url = string.Empty,
                            Description = string.Empty
                        };
                        Navigation.PushAsync(new AddEvent(emptyEvent, vm));
                    })
                });
            }
            ToolbarItems.Add(new ToolbarItem()
            {
                IconImageSource = "ic_autorenew.png",
                Text = "Refresh",
                Order = ToolbarItemOrder.Primary,
                Command = new Command(() =>
                {
                    if (vm.GetEventsCommand.CanExecute(this))
                        vm.GetEventsCommand.Execute(this);
                })
            });
            ToolbarItems.Add(new ToolbarItem()
            {
                IconImageSource = "ic_settings.png",
                Text = "Settings",
                Order = ToolbarItemOrder.Primary,
                Command = new Command(() =>
                {
                    Navigation.PushAsync(new SettingsView());
                })
            });

            // Add the tab content pages
            //
            Children.Add(new AllEvents(vm) { Title = "ALL EVENTS", IconImageSource = (Device.RuntimePlatform == Device.iOS ? "ic_group.png": string.Empty) });
            Children.Add(new MyEvents(vm) { Title = "MY EVENTS", IconImageSource = (Device.RuntimePlatform == Device.iOS ? "ic_person.png" : string.Empty) });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Populate lists on page creation
            //
            if ((isStartup) || (DateTime.Now.AddDays(-1) > lastRefresh))
            {
                if (vm.GetEventsCommand.CanExecute(this))
                    vm.GetEventsCommand.Execute(this);

                lastRefresh = DateTime.Now;
            }

            // Make main page if coming from a login
            //
            if (isStartup)
            {
                ResetNavigationStack();
                isStartup = false;
            }

            // Display Tips
            //
            if (Settings.HideInformation == false)
            {
                Settings.HideInformation = true;
                Navigation.PushModalAsync(new InfoTips());
            }
        }

        private void ResetNavigationStack()
        {
            if (Navigation != null && Navigation.NavigationStack.Count() > 1)
            {
                while(Navigation.NavigationStack.Count() > 1)
                {
                    Navigation.RemovePage(Navigation.NavigationStack[0]);
                }
            }
        }
    }
}