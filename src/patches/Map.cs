using HarmonyLib;

namespace com.owos02.toi_modbox;

internal enum RatVillageLocation : int {
    LongTailVillage,
    FarmBarn,
    TheRangersLodge,
    RatVillageTower,
    ElderHouseFloor2,
    OldMillFarm,
    TheRangersLodgeHut,
    MindersStoreRoom,
    LongTailVillageCenter,
    FarmersHouseBasement,
    MinersHouseBottom,
    UndergroundExplore02RENAMEME,
    RatVillageWildBench,
    MineEntrance,
    FarmCornfield,
    ElderHouseInventoryBox,
    FarmBarnInventoryBox,
    RatHouseInventoryBox,
    TowerInventoryBox,
    MinersStoreRoomInventoryBox,
    TowerInventoryBox02,
    VillageSewersInventoryBox01,
    VillageSewersInventoryBox02,
    VillageSewers,
    VillageCenterQuestboard,
    VillageCenterBench,
    TowerOutsideBench
}

internal partial class TOIPatches {
    [HarmonyPatch(typeof(Player), "ManagedUpdate")]
    [HarmonyPrefix]
    public static void Patch_TeleportToLocation(Player __instance) {
        if (!Plugin.imgui.TeleportToLocationEvent) return;
        Plugin.imgui.TeleportToLocationEvent = false;
        if (Plugin.imgui.SelectedMap == 0) return;
        // From short testing, it seems like we can just always pass over RatVillage,
        // though I'll keep the proper translation commented out in case there is a need for it.
        // LevelLoadManager.Instance.LoadScene(mapToScene(Plugin.imgui.CurrentMap!.Value), (QuestManager.MapEntrance)Plugin.imgui.SelectedLocation);
        LevelLoadManager.Instance.LoadScene(LevelLoadManager.GameScenes.RatVillage, CustomLocationToMapEntrance((RatVillageLocation)Plugin.imgui.SelectedLocation));
    }

    // ReSharper disable once InconsistentNaming
    [HarmonyPatch(typeof(MapManager), "SetMapLocation")]
    [HarmonyPostfix]
    public static void Patch_GetCurrentLocation(MapManager.LevelSectionLocation mapLocation) {
        Plugin.imgui.CurrentLocation = mapLocation;
    }

    // ReSharper disable once InconsistentNaming
    [HarmonyPatch(typeof(SaveDataManager), "SetCurrentMap")]
    [HarmonyPostfix]
    public static void Patch_GetCurrentMap(MapManager.Maps map) {
        Plugin.imgui.CurrentMap = map;
        Plugin.imgui.SelectedMap = (int)map;
    }
}