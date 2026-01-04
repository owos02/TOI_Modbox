using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace com.owos02.toi_modbox;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin {
    internal static new ManualLogSource Logger;
    internal static Harmony _harmony = new Harmony("TOI_Modbox");
    internal static Settings settings;
    internal static ImmediateModeGUI imgui = new ImmediateModeGUI();

    private void OnGUI() {
        imgui.Run(ref settings);
    }

    private void Awake() {
        // Plugin startup logic
        Logger = base.Logger;
#if DEBUG
        Logger.LogInfo($"Loading {MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION}");
        Logger.LogWarning($"You are running the debug version of the Plugin!");
#endif
#if RELEASE
            Logger.LogInfo($"Loading {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION}");
#endif
// ###
        settings = new Settings();
        _harmony.PatchAll(typeof(TOI_Patches));
// ###
#if DEBUG
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
#endif
#if RELEASE
            Logger.LogInfo( $"Plugin {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} is loaded!" );
#endif
    }
}

/// <summary>
/// This class is the container for all patches, and is being patched into the game.
/// Patches reside in the src/patches folder.
/// </summary>
internal partial class TOI_Patches {
    [HarmonyPatch(typeof(RedgiOptionsController), "ChangeMenuState")]
    [HarmonyPrefix]
    public static void GuiToggler(RedgiOptionsController.Menu menu) {
        Plugin.imgui.active = (menu == RedgiOptionsController.Menu.Settings);
    }
}