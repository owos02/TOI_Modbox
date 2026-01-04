using System.IO;
using BepInEx;
using BepInEx.Configuration;

namespace com.owos02.toi_modbox {
    internal class Settings {
        private static readonly string FilePath = Paths.ConfigPath + '\\' + MyPluginInfo.PLUGIN_GUID + ".cfg";

        private static ConfigFile _config;

        internal ConfigEntry<bool> configShowModBox;
        internal ConfigEntry<bool> configAlwaysShowModBox;
        internal ConfigEntry<bool> configSplashScreenSkip;
        internal ConfigEntry<bool> configInfiniteHealthFlask;
        internal ConfigEntry<bool> configInfiniteHealth;

        internal Settings() {
            var saveOnInit = false;
            Plugin.Logger.LogInfo($"Searching for config file at: {FilePath}");
            if (!File.Exists(FilePath)) {
                saveOnInit = true;
                Plugin.Logger.LogInfo($"Config not found. Generating settings");
            }
            else
                Plugin.Logger.LogInfo($"Config found");

            _config = new ConfigFile(Utility.CombinePaths(Paths.ConfigPath, MyPluginInfo.PLUGIN_GUID + ".cfg"), saveOnInit);
            BindEntries();
            if (!saveOnInit) _config.Reload();
        }

        internal void SaveConfig() {
            _config.Save();
        }

        private enum Categories {
            General,
            Player
        }
        
        private void BindEntries() {
            configShowModBox = _config.Bind(new ConfigDefinition(nameof(Categories.General), "ShowModBoxGui"), false, new ConfigDescription("Only for state saving"));
            configAlwaysShowModBox = _config.Bind(new ConfigDefinition(nameof(Categories.General), "AlwaysShowModBox"), false);
            configSplashScreenSkip = _config.Bind(new ConfigDefinition(nameof(Categories.General), "SkipSplashScreen"), true, new ConfigDescription("If enabled, skips to the controller screen"));
            configInfiniteHealth = _config.Bind(new ConfigDefinition(nameof(Categories.Player), "InfiniteHealth"), false);
            configInfiniteHealthFlask = _config.Bind(new ConfigDefinition(nameof(Categories.Player), "InfiniteHealthFlask"), false, new ConfigDescription("Inverts the health flask drain"));
        }
    }
}