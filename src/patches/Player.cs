using System;
using System.Linq;
using HarmonyLib;

namespace com.owos02.toi_modbox;

internal partial class TOIPatches {
    [HarmonyPatch(typeof(Player), "CharacterDamage")]
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

    [HarmonyPatch(typeof(RangedWeapon), "DecrementAmmo")]
    [HarmonyPrefix]
    public static bool Patch_InfiniteAmmo(RangedWeapon __instance) {
        if (!Plugin.settings.configInfiniteAmmo.Value) return true;
        var playerEnum = Traverse.Create(__instance).Field("m_OwnerCharacterType").GetValueType();
        var enumList = Enum.GetNames(playerEnum);
        if (!enumList.Contains("Player")) {
            Plugin.Logger.LogError($"Patch InfiniteAmmo: OwnerCharacterType contains {{{enumList.Join()}}} but has no \"Player\" Enum!");
            return true;
        }

        var targetedValue = Enum.Parse(playerEnum, "Player");
        return!Traverse.Create(__instance).Field("m_OwnerCharacterType").GetValue<object>().Equals(targetedValue);
    }

    [HarmonyPatch(typeof(Player), "ManagedUpdate")]
    [HarmonyPrefix]
    public static void Patch_SetCurrency(Player __instance) {
        Settings.dataGold = __instance.m_Inventory.m_CoinCurrency.ToString();
        Settings.dataIron = __instance.m_Inventory.m_IronCurrency.ToString();
        Settings.dataMonsterParts = __instance.m_Inventory.m_MonsterCurrency.ToString();
        if (Settings.incrementCoin == 0 && Settings.incrementIron == 0 && Settings.incrementMonster == 0) return;
        __instance.m_Inventory.m_CoinCurrency += Settings.incrementCoin;
        __instance.m_Inventory.m_IronCurrency += Settings.incrementIron;
        __instance.m_Inventory.m_MonsterCurrency += Settings.incrementMonster;
        Settings.incrementCoin = 0;
        Settings.incrementIron = 0;
        Settings.incrementMonster = 0;
    }
}