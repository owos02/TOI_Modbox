using System;
using GearType = ItemData.GearType;

namespace com.owos02.toi_modbox;

internal partial class TOIPatches {
    private static GearType GetGearType(ImmediateModeGUI.ItemCategory toConvert) {
        return toConvert switch {
            ImmediateModeGUI.ItemCategory.Weapon1H => GearType.OneHandedMeleeWeapon,
            ImmediateModeGUI.ItemCategory.Weapon2H => GearType.TwoHandedMeleeWeapon,
            ImmediateModeGUI.ItemCategory.WeaponRanged => GearType.RangedWeapon,
            ImmediateModeGUI.ItemCategory.Helmet => GearType.Helmet,
            ImmediateModeGUI.ItemCategory.Armour => GearType.Armour,
            ImmediateModeGUI.ItemCategory.Shield => GearType.Shield,
            _ => throw new Exception("KeyItems is not in Enum 'GearType'")
        };
    }

    /// <summary>
    /// If the maps need to be changed, then this function is used for the translation of the map to the Scene.
    /// </summary>
    /// <param name="selectedMap"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    internal static LevelLoadManager.GameScenes mapToScene(MapManager.Maps selectedMap) {
        return selectedMap switch {
            MapManager.Maps.RatFort => LevelLoadManager.GameScenes.RatFort,
            MapManager.Maps.RatVillage => LevelLoadManager.GameScenes.RatVillage,
            MapManager.Maps.Moletown => LevelLoadManager.GameScenes.MoleTown,
            MapManager.Maps.FrogVillage => LevelLoadManager.GameScenes.FrogVillage,
            MapManager.Maps.BrightFirForest => LevelLoadManager.GameScenes.BrightFirForest,
            MapManager.Maps.Count => LevelLoadManager.GameScenes.Count,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    internal static QuestManager.MapEntrance CustomLocationToMapEntrance(RatVillageLocation selectedMap) {
        return selectedMap switch {
            RatVillageLocation.LongTailVillage => QuestManager.MapEntrance.CheckPoint01,
            RatVillageLocation.FarmBarn => QuestManager.MapEntrance.CheckPoint02,
            RatVillageLocation.TheRangersLodge => QuestManager.MapEntrance.Checkpoint03,
            RatVillageLocation.RatVillageTower => QuestManager.MapEntrance.Checkpoint04,
            RatVillageLocation.ElderHouseFloor2 => QuestManager.MapEntrance.CheckPoint05,
            RatVillageLocation.OldMillFarm => QuestManager.MapEntrance.Boss01,
            RatVillageLocation.TheRangersLodgeHut => QuestManager.MapEntrance.CheckPoint06,
            RatVillageLocation.MindersStoreRoom => QuestManager.MapEntrance.CheckPoint08,
            RatVillageLocation.LongTailVillageCenter => QuestManager.MapEntrance.CheckPoint09,
            RatVillageLocation.FarmersHouseBasement => QuestManager.MapEntrance.CheckPoint10,
            RatVillageLocation.MinersHouseBottom => QuestManager.MapEntrance.CheckPoint11,
            RatVillageLocation.UndergroundExplore02RENAMEME => QuestManager.MapEntrance.CheckPoint12,
            RatVillageLocation.RatVillageWildBench => QuestManager.MapEntrance.CheckPoint13,
            RatVillageLocation.MineEntrance => QuestManager.MapEntrance.CheckPoint14,
            RatVillageLocation.FarmCornfield => QuestManager.MapEntrance.CheckPoint15,
            RatVillageLocation.ElderHouseInventoryBox => QuestManager.MapEntrance.InventoryBox01,
            RatVillageLocation.FarmBarnInventoryBox => QuestManager.MapEntrance.InventoryBox02,
            RatVillageLocation.RatHouseInventoryBox => QuestManager.MapEntrance.InventoryBox03,
            RatVillageLocation.TowerInventoryBox => QuestManager.MapEntrance.InventoryBox04,
            RatVillageLocation.MinersStoreRoomInventoryBox => QuestManager.MapEntrance.InventoryBox05,
            RatVillageLocation.TowerInventoryBox02 => QuestManager.MapEntrance.InventoryBox06,
            RatVillageLocation.VillageSewersInventoryBox01 => QuestManager.MapEntrance.InventoryBox07,
            RatVillageLocation.VillageSewersInventoryBox02 => QuestManager.MapEntrance.InventoryBox08,
            RatVillageLocation.VillageSewers => QuestManager.MapEntrance.CheckPoint16,
            RatVillageLocation.VillageCenterQuestboard => QuestManager.MapEntrance.CheckPoint17,
            RatVillageLocation.VillageCenterBench => QuestManager.MapEntrance.CheckPoint18,
            RatVillageLocation.TowerOutsideBench => QuestManager.MapEntrance.CheckPoint19,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}