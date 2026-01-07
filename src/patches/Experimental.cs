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

internal partial class TOI_Patches {
    internal static IEnumerable<KeyItems> keyItems;
    internal static IEnumerable<GearType> gearTypes;
    internal static IEnumerable<OHWeapon> weapons1H;
    internal static IEnumerable<THWeapon> weapons2H;
    internal static IEnumerable<RangedWeapons> weaponsRanged;
    internal static IEnumerable<Shields> shields;
    internal static IEnumerable<Helmets> helmets;
    internal static IEnumerable<Armours> armours;

    [HarmonyPatch(typeof(ItemData), "ManagedAwake")]
    [HarmonyPrefix]
    public static void Patch_Experimental_PopulateData(ItemData __instance) {
#if RELEASE
        return
#endif
        var dlcPattern = ".*DLC.*";
        gearTypes = Enum.GetValues(typeof(GearType)).Cast<GearType>();
        keyItems = Enum.GetValues(typeof(KeyItems)).Cast<KeyItems>();
        weapons1H = Enum.GetValues(typeof(OHWeapon)).Cast<OHWeapon>().Where((item) => !Regex.IsMatch(item.ToString(), dlcPattern));
        weapons2H = Enum.GetValues(typeof(THWeapon)).Cast<THWeapon>().Where((item) => !Regex.IsMatch(item.ToString(), dlcPattern));
        weaponsRanged = Enum.GetValues(typeof(RangedWeapons)).Cast<RangedWeapons>();
        shields = Enum.GetValues(typeof(Shields)).Cast<Shields>().Where((item) => !Regex.IsMatch(item.ToString(), dlcPattern));
        helmets = Enum.GetValues(typeof(Helmets)).Cast<Helmets>().Where((item) => !Regex.IsMatch(item.ToString(), dlcPattern));
        armours = Enum.GetValues(typeof(Armours)).Cast<Armours>().Where((item) => !Regex.IsMatch(item.ToString(), dlcPattern));
        Plugin.imgui.allItems = [keyItems.Select((x) => x.ToString()).ToArray(), weapons1H.Select((x) => x.ToString()).ToArray(), weapons2H.Select((x) => x.ToString()).ToArray(), weaponsRanged.Select((x) => x.ToString()).ToArray(), shields.Select((x) => x.ToString()).ToArray(), helmets.Select((x) => x.ToString()).ToArray(), armours.Select((x) => x.ToString()).ToArray()];
        Plugin.imgui.namesAreLoaded = true;
    }

    [HarmonyPatch(typeof(Player), "ManagedFixedUpdate")]
    [HarmonyPostfix]
    public static void Patch_ExperimentalRecieveItem(Player __instance) {
        if (!Plugin.imgui.addItemEvent) return;
        Plugin.imgui.addItemEvent = false;
        var category = (ImmediateModeGUI.SelectedCategory)Plugin.imgui.selectedCategory;
        if (category == ImmediateModeGUI.SelectedCategory.KeyItem) { }
        else {
            int index = Plugin.imgui.selectedItem;
            switch (category) {
                case ImmediateModeGUI.SelectedCategory.Weapon1H:
                    __instance.m_PlayerInventoryAsset.Equip(weapons1H.ElementAt(index));
                    break;
                case ImmediateModeGUI.SelectedCategory.Weapon2H:
                    __instance.m_PlayerInventoryAsset.Equip(weapons2H.ElementAt(index));
                    break;
                case ImmediateModeGUI.SelectedCategory.WeaponRanged:
                    __instance.m_PlayerInventoryAsset.Equip(weaponsRanged.ElementAt(index));
                    break;
                case ImmediateModeGUI.SelectedCategory.Shield:
                    __instance.m_PlayerInventoryAsset.Equip(shields.ElementAt(index));
                    break;
                case ImmediateModeGUI.SelectedCategory.Helmet:
                    __instance.m_PlayerInventoryAsset.Equip(helmets.ElementAt(index));
                    break;
                case ImmediateModeGUI.SelectedCategory.Armour:
                    __instance.m_PlayerInventoryAsset.Equip(armours.ElementAt(index));
                    break;
                default:
                    return;
            }
        }
    }
}