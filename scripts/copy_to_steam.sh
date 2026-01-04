#! /bin/bash
clear
now=$(date)
echo "Script Start: $now"

# Game Folder/s
readonly GAME_FOLDER='.local/share/Steam/steamapps/common/Tails of Iron'
readonly BEPIN_PLUGINS_FOLDER="$GAME_FOLDER/BepInEx/plugins"
readonly COM_OWOS02_MODBOX_FOLDER="$BEPIN_PLUGINS_FOLDER/com.owos02.toi_modbox"

# Dev Folder
readonly MOD_BIN_FOLDER='bin/Debug/netstandard2.0'

echo "Check existance of folder: $HOME/$COM_OWOS02_MODBOX_FOLDER"
if [ ! -d "$HOME/$COM_OWOS02_MODBOX_FOLDER" ]; then
  echo "Directory not found! Generating!"
  mkdir "$HOME/$COM_OWOS02_MODBOX_FOLDER"
else
  echo "Directory found!"
fi

echo "Copy: Start"
  cp -v "$MOD_BIN_FOLDER/com.owos02.toi_modbox.dll" "$HOME/$COM_OWOS02_MODBOX_FOLDER"
echo "Copy: Finished"
now=$(date)
echo "Script End: $now"

echo "Launching game"
steam steam://rungameid/1283410

