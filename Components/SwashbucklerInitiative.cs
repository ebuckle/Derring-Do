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
            int need_resource = getResourceAmount(evt);

            if (evt.Initiator.Descriptor.Resources.GetResourceAmount(resource) < need_resource)
            {
                return;
            }
            evt.AddTemporaryModifier(Owner.Stats.Initiative.AddModifier(bonus, this, ModifierDescriptor.UntypedStackable));
        }

        public override void OnEventDidTrigger(RuleInitiativeRoll evt)
        {
        }
    }
}
