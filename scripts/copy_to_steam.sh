#! /bin/bash
clear
now=$(date)
echo "Start: $now"

echo "Copying files"
# Location of the BepInEx/plugins folder
readonly GAME_FOLDER='.local/share/Steam/steamapps/common/Tails of Iron'

readonly GAME_FOLDER_MANAGED="$GAME_FOLDER/TOI_Data/Managed"
readonly BEPIN_PLUGINS_FOLDER="$GAME_FOLDER/BepInEx/plugins"

# Locaiton of the building binaries folder from the mod
readonly MOD_BIN_FOLDER='bin/Debug/netstandard2.0'

cp -v "$MOD_BIN_FOLDER/com.owos02.toi_modbox.dll" "$HOME/$BEPIN_PLUGINS_FOLDER"
echo "Copying done!"
now=$(date)
echo "Finish: $now"

echo "Starting Game:"

steam steam://rungameid/1283410

