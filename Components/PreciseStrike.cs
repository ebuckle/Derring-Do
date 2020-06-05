using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System.Linq;
using static CallOfTheWild.Helpers;
using static Derring_Do.Extensions;

namespace Derring_Do
{
    [ComponentName("Add bonus precision damage on swashbuckler weapons")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class AddBonusPrecisionDamageToSwashbucklerWeapons : RuleInitiatorLogicComponent<RuleAttackRoll>
    {
        public BlueprintAbilityResource resource;
        public BlueprintCharacterClass swashbuckler_class;
        private int need_resource = 1;
        public bool is_passive;

        public override void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (is_passive && evt.Initiator.Descriptor.Resources.GetResourceAmount(resource) < need_resource)
            {
                Main.logger.Log("Not enough resource - had " + evt.Target.Descriptor.Resources.GetResourceAmount(resource) + " and needed " + need_resource);
                return;
            }

            ItemEntityWeapon weapon = evt.Weapon;
            bool flag = isLightOrOneHandedPiercingWeapon(weapon.Blueprint, evt.Initiator.Descriptor);
            bool flag2 = base.Owner.Body.SecondaryHand.HasWeapon && base.Owner.Body.SecondaryHand.MaybeWeapon != base.Owner.Body.EmptyHandWeapon;
            bool flag3 = base.Owner.Body.SecondaryHand.HasShield;
            if (flag3)
            {
                ArmorProficiencyGroup proficiencyGroup = base.Owner.Body.SecondaryHand.MaybeShield.Blueprint.Type.ProficiencyGroup;
                flag3 = !(proficiencyGroup == ArmorProficiencyGroup.Buckler);
            }
            if (flag && !flag2 && !flag3)
            {
                evt.PreciseStrike += base.Owner.Progression.GetClassLevel(swashbuckler_class);
            }
        }

        public override void OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (is_passive)
            {
                return;
            }
            IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
            if (factContextOwner != null)
            {
                factContextOwner.RunActionInContext(CreateActionList(Create<ContextActionRemoveSelf>()), evt.Initiator);
            }
        }
    }

    [ComponentName("Check Target is not Immune to Precision Damage")]
    [AllowedOn(typeof(BlueprintComponent))]
    public class AbilityTargetNotImmuneToPrecision : BlueprintComponent, IAbilityTargetChecker
    {
        public bool CanTarget(UnitEntityData caster, TargetWrapper target)
        {
            UnitEntityData unit = target.Unit;
            if (unit == null)
            {
                return false;
            }


            if (target.Unit.Descriptor.Progression.Features.Enumerable.Any(f => f.Blueprint.GetComponent<AddImmunityToPrecisionDamage>() != null) || target.Unit.Descriptor.Buffs.Enumerable.Any(f => f.Blueprint.GetComponent<AddImmunityToPrecisionDamage>() != null))
            {
                return false;
            }

            return true;
        }
    }
}
