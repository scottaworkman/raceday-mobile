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
using RaceDay.ViewModel.Base;

namespace RaceDay.ViewModel
{
    /// <summary>
    /// Simple view model for settings view
    /// </summary>
    /// 
    public class SettingsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public bool NotifyNewRace { get; set; }
        public bool NotifyParticipantJoins { get; set; }
        public string UserName { get; set; }
        public bool ThemeLight { get; set; }
        public bool ThemeDark { get; set; }
        public bool ThemeSystem { get; set; }

        public SettingsViewModel()
        {
        }
    }
}
