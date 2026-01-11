#! /bin/bash
clear

readonly echo_default_color='\033[0m'
readonly echo_error_color='\033[0;31m'
readonly echo_success_color='\033[0;92m'
readonly echo_note_color='\033[0;37m'
readonly echo_info_color='\033[0;33m'
readonly echo_file_highlight_color='\033[1;4;97m'

echo "$(date)"

# Game Folder/s
readonly GAME_FOLDER='.local/share/Steam/steamapps/common/Tails of Iron'
readonly BEPIN_PLUGINS_FOLDER="$GAME_FOLDER/BepInEx/plugins"
readonly COM_OWOS02_MODBOX_FOLDER="$BEPIN_PLUGINS_FOLDER/com.owos02.toi_modbox"

# Dev Folder
readonly MOD_BIN_FOLDER='bin/Debug/netstandard2.0'

echo -e -n "${echo_note_color}Does folder $COM_OWOS02_MODBOX_FOLDER exist? "
if [ ! -d "$HOME/$COM_OWOS02_MODBOX_FOLDER" ]; then
  echo -e "${echo_info_color}NO\nGenerating ${echo_info_color}"
  mkdir "$HOME/$COM_OWOS02_MODBOX_FOLDER"
else
  echo -e "${echo_success_color}YES${echo_note_color}"
fi
echo "Copying"
echo -e "${echo_note_color}Target:      com.owos02.toi_modbox.dll"
echo "Destination: $HOME/$COM_OWOS02_MODBOX_FOLDER/"
  cp "$MOD_BIN_FOLDER/com.owos02.toi_modbox.dll" "$HOME/$COM_OWOS02_MODBOX_FOLDER"
echo -e "${echo_success_color}Done!${echo_note_color}"

echo "$(date)"

echo -e "\033[0;94mLaunching game!"
steam steam://rungameid/1283410

