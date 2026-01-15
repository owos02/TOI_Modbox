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
        Settings.dataGold = string.Empty;
        Settings.dataIron = string.Empty;
        Settings.dataMonsterParts = string.Empty;
        Settings.saveFile = Settings.NO_SAVE_FILE_SELECTED;
        Plugin.imgui.CurrentMap = null;
        Plugin.imgui.CurrentLocation = null;
    }

    // ReSharper disable once InconsistentNaming
    [HarmonyPatch(typeof(SaveDataManager), "OnSaveFileLoadComplete")]
    [HarmonyPostfix]
    public static void Patch_GetSaveSlotData(SaveDataManager __instance) {
        Settings.saveFile = __instance.GetProfileSlot();
    }
}