using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Validation;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace Derring_Do
{
    // Implementation thanks to spacehamster (https://github.com/spacehamster)
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class EvasiveLogic : OwnedGameLogicComponent<UnitDescriptor>, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ITargetRulebookHandler<RuleCheckTargetFlatFooted>, IRulebookHandler<RuleCheckTargetFlatFooted>, IResourceHandler,
        IInitiatorRulebookSubscriber, ITargetRulebookSubscriber, IGlobalSubscriber
    {
        public BlueprintAbilityResource Resource;
        private bool HasAddedDodge;

        public void OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            if (HasPanache(Owner) && evt.StatType == StatType.SaveReflex)
            {
                evt.Evasion = true;
            }
        }

        public void OnEventDidTrigger(RuleSavingThrow evt)
        {
        }

        public void OnEventAboutToTrigger(RuleCheckTargetFlatFooted evt)
        {
            if (evt.Target.Descriptor != Owner)
            {
                return;
            }
            if (!HasPanache(Owner))
            {
                return;
            }
            evt.IgnoreConcealment = true;
            evt.IgnoreVisibility = true;
        }

        public void OnEventDidTrigger(RuleCheckTargetFlatFooted evt)
        {
        }

        public void HandleResourceChanged(UnitDescriptor owner, BlueprintScriptableObject resourceBlueprint)
        {
            if (owner != Owner)
            {
                return;
            }

            var canAffordDeed = false;
            if (Resource == null)
            {
                Main.logger.Log("Panache resource is null");
            }
            else
            {
                canAffordDeed = HasPanache(owner);
            }
            if (HasAddedDodge && !canAffordDeed)
            {
                Owner.State.Features.CannotBeFlanked.Release();
                Owner.State.RemoveCondition(UnitCondition.AttackOfOpportunityBeforeInitiative);
                HasAddedDodge = false;
            }
            if (!HasAddedDodge && canAffordDeed)
            {
                Owner.State.AddCondition(UnitCondition.AttackOfOpportunityBeforeInitiative);
                Owner.State.Features.CannotBeFlanked.Retain();
                HasAddedDodge = true;
            }
        }

        public override void Validate(ValidationContext context)
        {
            if (Resource == null)
            {
                context.AddError("Resource is null");
            }
        }

        private bool HasPanache(UnitDescriptor unit)
        {
            return (unit.Resources.GetResourceAmount(Resource) > 0);
        }
    }
}
