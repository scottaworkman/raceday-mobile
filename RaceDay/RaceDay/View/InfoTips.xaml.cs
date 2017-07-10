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
    public partial class InfoTips : ContentPage
    {
        public InfoTips()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void BtnNext_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}