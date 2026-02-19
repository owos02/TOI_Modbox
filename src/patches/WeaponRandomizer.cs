using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace com.owos02.toi_modbox;

using HarmonyLib;

internal partial class TOIPatches {
    public static string[ ] itemsToBeRandomized = [];

    // ReSharper disable once InconsistentNaming
    [HarmonyPatch(typeof(SaveDataManager), "OnSaveFileLoadComplete")]
    [HarmonyPostfix]
    public static void Patch_RandomizeItems() {
        bool randomizeWeapons = true;
        if (randomizeWeapons) {
            Plugin.settings.random = new Random(int.Parse(Plugin.settings.randomSeed.Value));
            if (!randomizedDictionaryEntries.Any()) randomizedDictionaryEntries = RandomizeEquipableItems();
        }
    }

    [HarmonyPatch(typeof(PlayerInventoryAsset), "CollectBlueprint", new[ ] {typeof(ItemData.GearType), typeof(ItemData.OneHandedMeleeWeapons), typeof(ItemData.TwohandedMeleeWeapons), typeof(ItemData.RangedWeapons), typeof(ItemData.Shields), typeof(ItemData.Helmets), typeof(ItemData.Armour), typeof(bool)})]
    [HarmonyPrefix]
    public static void Patch_OverwritePickup(ref PlayerInventoryAsset __instance, ref ItemData.GearType gearType, ref ItemData.OneHandedMeleeWeapons oneHanded, ref ItemData.TwohandedMeleeWeapons twoHanded, ref ItemData.RangedWeapons ranged, ref ItemData.Shields shield, ref ItemData.Helmets helmet, ref ItemData.Armour armour) {
        Plugin.Logger.LogInfo("Changing Item");
        bool randomizeWeapons = true;
        if (!randomizeWeapons) return;
        int ID = 0;
        switch (gearType) {
            case ItemData.GearType.OneHandedMeleeWeapon:
                ID = (int)oneHanded;
                break;
            case ItemData.GearType.TwoHandedMeleeWeapon:
                ID = (int)twoHanded;
                break;
            case ItemData.GearType.RangedWeapon:
                ID = (int)ranged;
                break;
            case ItemData.GearType.Shield:
                ID = (int)shield;
                break;
            case ItemData.GearType.Helmet:
                ID = (int)helmet;
                break;
            case ItemData.GearType.Armour:
                ID = (int)armour;
                break;
        }

        GameItem toReplace = new(ID, gearType);
        var newItem = randomizedDictionaryEntries[toReplace];
        gearType = newItem.weaponType;
        oneHanded = (ItemData.OneHandedMeleeWeapons)newItem.ID;
        twoHanded = (ItemData.TwohandedMeleeWeapons)newItem.ID;
        ranged = (ItemData.RangedWeapons)newItem.ID;
        shield = (ItemData.Shields)newItem.ID;
        helmet = (ItemData.Helmets)newItem.ID;
        armour = (ItemData.Armour)newItem.ID;
    }

    public static string[ ] ShuffleList(string[ ] toRandomize) {
        
        int idx1 = toRandomize.Length;
        while (idx1 > 1) {
            idx1--;
            int idx2 = Plugin.settings.random.Next(idx1 + 1);
            (toRandomize[idx2], toRandomize[idx1]) = (toRandomize[idx1], toRandomize[idx2]);
        }

        return toRandomize;
    }

    public static Dictionary<GameItem, GameItem> ShuffleList(Dictionary<GameItem, GameItem> toRandomize) {
        
        var values = toRandomize.Values.ToList();
        foreach (var pair in toRandomize.Keys.ToList()) {
            int idx = Plugin.settings.random.Next(values.Count);
            toRandomize[pair] = values[idx];
            values.RemoveAt(idx);
        }

        return toRandomize;
    }

    public static Dictionary<GameItem, GameItem> RandomizeEquipableItems() {
        Plugin.Logger.LogInfo($"Randomizing with Seed: {Plugin.settings.randomSeed.Value}");
        var dict = new Dictionary<GameItem, GameItem>();
        var originalOHWeapon = Enum.GetNames(typeof(ItemData.OneHandedMeleeWeapons)).ToArray();
        var originalTHWeapon = Enum.GetNames(typeof(ItemData.TwohandedMeleeWeapons)).ToArray();
        var originalRangedWeapons = Enum.GetNames(typeof(ItemData.RangedWeapons)).ToArray();
        var originalShields = Enum.GetNames(typeof(ItemData.Shields)).ToArray();
        var originalArmours = Enum.GetNames(typeof(ItemData.Armour)).ToArray();
        var originalHelmets = Enum.GetNames(typeof(ItemData.Helmets)).ToArray();
        for (int i = 0; i < originalOHWeapon.Length; i++) {
            GameItem tmp = new(i, ItemData.GearType.OneHandedMeleeWeapon);
            dict.Add(tmp, new GameItem(tmp));
        }

        for (int i = 0; i < originalTHWeapon.Length; i++) {
            GameItem tmp = new(i, ItemData.GearType.TwoHandedMeleeWeapon);
            dict.Add(tmp, new GameItem(tmp));
        }

        for (int i = 0; i < originalRangedWeapons.Length; i++) {
            GameItem tmp = new(i, ItemData.GearType.RangedWeapon);
            dict.Add(tmp, new GameItem(tmp));
        }

        for (int i = 0; i < originalShields.Length; i++) {
            GameItem tmp = new(i, ItemData.GearType.Shield);
            dict.Add(tmp, new GameItem(tmp));
        }

        for (int i = 0; i < originalArmours.Length; i++) {
            GameItem tmp = new(i, ItemData.GearType.Armour);
            dict.Add(tmp, new GameItem(tmp));
        }

        for (int i = 0; i < originalHelmets.Length; i++) {
            GameItem tmp = new(i, ItemData.GearType.Helmet);
            dict.Add(tmp, new GameItem(tmp));
        }

        // All Categories shuffled, still problematic
        dict = ShuffleList(dict);
        // foreach (var VARIABLE in dict) {
        //     Plugin.Logger.LogInfo($"Searching for: {VARIABLE.Key} -> {VARIABLE.Value}");
        // }
        return dict;
    }
}

internal class GameItem : IEquatable<GameItem> {
    internal int ID;

    internal ItemData.GearType weaponType;
    // private int newID;
    // private Type newWeaponType;

    public GameItem(int id, ItemData.GearType weaponType) {
        ID = id;
        this.weaponType = weaponType;
    }

    public GameItem(GameItem old) {
        ID = old.ID;
        weaponType = old.weaponType;
    }

    public bool Equals(GameItem? other) {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return ID == other.ID && weaponType == other.weaponType;
    }

    public override bool Equals(object? obj) {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((GameItem)obj);
    }

    public override int GetHashCode() {
        unchecked {
            return(ID * 397) ^ (int)weaponType;
        }
    }
}