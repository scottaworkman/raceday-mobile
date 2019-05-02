#!/usr/bin/env bash

cat <<EOT >> $APPCENTER_SOURCE_DIRECTORY/RaceDay/RaceDay/Helpers/SettingsDefaults.cs
namespace RaceDay.Helpers
{
    public static class SettingsDefaults
    {
		public static readonly string ApiUrlDefault = "$RACEDAY_API_URL";
        public static readonly string GroupNameDefault = "$RACEAY_GROUP_NAME";
        public static readonly string GroupCodeDefault = "$RACEDAY_GROUP_CODE";
        public static readonly string GroupApiDefault = "$RACEDAY_GROUP_API";
        public static readonly string UserIdDefault = string.Empty;
        public static readonly string UserNameDefault = "Your Account";
        public static readonly string UserEmailDefault = string.Empty;
    }
}
EOT
