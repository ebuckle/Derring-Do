using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using static Derring_Do.Extensions;

namespace Derring_Do
{
    [ComponentName("Check Caster is Wielding a Swashbuckler Weapon")]
    [AllowedOn(typeof(BlueprintAbility))]
    [AllowedOn(typeof(BlueprintComponent))]
    public class AbilityCasterSwashbucklerWeaponCheck : BlueprintComponent, IAbilityCasterChecker
    {
        public bool CorrectCaster(UnitEntityData caster)
        {
            return (isLightOrOneHandedPiercingWeapon(caster.Body.PrimaryHand.Weapon.Blueprint, caster.Descriptor) || isLightOrOneHandedPiercingWeapon(caster.Body.SecondaryHand.Weapon.Blueprint, caster.Descriptor));
        }
        public string GetReason()
        {
            return "Require light or one-handed piercing weapon";
        }
    }

    [ComponentName("Check Caster has at least One Panache")]
    [AllowedOn(typeof(BlueprintAbility))]
    public class AbilityCasterHasAtLeastOnePanache : BlueprintComponent, IAbilityCasterChecker
    {
        public BlueprintAbilityResource resource;

        public bool CorrectCaster(UnitEntityData caster)
        {
            return (caster.Descriptor.Resources.GetResourceAmount(resource) > 0);
        }
        public string GetReason()
        {
            return "Require at least 1 panache point";
        }
    }
}
