using RaceDay.Validation;
using RaceDay.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace RaceDay.ViewModel
{
    public class PasswordViewModel : ViewModelBase, INotifyPropertyChanged
    {
        ValidatableObject<string> _email;
        string _errorMessage;
        bool _isValid;

        public PasswordViewModel()
        {
            _email = new ValidatableObject<string>(string.Empty);

            AddValidations();
        }

        public ValidatableObject<string> Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
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
                RaisePropertyChanged(() => ShowError);
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                RaisePropertyChanged(() => ErrorMessage);
                RaisePropertyChanged(() => ShowError);
            }
        }

        public bool ShowError
        {
            get
            {
                return !IsValid && !string.IsNullOrEmpty(ErrorMessage);
            }
        }

        public ICommand ValidateEmailCommand => new Command(() => ValidateEmail());

        public bool Validate()
        {
            return ValidateEmail();
        }

        bool ValidateEmail()
        {
            RaisePropertyChanged(() => Email);
            return _email.Validate();
        }

        private void AddValidations()
        {
            _email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email is required" });
        }
    }
}
