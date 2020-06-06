using Kingmaker.Blueprints;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;

namespace Derring_Do
{
    [ComponentName("Go to 1 HP when Incoming Damage Would Reduce to 0")]
    [AllowedOn(typeof(BlueprintBuff))]
    public class ReduceIncomingKillingBlow : RuleTargetLogicComponent<RuleDealDamage>
    {
        public BlueprintAbilityResource resource;

        public override void OnEventAboutToTrigger(RuleDealDamage evt)
        {
        }

        public override void OnEventDidTrigger(RuleDealDamage evt)
        {
            if (evt.Target.Damage < 0)
            {
                return;
            }

            if (evt.Target.Descriptor.Resources.GetResourceAmount(resource) < 1)
            {
                return;
            }

            int max_need_reduce = 1 - evt.Target.HPLeft;
            int reduce_damage = Math.Min(max_need_reduce, evt.Target.Damage);
            if (reduce_damage <= 0)
            {
                return;
            }

            var consume_amount = evt.Target.Descriptor.Resources.GetResourceAmount(resource);
            evt.Target.Descriptor.Resources.Spend(resource, consume_amount);

            evt.Target.Damage -= reduce_damage;
        }
    }
}
