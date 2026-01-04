namespace com.owos02.toi_modbox;
using HarmonyLib;

internal partial class TOI_Patches {
    private static float originalMinHealth = 0f;

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
}