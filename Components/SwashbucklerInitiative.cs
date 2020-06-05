using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;

namespace Derring_Do
{
    // TODO - CONSIDER HARD CODING VALUES SINCE ONLY USED ONCE
    [ComponentName("Add Static Bonus On Initiative Check If ResourceAvailable")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    [AllowMultipleComponents]
    public class AddStaticBonusOnInitiativeCheckIfResourceAvailable : RuleInitiatorLogicComponent<RuleInitiativeRoll>
    {
        public BlueprintAbilityResource resource;
        public int amount;
        public int bonus;

        private int getResourceAmount(RuleInitiativeRoll evt)
        {
            // TODO - Cost reduction feats
            return amount > 0 ? amount : 0;
        }

        public override void OnEventAboutToTrigger(RuleInitiativeRoll evt)
        {
            Main.logger.Log("About to roll initiative");

            int need_resource = getResourceAmount(evt);

            if (evt.Initiator.Descriptor.Resources.GetResourceAmount(resource) < need_resource)
            {
                Main.logger.Log("Not enough resource - had " + evt.Initiator.Descriptor.Resources.GetResourceAmount(resource) + " and needed " + need_resource);
                return;
            }
            Main.logger.Log("Adding bonus of " + bonus + " to the roll.");
            evt.AddTemporaryModifier(Owner.Stats.Initiative.AddModifier(bonus, this, ModifierDescriptor.UntypedStackable));
        }

        public override void OnEventDidTrigger(RuleInitiativeRoll evt)
        {
        }
    }
}
