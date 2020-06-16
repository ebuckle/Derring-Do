using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using static Derring_Do.Extensions;

namespace Derring_Do
{
    [ComponentName("Add bonus bleed damage on swashbuckler weapons")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class ApplySwashbucklerBleedOnHit : RuleInitiatorLogicComponent<RuleAttackRoll>
    {
        public BlueprintAbilityResource resource;
        public ActionList Action;
        public int need_resource;

        public override void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
        }

        public override void OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (evt.Initiator.Descriptor.Resources.GetResourceAmount(resource) < need_resource)
            {
                return;
            }

            var flying_blade_flag = (evt.Weapon.Blueprint.Category == WeaponCategory.Dagger || evt.Weapon.Blueprint.Category == WeaponCategory.Starknife) && evt.Initiator.Descriptor.HasFact(FlyingBlade.bleeding_wound_deed);

            if (!isSwashbucklerWeapon(evt.Weapon.Blueprint, evt.Initiator.Descriptor) && !flying_blade_flag)
            {
                return;
            }

            if (evt.ImmuneToSneakAttack)
            {
                return;
            }

            if (!evt.IsHit)
            {
                return;
            }

            evt.Initiator.Descriptor.Resources.Spend(resource, need_resource);

            IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
            if (factContextOwner != null)
            {
                factContextOwner.RunActionInContext(this.Action, evt.Target);
            }
        }
    }
}
