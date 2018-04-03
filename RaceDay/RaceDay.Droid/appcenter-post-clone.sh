#!/usr/bin/env bash

cat <<EOT >> $APPCENTER_SOURCE_DIRECTORY/RaceDay/RaceDay/Helpers/SettingsDefaults.cs
namespace RaceDay.Helpers
{
    public static class SettingsDefaults
    {
        public static readonly string GroupNameDefault = "$RACEDAY_GROUP_NAMEMultiFit";
        public static readonly string GroupCodeDefault = "$RACEDAY_GROUP_CODE";
        public static readonly string GroupFacebookIdDefault = "$RACEDAY_FACEBOOK_GROUP";
        public static readonly string GroupApiDefault = "$RACEDAY_GROUP_API";
        public static readonly string UserIdDefault = string.Empty;
        public static readonly string UserNameDefault = "Your Account";
        public static readonly string UserEmailDefault = string.Empty;
        public static readonly string FacebookAppIdDefault = "$RACEDAY_FACEBOOK_APP";
    }
}
EOT
