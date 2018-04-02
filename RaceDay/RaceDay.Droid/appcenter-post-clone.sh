#!/usr/bin/env bash

sed -i "s/RACEDAY_GROUP_NAME/$RACEDAY_GROUP_NAME/g" $APPCENTER_SOURCE_DIRECTORY/RaceDay/RaceDay/Helpers/SettingsDefaults.cs
sed -i "s/RACEDAY_GROUP_CODE/$RACEDAY_GROUP_CODE/g" $APPCENTER_SOURCE_DIRECTORY/RaceDay/RaceDay/Helpers/SettingsDefaults.cs
sed -i "s/RACEDAY_FACEBOOK_GROUP/$RACEDAY_FACEBOOK_GROUP/g" $APPCENTER_SOURCE_DIRECTORY/RaceDay/RaceDay/Helpers/SettingsDefaults.cs
sed -i "s/RACEDAY_GROUP_API/$RACEDAY_GROUP_API/g" $APPCENTER_SOURCE_DIRECTORY/RaceDay/RaceDay/Helpers/SettingsDefaults.cs
sed -i "s/RACEDAY_FACEBOOK_APP/$RACEDAY_FACEBOOK_APP/g" $APPCENTER_SOURCE_DIRECTORY/RaceDay/RaceDay/Helpers/SettingsDefaults.cs

cat $APPCENTER_SOURCE_DIRECTORY/RaceDay/RaceDay/Helpers/SettingsDefaults.cs
