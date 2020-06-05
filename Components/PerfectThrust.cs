using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using static CallOfTheWild.Helpers;

namespace Derring_Do
{
    [ComponentName("Resolve Attack against Touch AC")]
    [AllowedOn(typeof(BlueprintBuff))]
    public class AttackTargetsTouchAC : RuleInitiatorLogicComponent<RuleAttackRoll>
    {
        public override void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            evt.AttackType = AttackType.Touch;
        }

        public override void OnEventDidTrigger(RuleAttackRoll evt)
        {
        }
    }

    [ComponentName("Ignore all DR")]
    [AllowedOn(typeof(BlueprintBuff))]
    public class IgnoreAllDR : RuleInitiatorLogicComponent<RuleDealDamage>
    {
        public override void OnEventAboutToTrigger(RuleDealDamage evt)
        {
            foreach (BaseDamage baseDamage in evt.DamageBundle)
            {
                baseDamage.IgnoreReduction = true;
            }
        }

        public override void OnEventDidTrigger(RuleDealDamage evt)
        {
            IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
            if (factContextOwner != null)
            {
                factContextOwner.RunActionInContext(CreateActionList(Create<ContextActionRemoveSelf>()), evt.Initiator);
            }
        }
    }
}
