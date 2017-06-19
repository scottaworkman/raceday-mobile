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

namespace RaceDay.ViewModel
{
    /// <summary>
    /// Simple view model for settings view
    /// </summary>
    /// 
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public bool NotifyNewRace { get; set; }
        public bool NotifyParticipantJoins { get; set; }
        public string UserName { get; set; }

        public SettingsViewModel()
        {
        }

        /// <summary>
        /// Busy flag to prevent multiple calls while the async tasks are working
        /// </summary>
        /// 
        private bool busy = false;
        public bool IsBusy
        {
            get { return busy; }
            set
            {
                busy = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// PropertyChanged event handler used to identify UI when binding context changes
        /// </summary>
        /// 
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = null)
        {
            var changed = PropertyChanged;

            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
