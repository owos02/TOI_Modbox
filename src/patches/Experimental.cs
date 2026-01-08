using HarmonyLib;

namespace com.owos02.toi_modbox;

internal partial class TOIPatches {
    // ReSharper disable once InconsistentNaming
    [HarmonyPatch(typeof(ItemData), "ManagedAwake")]
    [HarmonyPrefix]
    public static void Patch_Experimental_TestingGround(ItemData __instance) {
#if RELEASE
        return
#endif
    }
}