namespace com.owos02.toi_modbox;

using HarmonyLib;

internal partial class TOI_Patches {
    [HarmonyPatch(typeof(SplashScreen), "ShowScreen")]
    [HarmonyPrefix]
    public static bool Patch_SkipSplashScreens(ref bool __result) {
        return __result = !Plugin.settings.configSplashScreenSkip.Value;
    }

    [HarmonyPatch(typeof(Player), "ManagedOnDestroy")]
    [HarmonyPrefix]
    public static void Patch_UnloadCurrenciesFromGUI(Player __instance) {
        Settings.dataGold = null;
        Settings.dataIron = null;
        Settings.dataMonsterParts = null;
    }
}