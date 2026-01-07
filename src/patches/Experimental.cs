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
using Armour = ItemData.Armour;

internal partial class TOI_Patches {
    private static IEnumerable<GearType> gearTypes; 
    private static IEnumerable<KeyItems> keyItems; 
    private static IEnumerable<OHWeapon> weapons1H; 
    private static IEnumerable<THWeapon> weapons2H; 
    private static IEnumerable<RangedWeapons> weaponsRanged; 
    private static IEnumerable<Shields> shields; 
    private static IEnumerable<Helmets> helmets; 
    private static IEnumerable<Armour> armours; 
    
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
        armours = Enum.GetValues(typeof(Armour)).Cast<Armour>().Where((item) => !Regex.IsMatch(item.ToString(), dlcPattern));
        
        foreach (var d in gearTypes) {
            Plugin.Logger.LogInfo($"Gear: {d}");
        }
        
        foreach (var d in keyItems) {
            Plugin.Logger.LogInfo($"Key: {d}");
        }
        
        foreach (var d in weapons1H) {
            Plugin.Logger.LogInfo($"1H: {d}");
        }
        
        foreach (var d in weapons2H) {
            Plugin.Logger.LogInfo($"2H: {d}");
        }
        
        foreach (var d in weaponsRanged) {
            Plugin.Logger.LogInfo($"Ranged Weapon: {d}");
        }
        
        foreach (var d in shields) {
            Plugin.Logger.LogInfo($"Shields: {d}");
        }
        
        foreach (var d in helmets) {
            Plugin.Logger.LogInfo($"Helmets: {d}");
        }
        
        foreach (var d in armours) {
            Plugin.Logger.LogInfo($"Armour: {d}");
        }
    }

    
    [HarmonyPatch(typeof(Player), "ManagedOnEnable")]
    [HarmonyPostfix]
    public static void Patch_ExperimentalRecieveItem(Player __instance) {
        Plugin.Logger.LogInfo("Equipping Item now");
        __instance.m_PlayerInventoryAsset.Equip(OHWeapon.PurpleHeroSword);
    }
}