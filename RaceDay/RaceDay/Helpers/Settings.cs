// Helpers/Settings.cs
using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using RaceDay.Model;
using Xamarin.Forms;

namespace RaceDay.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        public enum ApplicationRole
        {
            NotInGroup = -1,
            Denied = 1,
            Member = 5,
            Admin = 10
        };

        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        // User information obtained from Facebook
        //
        private const string ApiUrlKey = "apiurl_key";

        private const string GroupNameKey = "groupname_key";

        private const string GroupCodeKey = "groupcode_key";

        private const string GroupApiKey = "groupapi_key";

        private const string UserIdKey = "userid_key";

        private const string UserNameKey = "username_key";

        private const string UserFirstNameKey = "userfirstname_key";

        private const string UserLastNameKey = "userlastname_key";

        private const string UserEmailKey = "useremail_key";

        private const string UserPasswordKey = "userpassword_key";

        private const string AppThemeKey = "apptheme_key";

        // Access Token authorizing REST client to the server API
        //
        private const string AccessTokenKey = "accesstoken_key";
        private static readonly string AccessTokenDefault = string.Empty;

        private const string AccessExpirationKey = "accessexpiration_key";
        private static readonly DateTime AccessExpirationDefault = DateTime.MinValue;

        private const string AccessRoleKey = "accessrole_key";
        private static readonly int AccessRoleDefault = (int)ApplicationRole.Member;

        // Application notification settings (future)
        //
        private const string NotifyNewRaceKey = "notifynewrace_key";
        private static readonly bool NotifyNewRaceDefault = false;

        private const string NotifyParticipantJoinsKey = "notifyparticipantjoins_key";
        private static readonly bool NotifyParticipantJoinsDefault = false;

        private const string HideInformationKey = "hideinformation_key";
        private static readonly bool HideInformationDefault = false;

        #endregion

        public static string APIUrl
        {
            get
            {
                return AppSettings.GetValueOrDefault(ApiUrlKey, SettingsDefaults.ApiUrlDefault);
            }
        }

        public static string GroupName
        {
            get
            {
                return AppSettings.GetValueOrDefault(GroupNameKey, SettingsDefaults.GroupNameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(GroupNameKey, value);
            }
        }

        public static string GroupCode
        {
            get
            {
                return AppSettings.GetValueOrDefault(GroupCodeKey, SettingsDefaults.GroupCodeDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(GroupCodeKey, value);
            }
        }

        public static string GroupApi
        {
            get
            {
                return AppSettings.GetValueOrDefault(GroupApiKey, SettingsDefaults.GroupApiDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(GroupApiKey, value);
            }
        }

        public static string UserId
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserIdKey, SettingsDefaults.UserIdDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserIdKey, value);
            }
        }

        public static string UserName
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserNameKey, SettingsDefaults.UserNameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserNameKey, value);
            }
        }

        public static string UserFirstName
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserFirstNameKey, String.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserFirstNameKey, value);
            }
        }

        public static string UserLastName
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserLastNameKey, String.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserLastNameKey, value);
            }
        }

        public static string UserEmail
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserEmailKey, SettingsDefaults.UserEmailDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserEmailKey, value);
            }
        }

        public static string UserPassword
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserPasswordKey, String.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserPasswordKey, value);
            }
        }

        public static OSAppTheme AppTheme
        {
            get
            {
                var defaultTheme = Application.Current.RequestedTheme == OSAppTheme.Light ? 0 : 1;
                var intTheme = AppSettings.GetValueOrDefault(AppThemeKey, defaultTheme);
                return (intTheme == 0 ? OSAppTheme.Light : intTheme == 1 ? OSAppTheme.Dark : OSAppTheme.Unspecified);
            }
            set
            {
                if (value == OSAppTheme.Light)
                    AppSettings.AddOrUpdateValue(AppThemeKey, 0);
                else if (value == OSAppTheme.Dark)
                    AppSettings.AddOrUpdateValue(AppThemeKey, 1);
                else
                    AppSettings.AddOrUpdateValue(AppThemeKey, 2);
            }
        }

        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(AccessTokenKey, AccessTokenDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AccessTokenKey, value);
            }
        }

        public static DateTime AccessExpiration
        {
            get
            {
                return AppSettings.GetValueOrDefault(AccessExpirationKey, AccessExpirationDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AccessExpirationKey, value);
            }
        }

        public static int AccessRole
        {
            get
            {
                return AppSettings.GetValueOrDefault(AccessRoleKey, AccessRoleDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AccessRoleKey, value);
            }
        }

        public static Boolean IsAuthenticated
        {
            get
            {
                return !string.IsNullOrEmpty(UserEmail) && !string.IsNullOrEmpty(UserPassword);
            }
        }

        public static bool NotifyNewRace
        {
            get
            {
                return AppSettings.GetValueOrDefault(NotifyNewRaceKey, NotifyNewRaceDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(NotifyNewRaceKey, value);
            }
        }

        public static bool NotifyParticipantJoins
        {
            get
            {
                return AppSettings.GetValueOrDefault(NotifyParticipantJoinsKey, NotifyParticipantJoinsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(NotifyParticipantJoinsKey, value);
            }
        }

        public static bool HideInformation
        {
            get
            {
                return AppSettings.GetValueOrDefault(HideInformationKey, HideInformationDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(HideInformationKey, value);
            }
        }

        // Retrieve stored Access Token, if available.  If no token stored or it has expired, then return null
        //
        public static AccessToken Token
        {
            get
            {
                AccessToken token = new AccessToken
                {
                    Token = AccessToken,
                    Expiration = AccessExpiration,
                    Role = AccessRole,
                };

                if (string.IsNullOrEmpty(token.Token) || (token.Expiration < DateTime.Now))
                    return null;

                return token;
            }
            set
            {
                if (value != null)
                {
                    AccessToken = value.Token;
                    AccessExpiration = value.Expiration;
                    AccessRole = value.Role;
                }
            }
        }
    }
}