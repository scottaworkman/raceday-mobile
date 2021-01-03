using RaceDay.Model;
using RaceDay.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using System.ComponentModel;

namespace RaceDay.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyEvents : ContentPage, INotifyPropertyChanged
    {
        public MyEvents(EventsViewModel vm)
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            vm.IsBusy = false;
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