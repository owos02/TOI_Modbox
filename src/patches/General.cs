namespace com.owos02.toi_modbox;

using HarmonyLib;

internal partial class TOIPatches {
    // ReSharper disable once InconsistentNaming
    [HarmonyPatch(typeof(SplashScreen), "ShowScreen")]
    [HarmonyPrefix]
    public static bool Patch_SkipSplashScreens(ref bool __result) {
        return __result = !Plugin.settings.configSplashScreenSkip.Value;
    }

    // ReSharper disable once InconsistentNaming
    [HarmonyPatch(typeof(Player), "ManagedOnDestroy")]
    [HarmonyPrefix]
    public static void Patch_UnloadSaveDataFromGUI(Player __instance) {
        Settings.dataGold = null;
        Settings.dataIron = null;
        Settings.dataMonsterParts = null;
        Settings.saveFile = Settings.NO_SAVE_FILE_SELECTED;
    }

    // ReSharper disable once InconsistentNaming
    [HarmonyPatch(typeof(SaveDataManager), "OnSaveFileLoadComplete")]
    [HarmonyPostfix]
    public static void Patch_GetSaveSlotData(SaveDataManager __instance) {
        Settings.saveFile = __instance.GetProfileSlot();
    }
}