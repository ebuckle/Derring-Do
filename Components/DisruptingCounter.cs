using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Controllers;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System;

namespace Derring_Do
{
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class DisruptingCounterTargetLogic : RuleTargetLogicComponent<RuleAttackWithWeapon>
    {
        public BlueprintBuff enemy_flag;

        public override void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
        }

        public override void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            MechanicsContext context = ElementsContext.GetData<MechanicsContext.Data>()?.Context;

            if (this.Owner.Body.PrimaryHand.MaybeWeapon != null &&  (this.Owner.Body.PrimaryHand.MaybeWeapon.Blueprint.Category == WeaponCategory.Dagger || this.Owner.Body.PrimaryHand.MaybeWeapon.Blueprint.Category == WeaponCategory.Starknife) && this.Owner.Unit.CombatState.IsEngage(evt.Initiator))
            {
                if (this.Owner.Resources.GetResourceAmount(Swashbuckler.panache_resource) > 0 && evt.Target.CombatState.AttackOfOpportunityCount > 0)
                {
                    this.Owner.Resources.Spend(Swashbuckler.panache_resource, 1);
                    evt.Initiator.Descriptor.AddBuff(enemy_flag, context, new TimeSpan?(1.Seconds()));
                    Game.Instance.CombatEngagementController.ForceAttackOfOpportunity(this.Owner.Unit, evt.Initiator);
                }
            }
        }
    }

    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class DisruptingCounterAOOLogic : RuleInitiatorLogicComponent<RuleAttackWithWeapon>
    {
        public BlueprintBuff enemy_flag;
        public BlueprintBuff debuff;

        public override void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
        }

        public override void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            MechanicsContext context = ElementsContext.GetData<MechanicsContext.Data>()?.Context;

            if (evt.IsAttackOfOpportunity && (evt.Weapon.Blueprint.Category == WeaponCategory.Dagger || evt.Weapon.Blueprint.Category == WeaponCategory.Starknife) && evt.Target.Descriptor.HasFact(enemy_flag) && evt.AttackRoll.IsHit)
            {
                evt.Target.Descriptor.AddBuff(debuff, context, new TimeSpan?(4.Seconds()));
            }
        }
    }
}
