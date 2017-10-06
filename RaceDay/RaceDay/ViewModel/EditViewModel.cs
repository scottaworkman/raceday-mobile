using RaceDay.Validation;
using RaceDay.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RaceDay.ViewModel
{
    public class EditViewModel : ViewModelBase, INotifyPropertyChanged
    {
        ValidatableObject<string> _eventName;
        ValidatableObject<DateTime> _eventDate;
        ValidatableObject<string> _eventUrl;
        ValidatableObject<string> _eventLocation;
        ValidatableObject<string> _eventDescription;
        private bool _isValid;

        public EditViewModel()
        {
            _eventName = new ValidatableObject<string>(string.Empty);
            _eventDate = new ValidatableObject<DateTime>(DateTime.Now);
            _eventUrl = new ValidatableObject<string>(string.Empty);
            _eventLocation = new ValidatableObject<string>(string.Empty);
            _eventDescription = new ValidatableObject<string>(string.Empty);

            AddValidations();
        }

        public ValidatableObject<string> EventName
        {
            get
            {
                return _eventName;
            }
            set
            {
                _eventName = value;
                RaisePropertyChanged(() => EventName);
            }
        }

        public ValidatableObject<DateTime> EventDate
        {
            get
            {
                return _eventDate;
            }
            set
            {
                _eventDate = value;
                RaisePropertyChanged(() => EventDate);
            }
        }

        public ValidatableObject<string> EventUrl
        {
            get
            {
                return _eventUrl;
            }
            set
            {
                _eventUrl = value;
                RaisePropertyChanged(() => EventUrl);
            }
        }

        public ValidatableObject<string> EventLocation
        {
            get
            {
                return _eventLocation;
            }
            set
            {
                _eventLocation = value;
                RaisePropertyChanged(() => EventLocation);
            }
        }

        public ValidatableObject<string> EventDescription
        {
            get
            {
                return _eventDescription;
            }
            set
            {
                _eventDescription = value;
                RaisePropertyChanged(() => EventDescription);
            }
        }

        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                _isValid = value;
                RaisePropertyChanged(() => IsValid);
            }
        }

        public ICommand ValidateEventNameCommand => new Command(() => ValidateEventName());
        public ICommand ValidateEventDateCommand => new Command(() => ValidateEventDate());
        public ICommand ValidateEventUrlCommand => new Command(() => ValidateEventUrl());

        public bool Validate()
        {
            bool isValidName = ValidateEventName();
            bool isValidDate = ValidateEventDate();
            bool isValidUrl = ValidateEventUrl();

            return isValidName && isValidDate && isValidUrl;
        }

        bool ValidateEventName()
        {
            return _eventName.Validate();
        }

        bool ValidateEventDate()
        {
            return _eventDate.Validate();
        }

        bool ValidateEventUrl()
        {
            return _eventUrl.Validate();
        }

        private void AddValidations()
        {
            _eventName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "The name of the event is required." });
            _eventDate.Validations.Add(new IsFutureDate<DateTime> { ValidationMessage = "The date must be today or later." });
            _eventUrl.Validations.Add(new IsUrlSchemeRule<string> { ValidationMessage = "Url must start with http: or https:" });
        }
    }
}
