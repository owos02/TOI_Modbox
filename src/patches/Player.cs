using System.Reflection;
using Steamworks;

namespace com.owos02.toi_modbox;

using HarmonyLib;

internal partial class TOI_Patches {
    [HarmonyPatch(typeof(Player), "DealerDamage")]
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

    private const int RangedWeaponPlayerEnum = 0;

    [HarmonyPatch(typeof(RangedWeapon), "DecrementAmmo")]
    [HarmonyPrefix]
    public static bool Patch_InfiniteAmmo(RangedWeapon __instance) {
        if (!Plugin.settings.configInfiniteAmmo.Value) return true;
        return Traverse.Create(__instance).Field("m_OwnerCharacterType").GetValue<int>() != RangedWeaponPlayerEnum;
    }
}