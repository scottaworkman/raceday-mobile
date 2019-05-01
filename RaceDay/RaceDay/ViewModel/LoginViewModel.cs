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
    public class LoginViewModel : ViewModelBase, INotifyPropertyChanged
    {
        ValidatableObject<string> _email;
        ValidatableObject<string> _password;
        string _errorMessage;
        bool _isValid;

        public LoginViewModel()
        {
            _email = new ValidatableObject<string>(string.Empty);
            _password = new ValidatableObject<string>(string.Empty);

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

        public ValidatableObject<string> Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
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
        public ICommand ValidatePasswordCommand => new Command(() => ValidatePassword());

        public bool Validate()
        {
            return ValidateEmail() && ValidatePassword();
        }

        bool ValidateEmail()
        {
            RaisePropertyChanged(() => Email);
            return _email.Validate();
        }

        bool ValidatePassword()
        {
            RaisePropertyChanged(() => Password);
            return _password.Validate();
        }

        private void AddValidations()
        {
            _email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email is required" });
            _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password is required" });
        }
    }
}
