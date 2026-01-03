using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace com.owos02.toi_modbox {
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

    class TOI_Patches {
        [HarmonyPatch(typeof(RedgiOptionsController), "ChangeMenuState")]
        [HarmonyPrefix]
        public static void GuiToggler(RedgiOptionsController.Menu menu) {
            Plugin.imgui.active = (menu == RedgiOptionsController.Menu.Settings);
        }

        private static float originalMinHealth = 0f;

        [HarmonyPatch(typeof(Player), "DealerDamage")]
        [HarmonyPrefix]
        public static void Patch_InfiniteHealth(ref float Damage) {
            if (!Plugin.settings.configInfiniteHealth.Value) return;
            Damage = 0f;
        }

        [HarmonyPatch(typeof(Inventory), "DecrementHealthFlaskAmount")]
        [HarmonyPrefix]
        public static void Patch_InfiniteHealthFlask(Inventory __instance, ref float value) {
            if (!Plugin.settings.configInfiniteHealthFlask.Value) return;
            value = 0;
        }

        [HarmonyPatch(typeof(SplashScreen), "ShowScreen")]
        [HarmonyPrefix]
        public static bool Patch_SkipSplashScreens(ref bool __result) {
            return __result = !Plugin.settings.configSplashScreenSkip.Value;
        }
    }
}