using CallOfTheWild;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Validation;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.View;

namespace Derring_Do
{
    [Harmony12.HarmonyPatch(typeof(UnitEntityView))]
    [Harmony12.HarmonyPatch("LeaveProneState", Harmony12.MethodType.Normal)]
    class Patch_UnitEntityView_LeaveProneState
    {
        static BlueprintBuff kip_up_buff = Main.library.Get<BlueprintBuff>("10e9ae1dcaa64a8bb10d68df1873d52c");
        static BlueprintAbilityResource resource = Main.library.Get<BlueprintAbilityResource>("2087ab6ed0df4c8480379105bc0962a7");

        static public void Postfix(UnitEntityView __instance)
        {
            var unit = __instance.EntityData;

            if (unit.Descriptor.HasFact(kip_up_buff) && unit.Descriptor.Resources.GetResourceAmount(resource) > 0 && unit.CombatState.Cooldown.SwiftAction == 0.0f)
            {
                unit.CombatState.Cooldown.SwiftAction = 6.0f;
                unit.Descriptor.Resources.Spend(resource, 1);
                unit.CombatState.Cooldown.MoveAction = 0.0f;
            }
        }
    }

    [AllowedOn(typeof(BlueprintUnitFact))]
    public class KipUpLogic : OwnedGameLogicComponent<UnitDescriptor>, IResourceHandler, IInitiatorRulebookSubscriber, ITargetRulebookSubscriber, IGlobalSubscriber
    {
        public BlueprintAbilityResource Resource;
        private bool HasAddedKipUp;

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
            if (HasAddedKipUp && !canAffordDeed)
            {
                Owner.State.Features.GetUpWithoutAttackOfOpportunity.Release();
                HasAddedKipUp = false;
            }
            if (!HasAddedKipUp && canAffordDeed)
            {
                Owner.State.Features.GetUpWithoutAttackOfOpportunity.Retain();
                HasAddedKipUp = true;
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
