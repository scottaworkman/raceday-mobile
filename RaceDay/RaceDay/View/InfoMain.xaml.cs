using Plugin.Connectivity;
using RaceDay.Helpers;
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
    public partial class InfoMain : ContentPage
    {
        Boolean isStartup = true;
        public InfoMain()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (isStartup)
            {
                ResetNavigationStack();
                isStartup = false;
            }
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("No Connection!", "No internet connection.  Internet is required to login.", "OK");
                return;
            }

            await Navigation.PushAsync(new LoginView());
        }

        private void ResetNavigationStack()
        {
            if (Navigation != null && Navigation.NavigationStack.Count() > 1)
            {
                while (Navigation.NavigationStack.Count() > 1)
                {
                    Navigation.RemovePage(Navigation.NavigationStack[0]);
                }
            }
        }
    }
}