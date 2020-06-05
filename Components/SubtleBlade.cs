using Kingmaker.Blueprints;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using static Derring_Do.Extensions;

namespace Derring_Do
{
    [ComponentName("Immune to Disarm against Swashbuckler Weapons")]
    public class SwashbucklerWeaponDisarmImmune : RuleTargetLogicComponent<RuleCombatManeuver>
    {
        public BlueprintAbilityResource resource;
        private int amount = 1;

        public override void OnEventAboutToTrigger(RuleCombatManeuver evt)
        {
            if (!(evt.Type == CombatManeuver.Disarm))
            {
                return;
            }

            if (evt.Target.Descriptor.Resources.GetResourceAmount(resource) < amount)
            {
                return;
            }

            ItemEntityWeapon maybeWeapon = evt.Target.Body.PrimaryHand.MaybeWeapon;
            ItemEntityWeapon maybeWeapon2 = evt.Target.Body.SecondaryHand.MaybeWeapon;
            if ((maybeWeapon != null && !maybeWeapon.Blueprint.IsUnarmed && !maybeWeapon.Blueprint.IsNatural) && isLightOrOneHandedPiercingWeapon(maybeWeapon.Blueprint, evt.Target.Descriptor))
            {
                evt.AutoFailure = true;
            }
            else if ((maybeWeapon2 != null && !maybeWeapon2.Blueprint.IsUnarmed && !maybeWeapon2.Blueprint.IsNatural) && isLightOrOneHandedPiercingWeapon(maybeWeapon.Blueprint, evt.Target.Descriptor))
            {
                evt.AutoFailure = true;
            }
        }

        public override void OnEventDidTrigger(RuleCombatManeuver evt)
        {
        }
    }
}
