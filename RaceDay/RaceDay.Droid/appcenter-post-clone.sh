#!/usr/bin/env bash

sed "s/RACEDAY_GROUP_NAME/$RACEDAY_GROUP_NAME/g" -i $APPCENTER_SOURCE_DIRECTORY/RaceDay/RaceDay/Helpers/SettingsDefaults.cs
sed "s/RACEDAY_GROUP_CODE/$RACEDAY_GROUP_CODE/g" -i $APPCENTER_SOURCE_DIRECTORY/RaceDay/RaceDay/Helpers/SettingsDefaults.cs
sed "s/RACEDAY_FACEBOOK_GROUP/$RACEDAY_FACEBOOK_GROUP/g" -i $APPCENTER_SOURCE_DIRECTORY/RaceDay/RaceDay/Helpers/SettingsDefaults.cs
sed "s/RACEDAY_GROUP_API/$RACEDAY_GROUP_API/g" -i $APPCENTER_SOURCE_DIRECTORY/RaceDay/RaceDay/Helpers/SettingsDefaults.cs
sed "s/RACEDAY_FACEBOOK_APP/$RACEDAY_FACEBOOK_APP/g" -i $APPCENTER_SOURCE_DIRECTORY/RaceDay/RaceDay/Helpers/SettingsDefaults.cs

cat $APPCENTER_SOURCE_DIRECTORY/RaceDay/RaceDay/Helpers/SettingsDefaults.cs
