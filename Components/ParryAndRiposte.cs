using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Combat;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using static CallOfTheWild.Helpers;

namespace Derring_Do
{
    [ComponentName("Swashbuckler Parry and Riposte")]
    [AllowedOn(typeof(BlueprintBuff))]
    public class SwashbucklerParryAndRiposte : OwnedGameLogicComponent<UnitDescriptor>, IGlobalRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, IGlobalRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (evt.Target.Descriptor.Resources.GetResourceAmount(resource) < cost)
            {
                return;
            }
            if (!evt.Weapon.Blueprint.IsMelee || evt.Parry != null || !base.Owner.Unit.IsEnemy(evt.Initiator))
            {
                return;
            }
            if (evt.Target != base.Owner.Unit)
            {
                return;
            }
            if (!base.Owner.Unit.IsReach(evt.Target, base.Owner.Body.PrimaryHand))
            {
                return;
            }
            //TODO - Conditions?
            /*
            if (this.AttackerCondition.HasConditions)
            {
                MechanicsContext maybeContext = base.Fact.MaybeContext;
                using ((maybeContext != null) ? maybeContext.GetDataScope(evt.Initiator) : null)
                {
                    if (!this.AttackerCondition.Check(null))
                    {
                        return;
                    }
                }
            }
            */
            evt.TryParry(base.Owner.Unit, base.Owner.Body.PrimaryHand.Weapon, 0);
            if (evt.Parry == null)
            {
                return;
            }
            ModifiableValue additionalAttackBonus = base.Owner.Stats.AdditionalAttackBonus;
            int num = evt.Initiator.Descriptor.State.Size - base.Owner.State.Size;
            if (num > 0)
            {
                int value = -2 * num;
                evt.AddTemporaryModifier(additionalAttackBonus.AddModifier(value, this, ModifierDescriptor.Penalty));
            }
        }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            RuleAttackRoll.ParryData parry = evt.Parry;
            if (((parry != null) ? parry.Initiator : null) != base.Owner.Unit)
            {
                return;
            }

            if (!evt.Parry.IsTriggered)
            {
                return;
            }

            evt.Target.Descriptor.Resources.Spend(resource, cost);

            if (evt.Result == AttackResult.Parried && evt.Target.Descriptor.Resources.GetResourceAmount(resource) >= cost)
            {
                Game.Instance.CombatEngagementController.ForceAttackOfOpportunity(base.Owner.Unit, evt.Initiator);
            }

            //base.Owner.RemoveFact(base.Fact); Unsure what this does in context of original parry

            IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
            if (factContextOwner != null)
            {
                factContextOwner.RunActionInContext(CreateActionList(Create<ContextActionRemoveSelf>()), evt.Initiator);
            }
        }

        //TODO Conditions
        public ConditionsChecker AttackerCondition;

        private enum TargetType
        {
            Self
        }

        public BlueprintAbilityResource resource;
        private int cost = 1;
    }
}
