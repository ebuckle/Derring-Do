using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem.Rules;
using static Derring_Do.Extensions;
using static CallOfTheWild.Helpers;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace Derring_Do
{
    [ComponentName("Intimidate on hit if owner has panache")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class IndimidateOnHitWithSwashbucklerWeapon : RuleInitiatorLogicComponent<RuleAttackRoll>
    {
        public BlueprintAbilityResource resource;
        private int need_resource = 1;
        public GameAction demoralize_action;

        public override void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
        }
        public override void OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (evt.Initiator.Descriptor.Resources.GetResourceAmount(resource) < need_resource)
            {
                Main.logger.Log("Not enough resource - had " + evt.Target.Descriptor.Resources.GetResourceAmount(resource) + " and needed " + need_resource);
                return;
            }

            if (evt.Initiator.CombatState.Cooldown.SwiftAction != 0.0f)
            {
                Main.logger.Log("Swift action on Cooldown");
                return;
            }

            Main.logger.Log("Cooldown was " + evt.Initiator.CombatState.Cooldown.SwiftAction);

            if (!isLightOrOneHandedPiercingWeapon(evt.Weapon.Blueprint, evt.Initiator.Descriptor))
            {
                return;
            }

            if (!evt.IsHit)
            {
                return;
            }

            IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
            if (factContextOwner != null)
            {
                evt.Initiator.CombatState.Cooldown.SwiftAction = 6.0f;
                factContextOwner.RunActionInContext(CreateActionList(demoralize_action), evt.Target);
            }
        }
    }
}
