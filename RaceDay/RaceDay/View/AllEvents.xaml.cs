using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaceDay.Model;
using RaceDay.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
    }
}