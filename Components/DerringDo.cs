using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System.Linq;

namespace Derring_Do
{
    [ComponentName("Add Exploding D6s on Derring-Do Skills and Spend Panache")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    [AllowedOn(typeof(BlueprintBuff))]
    [AllowMultipleComponents]
    public class AddExplodingD6sToDerringDoSkillChecks : RuleInitiatorLogicComponent<RuleSkillCheck>
    {
        public BlueprintAbilityResource resource;
        private int cost = 1;
        private int will_spend = 0;
        private StatType[] stats = { StatType.SkillMobility, StatType.SkillAthletics };

        private int getResourceCost(RuleSkillCheck evt)
        {
            // TODO - Cost reduction feats
            return cost > 0 ? cost : 0;
        }

        private int calculateExplodingDice(RuleSkillCheck evt)
        {
            int total = 0;
            DiceFormula dice_formula = new DiceFormula(1, DiceType.D6);
            RuleRollDice rule = new RuleRollDice(evt.Initiator, dice_formula);
            int roll = this.Fact.MaybeContext.TriggerRule<RuleRollDice>(rule).Result;
            total += roll;
            Main.logger.Log("Roll 1 was a " + roll);
            if (roll == 6)
            {
                Main.logger.Log("Exploding!");
                int attempts = Owner.Stats.Dexterity.Bonus > 0 ? Owner.Stats.Dexterity.Bonus : 1;
                for (int x = 0; x < attempts; x++)
                {
                    rule = new RuleRollDice(evt.Initiator, dice_formula);
                    roll = this.Fact.MaybeContext.TriggerRule<RuleRollDice>(rule).Result;
                    total += roll;
                    Main.logger.Log("Extra attempt " + x + " was a " + roll);
                    if (roll != 6)
                    {
                        break;
                    }
                }
            }
            return total;
        }

        public override void OnEventAboutToTrigger(RuleSkillCheck evt)
        {
            will_spend = 0;

            if (!stats.Contains(evt.StatType))
            {
                return;
            }

            int need_resource = getResourceCost(evt);

            if (evt.Initiator.Descriptor.Resources.GetResourceAmount(resource) < need_resource)
            {
                return;
            }

            will_spend = need_resource;

            int result = calculateExplodingDice(evt);
            Main.logger.Log("Adding bonus of " + result + " to check");
            evt.Bonus.AddModifier(result, this, ModifierDescriptor.UntypedStackable);
        }

        public override void OnEventDidTrigger(RuleSkillCheck evt)
        {
            if (will_spend > 0)
            {
                evt.Initiator.Descriptor.Resources.Spend(resource, will_spend);
            }
            will_spend = 0;
        }
    }
}
