using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaceDay.Model;
using RaceDay.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RaceDay.Helpers;

namespace RaceDay.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllEvents : ContentPage
    {
        public AllEvents(EventsViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;

            // Floating Action Button layout
            //
            if (Device.RuntimePlatform == Device.Android)
            {
                var fab = new FloatingActionButtonView()
                {
                    ImageName = "ic_add.png",
                    ColorNormal = Color.FromHex("ff03A9F4"),
                    ColorRipple = Color.FromHex("fffb8c00"),
                };
                vm.FAButton = fab;

                fab.Clicked = async (sender, args) =>
                {
                    Event emptyEvent = new Event()
                    {
                        Name = string.Empty,
                        Date = DateTime.Now.Date,
                        Location = string.Empty,
                        Url = string.Empty,
                        Description = string.Empty
                    };
                    await Navigation.PushAsync(new AddEvent(emptyEvent, vm));
                };

                AbsoluteLayout.SetLayoutFlags(fab, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(fab, new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

                absLayout.Children.Add(fab);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ListViewEvents.ItemSelected += ListViewEvents_ItemSelected;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ListViewEvents.ItemSelected -= ListViewEvents_ItemSelected;
        }

        private async void ListViewEvents_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var race = e.SelectedItem as Event;
            if (race == null)
                return;

            await Navigation.PushAsync(new DetailsPage(race, BindingContext as EventsViewModel));

            ListViewEvents.SelectedItem = null;
        }

        /// <summary>
        /// Allow app admins to delete events from the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private async void ListViewEvents_Delete(object sender, EventArgs e)
        {
            try
            {
                MenuItem item = (MenuItem)sender;
                if (item.CommandParameter is Event race)
                {
                    if (await this.DisplayAlert("Delete Event?",
                          "Are you sure you want to delete '" + race.Name + "'?", "Yes", "Cancel") == true)
                    {
                        if (BindingContext is EventsViewModel vm)
                        {
                            if (vm.DeleteEventItemCommand.CanExecute(race.EventId))
                                vm.DeleteEventItemCommand.Execute(race.EventId);
                        }
                    }
                }
            }
            finally
            {
                ListViewEvents.SelectedItem = null;
            }
        }

        /// <summary>
        /// Allow app admins or event creators to edit the event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void ListViewEvents_Edit(object sender, EventArgs e)
        {
            try
            {
                MenuItem item = (MenuItem)sender;
                if (item.CommandParameter is Event race)
                {
                    if (BindingContext is EventsViewModel vm)
                    {
                        vm.EventInfo = race;
                        Navigation.PushAsync(new AddEvent(vm.EventInfo, vm));
                    }
                }
            }
            finally
            {
                ListViewEvents.SelectedItem = null;
            }
        }

        /// <summary>
        /// Conditionally, turn on/off the delete menu item depending on permissions of the user.  Must be an admin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void ListViewEvents_BindingContextChanged(object sender, EventArgs e)
        {
            base.OnBindingContextChanged();

            if (BindingContext == null)
                return;

            if (sender is ViewCell eventCell)
            {
                foreach (var action in eventCell.ContextActions)
                {
                    if (action.Text == "Delete")
                        action.Clicked -= ListViewEvents_Delete;
                    else if (action.Text == "Edit")
                        action.Clicked -= ListViewEvents_Edit;
                }
                eventCell.ContextActions.Clear();

                if ((Device.RuntimePlatform == Device.Android) &&
                    ((Settings.AccessRole == (int)Settings.ApplicationRole.Admin) || ((eventCell.BindingContext != null) && (Settings.UserId == ((Event)eventCell.BindingContext).CreatorId))))
                {
                    var menu = new MenuItem()
                    {
                        Text = "Edit",
                        Icon = "ic_create.png",
                        IsDestructive = false,
                        CommandParameter = eventCell.BindingContext
                    };
                    menu.Clicked += ListViewEvents_Edit;
                    eventCell.ContextActions.Add(menu);
                }

                if (Settings.AccessRole == (int)Settings.ApplicationRole.Admin)
                {
                    var menu = new MenuItem()
                    {
                        Text = "Delete",
                        Icon = "ic_delete.png",
                        IsDestructive = true,
                        CommandParameter = eventCell.BindingContext
                    };
                    menu.Clicked += ListViewEvents_Delete;
                    eventCell.ContextActions.Add(menu);
                }
            }
        }
    }
}