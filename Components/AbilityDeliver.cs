using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Derring_Do
{
    [ComponentName("Delivery ability with a weapon hit")]
    [AllowedOn(typeof(BlueprintAbility))]
    public class AbilityDeliverHitWithMeleeWeapon : AbilityDeliverEffect
    {
        public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper target)
        {
            UnitEntityData caster = context.MaybeCaster;
            if (caster == null)
            {
                UberDebug.LogError("Caster is missing", Array.Empty<object>());
                yield break;
            }

            RulebookEventContext rulebookContext = context.RulebookContext;
            RuleAttackWithWeapon attackWithWeapon = (rulebookContext != null) ? rulebookContext.AllEvents.LastOfType<RuleAttackWithWeapon>() : null;
            RuleAttackWithWeapon ruleAttackWithWeapon = attackWithWeapon;
            RuleAttackRoll attackRoll = (ruleAttackWithWeapon != null) ? ruleAttackWithWeapon.AttackRoll : null;
            attackRoll = (attackRoll ?? context.TriggerRule<RuleAttackRoll>(new RuleAttackRoll(caster, target.Unit, caster.GetFirstWeapon(), 0)));
            if (attackWithWeapon == null)
            {
                attackRoll.ConsumeMirrorImageIfNecessary();
            }
            yield return new AbilityDeliveryTarget(target)
            {
                AttackRoll = attackRoll
            };
            yield break;
        }
    }

    public class AbilityDeliverAttackWithWeaponOnHit : AbilityDeliverEffect
    {
        public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper target)
        {
            if (target.Unit == null)
            {
                UberDebug.LogError("Target unit is missing", Array.Empty<object>());
                yield break;
            }
            UnitAttack cmd = new UnitAttack(target.Unit)
            {
                IsSingleAttack = true
            };
            cmd.Init(context.Caster);
            cmd.Start();
            AttackHandInfo attackHandInfo = cmd.AllAttacks.FirstOrDefault<AttackHandInfo>();
            ItemEntityWeapon weapon = (attackHandInfo != null) ? attackHandInfo.Weapon : null;
            if (weapon == null)
            {
                UberDebug.LogError("Has no weapon for attack", Array.Empty<object>());
                cmd.Interrupt();
                yield break;
            }
            bool hitHandled = false;
            bool isMelee = weapon.Blueprint.IsMelee;
            for (; ; )
            {
                if (cmd.IsFinished)
                {
                    RuleAttackWithWeapon lastAttackRule = cmd.LastAttackRule;
                    if (((lastAttackRule != null) ? lastAttackRule.Projectile : null) == null || cmd.LastAttackRule.Projectile.IsHit || cmd.LastAttackRule.Projectile.Cleared || cmd.LastAttackRule.Projectile.Destroyed)
                    {
                        break;
                    }
                }
                bool wasActed = cmd.IsActed;
                if (!cmd.IsFinished)
                {
                    cmd.Tick();
                }
                RuleAttackWithWeapon lastAttackRule2 = cmd.LastAttackRule;
                if (!wasActed && cmd.IsActed && isMelee)
                {
                    hitHandled = true;
                    if (lastAttackRule2.AttackRoll.IsHit)
                    {
                        yield return new AbilityDeliveryTarget(target);
                    }
                }
                yield return null;
            }
            if (!hitHandled && !isMelee)
            {
                RuleAttackWithWeapon lastAttackRule3 = cmd.LastAttackRule;
                bool? flag3 = (lastAttackRule3 != null) ? new bool?(lastAttackRule3.AttackRoll.IsHit) : null;
                if (flag3 != null && flag3.Value)
                {
                    yield return new AbilityDeliveryTarget(target);
                }
            }
            yield break;
        }
    }
}
