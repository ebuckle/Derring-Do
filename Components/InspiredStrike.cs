using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;
using static CallOfTheWild.Helpers;

namespace Derring_Do
{
    [AllowedOn(typeof(BlueprintUnitFact))]
    [ComponentName("Attack bonus for specific weapon types")]
    public class ContextWeaponCategoryAttackBonus : RuleInitiatorLogicComponent<RuleAttackWithWeapon>
    {
        public WeaponCategory[] categories;
        public ContextValue Value;
        public ModifierDescriptor descriptor;

        public override void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            int num = Value.Calculate(this.Fact.MaybeContext);

            if (categories.Contains(evt.Weapon.Blueprint.Category))
            {
                evt.AddTemporaryModifier(evt.Initiator.Stats.AdditionalAttackBonus.AddModifier(num, this, this.descriptor));
            }
        }

        public override void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
        }
    }

    [AllowedOn(typeof(BlueprintUnitFact))]
    [ComponentName("Auto critical threat with Inspired Strike")]
    public class InspiredStrikeLogic : RuleInitiatorLogicComponent<RuleAttackWithWeapon>
    {
        private int will_spend;

        public override void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            if (evt.Weapon.Blueprint.Category != WeaponCategory.Rapier)
            {
                return;
            }

            will_spend = 0;

            var bonus = evt.Initiator.Stats.Intelligence.Bonus < 1 ? 1 : evt.Initiator.Stats.Intelligence.Bonus;

            evt.AddTemporaryModifier(evt.Initiator.Stats.AdditionalAttackBonus.AddModifier(bonus, this, ModifierDescriptor.Insight));

            if (evt.Initiator.Descriptor.HasFact(InspiredBlade.inspired_strike_critical_buff) && evt.Initiator.Descriptor.Resources.GetResourceAmount(Swashbuckler.panache_resource) > 0)
            {
                will_spend = 1;
                evt.AutoCriticalThreat = true;
            }
        }

        public override void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            if (will_spend > 0 && evt.AttackRoll.IsHit)
            {
                evt.Initiator.Descriptor.Resources.Spend(Swashbuckler.panache_resource, will_spend);
            }

            will_spend = 0;

            IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
            if (factContextOwner != null)
            {
                factContextOwner.RunActionInContext(CreateActionList(Create<ContextActionRemoveSelf>()), evt.Initiator);
            }
        }
    }
}
