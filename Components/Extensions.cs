using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;

namespace Derring_Do
{
    public static partial class Extensions
    {
        public static bool isSwashbucklerWeapon(BlueprintItemWeapon weapon, UnitDescriptor wielder)
        {
            if (wielder.Progression.IsArchetype(InspiredBlade.inspired_blade) && weapon.Category == WeaponCategory.Rapier)
            {
                return true;
            }
            // Identical check for Duelist weapons
            if (weapon.Category.HasSubCategory(WeaponSubCategory.Light) || weapon.Category.HasSubCategory(WeaponSubCategory.OneHandedPiercing) || (wielder.State.Features.DuelingMastery && weapon.Category == WeaponCategory.DuelingSword) || wielder.Ensure<DamageGracePart>().HasEntry(weapon.Category))
            {
                return true;
            }
            return false;
        }
    }
}
