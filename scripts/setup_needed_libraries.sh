#! /bin/bash
clear

echo_default_color='\033[0m'
echo_error_color='\033[0;31m'
echo_success_color='\033[0;92m'
echo_note_color='\033[0;37m'
echo_question_color='\033[0;33m'
echo_file_highlight_color='\033[1;4;97m'

echo -e -n "${echo_note_color}Executed script in correct directory? "
current_dir=$(basename "$(pwd)" )
developer_directory_name="com.owos02.toi_modbox"

# Check if script is running in root
if [ "$current_dir" != "$developer_directory_name" ]; then
  echo -e "${echo_error_color}NO\nWrong directory"
  echo -e "${echo_question_color}Run the script in ${echo_file_highlight_color}ROOT directory${echo_question_color}. If it still fails continue reading!\n"
  echo -e "${echo_note_color}Script must be run in the root folder, and if you did\nthe directory name of the plugin was changed!\n"
  echo -e "${echo_note_color}To fix this go into the script and change the name in ${echo_file_highlight_color}line: 10\n"
  
  exit 1
fi
echo -e "${echo_success_color}YES${echo_note_color}"

# Game Folder/s
readonly GAME_FOLDER='.local/share/Steam/steamapps/common/Tails of Iron/TOI_Data/Managed'
readonly NEEDED_DLLS=("Assembly-CSharp.dll" "UnityEngine.dll" "UnityEngine.CoreModule.dll" "UnityEngine.IMGUIModule.dll" "UnityEngine.TextRenderingModule.dll" )
# Dev Folder
readonly PLUGIN_LIB_FOLDER='lib'

echo -n "Does folder $HOME/$GAME_FOLDER exist? "
if [ ! -d "$HOME/$GAME_FOLDER" ]; then
  echo "${echo_error_color}Directory not found! Generating!"
  echo "${echo_question_color}Is the search directory correct?${echo_file_highlight_color}"
  echo "$HOME/$GAME_FOLDER"
  exit 1
else
  echo -e "${echo_success_color}YES${echo_note_color}"
fi

echo -e "${echo_note_color}Copy start!$"
echo "  Source: $HOME/$GAME_FOLDER"
echo "  Target: "$(pwd)"/$PLUGIN_LIB_FOLDER"
for DLL in ${NEEDED_DLLS[@]}; do
  echo -e "${echo_note_color}Copying: ${echo_file_highlight_color}$DLL"
  cp "$HOME/$GAME_FOLDER/$DLL" "$(pwd)/$PLUGIN_LIB_FOLDER"
done
echo -e "${echo_note_color}Copy done!"
echo -e "${echo_success_color}Setup finished!\n"

