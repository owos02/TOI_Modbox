#! /bin/bash
clear

echo_default_color='\033[0m'
echo_error_color='\033[0;31m'
echo_success_color='\033[0;92m'
echo_note_color='\033[0;37m'
echo_info_color='\033[0;33m'
echo_file_highlight_color='\033[1;4;97m'

echo -e -n "${echo_note_color}Executed script in correct directory? "
WORKING_DIRECTORY=$(pwd)
CURRENT_FOLDER=$(basename "$WORKING_DIRECTORY" )
DEVELOPER_DIRECTORY_NAME="TOI_Modbox"

# Check if script is running in root
if [ "$CURRENT_FOLDER" != "$DEVELOPER_DIRECTORY_NAME" ]; then
  echo -e "${echo_error_color}NO\nWrong directory"
  echo -e "${echo_info_color}Run the script in ${echo_file_highlight_color}ROOT directory${echo_info_color}. If it still fails continue reading!\n"
  echo -e "${echo_note_color}Script must be run in the root folder, and if you did\nthe directory name of the plugin was changed!\n"
  echo -e "${echo_note_color}To fix this go into the script and change the name in ${echo_file_highlight_color}line: 10\n"
  
  exit 1
fi
echo -e "${echo_success_color}YES${echo_note_color}"

echo -e -n "${echo_note_color}Does the lib folder exist? "
if [ ! -d "$WORKING_DIRECTORY/lib" ]; then
  echo -e "${echo_info_color}No\nCreating folder ${echo_file_highlight_color}${WORKING_DIRECTORY}/lib${echo_note_color}"
  mkdir "$WORKING_DIRECTORY/lib"
else
  echo -e "${echo_info_color}YES${echo_note_color}"
fi
# Game Folder/s
readonly GAME_FOLDER='.local/share/Steam/steamapps/common/Tails of Iron/TOI_Data/Managed'
readonly NEEDED_DLLS=("Assembly-CSharp.dll" "UnityEngine.dll" "UnityEngine.CoreModule.dll" "UnityEngine.IMGUIModule.dll" "UnityEngine.TextRenderingModule.dll" )
# Dev Folder
readonly PLUGIN_LIB_FOLDER='lib'

echo -n "Does folder $HOME/$GAME_FOLDER exist? "
if [ ! -d "$HOME/$GAME_FOLDER" ]; then
  echo "${echo_error_color}Directory not found! Generating!"
  echo "${echo_info_color}Is the search directory correct?${echo_file_highlight_color}"
  echo "$HOME/$GAME_FOLDER"
  exit 1
else
  echo -e "${echo_success_color}YES${echo_note_color}"
fi

echo -e "${echo_note_color}Copy start!$"
echo "  Source: $HOME/$GAME_FOLDER"
echo "  Target: ${WORKING_DIRECTORY}/$PLUGIN_LIB_FOLDER"
for DLL in ${NEEDED_DLLS[@]}; do
  echo -e "${echo_note_color}Copying: ${echo_file_highlight_color}$DLL"
  cp "$HOME/$GAME_FOLDER/$DLL" "$WORKING_DIRECTORY/$PLUGIN_LIB_FOLDER"
done
echo -e "${echo_note_color}Copy done!"
echo -e "${echo_success_color}Setup finished!\n"

