using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using System;

namespace Derring_Do
{
    // Implementation thanks to spacehamster (https://github.com/spacehamster)
    public interface IResourceHandler : IGlobalSubscriber
    {
        void HandleResourceChanged(UnitDescriptor owner, BlueprintScriptableObject resourceBlueprint);
    }
    [Harmony12.HarmonyPatch(typeof(UnitAbilityResourceCollection), "Restore",
        new Type[] { typeof(BlueprintScriptableObject), typeof(int), typeof(bool) })]
    static class UnitAbilityResourceCollection_Restore_Patch
    {
        static void Postfix(UnitAbilityResourceCollection __instance, UnitDescriptor ___m_Owner, BlueprintScriptableObject blueprint)
        {
            EventBus.RaiseEvent<IResourceHandler>(h => h.HandleResourceChanged(___m_Owner, blueprint));
        }
    }
    [Harmony12.HarmonyPatch(typeof(UnitAbilityResourceCollection), "Spend")]
    static class UnitAbilityResourceCollection_Spend_Patch
    {
        static void Postfix(UnitAbilityResourceCollection __instance, UnitDescriptor ___m_Owner, BlueprintScriptableObject blueprint)
        {
            EventBus.RaiseEvent<IResourceHandler>(h => h.HandleResourceChanged(___m_Owner, blueprint));
        }
    }
    [Harmony12.HarmonyPatch(typeof(UnitAbilityResourceCollection), "Add")]
    static class UnitAbilityResourceCollection_Add_Patch
    {
        static void Postfix(UnitAbilityResourceCollection __instance, UnitDescriptor ___m_Owner, BlueprintScriptableObject blueprint)
        {
            EventBus.RaiseEvent<IResourceHandler>(h => h.HandleResourceChanged(___m_Owner, blueprint));
        }
    }
    [Harmony12.HarmonyPatch(typeof(UnitAbilityResourceCollection), "Remove")]
    static class UnitAbilityResourceCollection_Remove_Patch
    {
        static void Postfix(UnitAbilityResourceCollection __instance, UnitDescriptor ___m_Owner, BlueprintScriptableObject blueprint)
        {
            EventBus.RaiseEvent<IResourceHandler>(h => h.HandleResourceChanged(___m_Owner, blueprint));
        }
    }
}
