using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace com.owos02.toi_modbox;

using GearType = ItemData.GearType;
using KeyItems = ItemData.KeyItems;
using OHWeapon = ItemData.OneHandedMeleeWeapons;
using THWeapon = ItemData.TwohandedMeleeWeapons;
using RangedWeapons = ItemData.RangedWeapons;
using Shields = ItemData.Shields;
using Helmets = ItemData.Helmets;
using Armours = ItemData.Armour;

internal partial class TOIPatches {
    private static IEnumerable<KeyItems>? keyItems;
    private static IEnumerable<GearType>? gearTypes;
    private static IEnumerable<OHWeapon>? weapons1H;
    private static IEnumerable<THWeapon>? weapons2H;
    private static IEnumerable<RangedWeapons>? weaponsRanged;
    private static IEnumerable<Shields>? shields;
    private static IEnumerable<Helmets>? helmets;
    private static IEnumerable<Armours>? armours;
    private static string[ ] CreateList<T>(IEnumerable<T> list) => list.Select(x => x!.ToString()).ToArray();

    // ReSharper disable once InconsistentNaming
    [HarmonyPatch(typeof(ItemData), "ManagedAwake")]
    [HarmonyPrefix]
    public static void Patch_Experimental_PopulateData(ItemData __instance) {
#if RELEASE
        return
#endif
        if (Plugin.imgui.AllItems != null) return;
        const string dlcPattern = ".*DLC.*";
        gearTypes = Enum.GetValues(typeof(GearType)).Cast<GearType>();
        keyItems = Enum.GetValues(typeof(KeyItems)).Cast<KeyItems>();
        weapons1H = Enum.GetValues(typeof(OHWeapon)).Cast<OHWeapon>().Where((item) => !Regex.IsMatch(item.ToString(), dlcPattern));
        weapons2H = Enum.GetValues(typeof(THWeapon)).Cast<THWeapon>().Where((item) => !Regex.IsMatch(item.ToString(), dlcPattern));
        weaponsRanged = Enum.GetValues(typeof(RangedWeapons)).Cast<RangedWeapons>();
        shields = Enum.GetValues(typeof(Shields)).Cast<Shields>().Where((item) => !Regex.IsMatch(item.ToString(), dlcPattern));
        helmets = Enum.GetValues(typeof(Helmets)).Cast<Helmets>().Where((item) => !Regex.IsMatch(item.ToString(), dlcPattern));
        armours = Enum.GetValues(typeof(Armours)).Cast<Armours>().Where((item) => !Regex.IsMatch(item.ToString(), dlcPattern));
        Plugin.imgui.AllItems = new Dictionary<string, string[ ]> {
            {nameof(ImmediateModeGUI.ItemCategory.Weapon1H), CreateList(weapons1H)},
            {nameof(ImmediateModeGUI.ItemCategory.Weapon2H), CreateList(weapons2H)},
            {nameof(ImmediateModeGUI.ItemCategory.WeaponRanged), CreateList(weaponsRanged)},
            {nameof(ImmediateModeGUI.ItemCategory.Shield), CreateList(shields)},
            {nameof(ImmediateModeGUI.ItemCategory.Helmet), CreateList(helmets)},
            {nameof(ImmediateModeGUI.ItemCategory.Armour), CreateList(armours)},
            {nameof(ImmediateModeGUI.ItemCategory.KeyItem), CreateList(keyItems)}
        };
        Plugin.imgui.NamesAreLoaded = true;
    }

    // ReSharper disable once InconsistentNaming
    [HarmonyPatch(typeof(Player), "ManagedFixedUpdate")]
    [HarmonyPostfix]
    public static void Patch_ExperimentalReceiveItem(Player __instance) {
        if (!Plugin.imgui.AddItemEvent) return;
        
        Plugin.imgui.AddItemEvent = false;
        var category = (ImmediateModeGUI.ItemCategory)Plugin.imgui.SelectedCategory;
        if (category == ImmediateModeGUI.ItemCategory.KeyItem) { }
        else {
            var index = Plugin.imgui.SelectedItem;
            Plugin.itemEquipGearEvent!.m_GearType = GetGearType(category);
            Plugin.itemEquipGearEvent.m_OneHandedMeleeWeapon = (OHWeapon)index;
            Plugin.itemEquipGearEvent.m_TwoHandedMeleeWeapon = (THWeapon)index;
            Plugin.itemEquipGearEvent.m_RangedWeapon = (RangedWeapons)index;
            Plugin.itemEquipGearEvent.m_Helmet = (Helmets)index;
            Plugin.itemEquipGearEvent.m_Armour = (Armours)index;
            Plugin.itemEquipGearEvent.m_Shield = (Shields)index;
            __instance.m_PlayerInventoryAsset.Equip(Plugin.itemEquipGearEvent);
        }
    }

    #region HelperFunctions

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
    #endregion
    
}