using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.ActivatableAbilities.Restrictions;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;

namespace Derring_Do
{
    [ComponentName("Check Caster is Prone")]
    [AllowedOn(typeof(BlueprintComponent))]
    public class AbilityCasterIsProne : BlueprintComponent, IAbilityCasterChecker
    {
        public bool CorrectCaster(UnitEntityData caster)
        {
            if (!caster.View.IsProne)
            {
                return false;
            }
            return true;
        }
        public string GetReason()
        {
            return "Must be prone";
        }
    }

    public class GetUpFromProne : ContextAction
    {
        public override string GetCaption()
        {
            return "Get up from prone";
        }

        public override void RunAction()
        {
            var owner = this.Context.MaybeOwner;
            if (owner == null)
            {
                return;
            };
            owner.Descriptor.State.Prone.ShouldBeActive = false;
            owner.Descriptor.State.Prone.Active = false;
            owner.Descriptor.State.RemoveCondition(UnitCondition.Prone);
            owner.Descriptor.State.Prone.Duration = TimeSpan.Zero;
            owner.View.LeaveProneState();
            owner.View.AnimationManager.StandUpImmediately();
        }
    }

    [AllowMultipleComponents]
    [ComponentName("AA restriction unit prone")]
    public class RestrictionHasUnitProne : ActivatableAbilityRestriction
    {
        public override bool IsAvailable()
        {
            return base.Owner.Unit.View.IsProne;
        }
    }
}
