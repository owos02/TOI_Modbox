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
}