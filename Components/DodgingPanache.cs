using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using static CallOfTheWild.Helpers;

namespace Derring_Do
{
    [ComponentName("Add Dodge Bonus to AC Against Attack")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    [AllowedOn(typeof(BlueprintBuff))]
    [AllowMultipleComponents]
    public class AddACBonusOnAttackAndConsumePanache : RuleTargetLogicComponent<RuleAttackWithWeapon>
    {
        private int cost = 1;
        private int bonus;
        private int will_spend = 0;
        public BlueprintAbilityResource resource;
        private ActionList ActionOnSelf = CreateActionList(Create<ContextActionRemoveSelf>());

        private int getResourceCost(RuleAttackWithWeapon evt)
        {
            // TODO - Cost reduction feats
            return cost > 0 ? cost : 0;
        }

        public override void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            Main.logger.Log("About to be attacked");
            if (!evt.Weapon.Blueprint.IsMelee)
            {
                Main.logger.Log("Attack was not with a melee weapon");
                return;
            }
            will_spend = 0;
            int need_resource = getResourceCost(evt);
            if (evt.Target.Descriptor.Resources.GetResourceAmount(resource) < need_resource)
            {
                Main.logger.Log("Not enough resource - had " + evt.Target.Descriptor.Resources.GetResourceAmount(resource) + " and needed " + need_resource);
                return;
            }

            will_spend = need_resource;

            bonus = Owner.Stats.Charisma.Bonus > 0 ? Owner.Stats.Charisma.Bonus : 0;
            Main.logger.Log("Adding bonus of " + bonus + " to AC");
            evt.AddTemporaryModifier(evt.Target.Stats.AC.AddModifier(bonus, this, ModifierDescriptor.Dodge));
        }

        public override void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            if (will_spend > 0)
            {
                Main.logger.Log("Spending " + will_spend + " panache");
                evt.Target.Descriptor.Resources.Spend(resource, will_spend);
            }
            IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
            if (factContextOwner != null)
            {
                factContextOwner.RunActionInContext(this.ActionOnSelf, evt.Initiator);
            }
            will_spend = 0;
        }
    }


    [ComponentName("Check Caster is Wearing Light or No Armor and is at Light Load")]
    [AllowedOn(typeof(BlueprintAbility))]
    public class AbilityCasterLightOrNoArmorCheck : BlueprintComponent, IAbilityCasterChecker
    {
        public bool CorrectCaster(UnitEntityData caster)
        {
            if (((!caster.Body.Armor.HasArmor || !caster.Body.Armor.Armor.Blueprint.IsArmor) || (caster.Body.Armor.Armor.Blueprint.ProficiencyGroup == ArmorProficiencyGroup.Light)) && caster.Descriptor.Encumbrance == Encumbrance.Light)
            {
                return true;
            }
            return false;
        }
        public string GetReason()
        {
            return "Require light or no armor and a light load";
        }
    }
}
