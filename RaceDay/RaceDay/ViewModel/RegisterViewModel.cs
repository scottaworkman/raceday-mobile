using Microsoft.AppCenter.Analytics;
using RaceDay.Helpers;
using RaceDay.Services;
using RaceDay.Validation;
using RaceDay.View;
using RaceDay.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RaceDay.ViewModel
{
    public class RegisterViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public enum ModelType
        {
            Register,
            Profile,
            Password
        };
        ModelType modelType;

        ValidatableObject<string> _firstName;
        ValidatableObject<string> _lastName;
        ValidatableObject<string> _email;
        ValidatableObject<string> _password;
        ValidatableObject<string> _confirmPassword;
        ValidatableObject<string> _groupcode;
        string _errorMessage;
        bool _isValid;

        public Command RegisterAccountCommand { get; set; }
        public Command UpdateProfileCommand { get; set; }
        public Command UpdatePasswordCommand { get; set; }

        public RegisterViewModel(ModelType type = ModelType.Register)
        {
            modelType = type;

            _firstName = new ValidatableObject<string>(string.Empty);
            _lastName = new ValidatableObject<string>(string.Empty);
            _email = new ValidatableObject<string>(string.Empty);
            _password = new ValidatableObject<string>(string.Empty);
            _confirmPassword = new ValidatableObject<string>(string.Empty);
            _groupcode = new ValidatableObject<string>(string.Empty);

            AddValidations();

            RegisterAccountCommand = new Command<ContentPage>(
                async (page) => await RegisterAccount(page),
                (page) => !IsBusy);

            UpdateProfileCommand = new Command<ContentPage>(
                async (page) => await UpdateProfile(page),
                (page) => !IsBusy);

            UpdatePasswordCommand = new Command<ContentPage>(
                async (page) => await UpdatePassword(page),
                (page) => !IsBusy);
        }

        public ValidatableObject<string> FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                RaisePropertyChanged(() => FirstName);
            }
        }

        public ValidatableObject<string> LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                RaisePropertyChanged(() => LastName);
            }
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

        public ValidatableObject<string> ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                _confirmPassword = value;
                RaisePropertyChanged(() => ConfirmPassword);
            }
        }

        public ValidatableObject<string> GroupCode
        {
            get
            {
                return _groupcode;
            }
            set
            {
                _groupcode = value;
                RaisePropertyChanged(() => GroupCode);
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

        public ICommand ValidateFirstNameCommand => new Command(() => ValidateFirstName());
        public ICommand ValidateLastNameCommand => new Command(() => ValidateLastName());
        public ICommand ValidateEmailCommand => new Command(() => ValidateEmail());
        public ICommand ValidatePasswordCommand => new Command(() => ValidatePassword());
        public ICommand ValidateConfirmPasswordCommand => new Command(() => ValidateConfirmPassword());
        public ICommand ValidateGroupCodeCommand => new Command(() => ValidateGroupCode());

        public bool Validate()
        {
            if (modelType == ModelType.Profile)
            {
                return ValidateFirstName() & ValidateLastName() & ValidateEmail();
            }
            else if (modelType == ModelType.Password)
            {
                return ValidatePassword() & ValidateConfirmPassword();
            }
            else
            {
                return ValidateFirstName() & ValidateLastName() & ValidateEmail() & ValidatePassword() & ValidateConfirmPassword() & ValidateGroupCode();
            }
        }

        bool ValidateFirstName()
        {
            RaisePropertyChanged(() => FirstName);
            return _firstName.Validate();
        }

        bool ValidateLastName()
        {
            RaisePropertyChanged(() => LastName);
            return _lastName.Validate();
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

        bool ValidateConfirmPassword()
        {
            RaisePropertyChanged(() => ConfirmPassword);
            return _confirmPassword.Validate();
        }

        bool ValidateGroupCode()
        {
            RaisePropertyChanged(() => GroupCode);
            return _groupcode.Validate();
        }

        private void AddValidations()
        {
            if (modelType == ModelType.Profile || modelType == ModelType.Register)
            {
                _firstName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "First name is required" });
                _lastName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Last name is required" });
                _email.Validations.Add(new IsEmailRule<string> { ValidationMessage = "Invalid email", IsRequired = true });
            }
            if (modelType == ModelType.Password || modelType == ModelType.Register)
            {
                _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password is required" });
                _confirmPassword.Validations.Add(new IsComparisonRule<string> { ValidationMessage = "Passwords do not match", CompareValue = new PasswordCompareValue(this) });
            }
            if (modelType == ModelType.Register)
            {
                _groupcode.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Group Code is required" });
            }
        }

        /// <summary>
        /// Common Task handler for commands.  Wraps the logic in common checks and error handling
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        /// 
        private async Task ExecuteCommand(Func<Task> action, bool mainError = false)
        {
            if (IsBusy)
                return;

            Exception error = null;
            try
            {
                IsBusy = true;

                await action();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                error = ex;
            }
            finally
            {
                IsBusy = false;
            }

            if (error != null)
                await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
        }

        async Task RegisterAccount(ContentPage page)
        {
            await ExecuteCommand(async () =>
            {
                // Register Account using the API
                //
                var statusCode = await RaceDayV2Client.UserRegister( GroupCode.Value.Trim().ToUpper(),  FirstName.Value.Trim(),  LastName.Value.Trim(),  Email.Value.Trim(),  Password.Value.Trim());
                if (statusCode == HttpStatusCode.Created)
                {
                    var login = await RaceDayV2Client.Login( Email.Value.Trim(),  Password.Value.Trim());
                    if (login != null)
                    {
                        Settings.UserId = login.userid;
                        Settings.UserEmail = login.email;
                        Settings.UserPassword =  Password.Value.Trim();
                        Settings.UserFirstName = login.firstname;
                        Settings.UserLastName = login.lastname;
                        Settings.UserName = login.name;

                        Settings.Token = new Model.AccessToken
                        {
                            Token = login.token,
                            Expiration = login.expiration,
                            Role = login.role
                        };

                        Analytics.TrackEvent("Login",
                            new Dictionary<string, string>
                            {
                            { "UID", Settings.UserId },
                            { "Email", Settings.UserEmail }
                            });

                        await page.Navigation.PushAsync(new EventTabs(), false);
                    }
                    else
                    {
                         ErrorMessage = "Unable to login with email/password";
                         Email.IsValid = false;
                         Password.IsValid = false;
                    }
                }
                else if (statusCode == HttpStatusCode.BadRequest)
                {
                     ErrorMessage = "Invalid user information";
                     IsValid = false;
                }
                else if (statusCode == HttpStatusCode.Conflict)
                {
                     ErrorMessage = "User with same email already exists";
                     Email.IsValid = false;
                }
                else if (statusCode == HttpStatusCode.NotFound)
                {
                     ErrorMessage = "Invalid Group Code";
                     IsValid = false;
                }
                else
                {
                     ErrorMessage = "Unable to create account";
                     IsValid = false;
                }
            });
        }

        async Task UpdateProfile(ContentPage page)
        {
            await ExecuteCommand(async () =>
            {
                // Update Account using the API
                //
                var jsonUser = new Services.Models.JsonUser
                {
                    FirstName = FirstName.Value.Trim(),
                    LastName = LastName.Value.Trim(),
                    Name = $"{FirstName.Value.Trim()} {LastName.Value.Trim()}",
                    Email = Email.Value
                };
                var statusCode = await RaceDayV2Client.EditUser(Settings.UserEmail, jsonUser);

                if (statusCode == HttpStatusCode.OK)
                {
                    Settings.UserEmail = jsonUser.Email;
                    Settings.UserFirstName = jsonUser.FirstName;
                    Settings.UserLastName = jsonUser.LastName;
                    Settings.UserName = jsonUser.Name;

                    var snack = DependencyService.Get<ISnackbar>();
                    await snack.Show(new SnackbarOptions { Text = "Profile updated", Duration = SnackbarDuration.Short });
                    await Task.Delay(1500);

                    await page.Navigation.PopModalAsync();
                }
                else if (statusCode == HttpStatusCode.Conflict)
                {
                    ErrorMessage = "User with same email already exists";
                    Email.IsValid = false;
                }
                else
                {
                    ErrorMessage = "Invalid user information";
                    IsValid = false;
                }
            });
        }

        async Task UpdatePassword(ContentPage page)
        {
            await ExecuteCommand(async () =>
            {
                // Update Account using the API
                //
                var statusCode = await RaceDayV2Client.UpdatePassword(Settings.UserEmail, Password.Value.Trim());

                if (statusCode == HttpStatusCode.OK)
                {
                    Settings.UserPassword = Password.Value.Trim();

                    var snack = DependencyService.Get<ISnackbar>();
                    await snack.Show(new SnackbarOptions { Text = "Password updated", Duration = SnackbarDuration.Short });
                    await Task.Delay(1500);

                    await page.Navigation.PopModalAsync();
                }
                else
                {
                    ErrorMessage = "Invalid user information";
                    IsValid = false;
                }
            });
        }
    }

    public class PasswordCompareValue : IComparisonValue
    {
        RegisterViewModel _model = null;

        public PasswordCompareValue(RegisterViewModel vm)
        {
            _model = vm;
        }

        public string ValueToCompare()
        {
            return _model.Password.Value;
        }
    }
}
