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
        public InfoMain()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginView());
        }
    }
}