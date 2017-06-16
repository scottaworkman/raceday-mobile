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
    public partial class MyEvents : ContentPage
    {
        public MyEvents(EventsViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;
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