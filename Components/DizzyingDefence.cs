using CallOfTheWild;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Class.Kineticist.Properties;
using Kingmaker.Utility;

namespace Derring_Do
{
    [Harmony12.HarmonyPatch(typeof(FightingDefensivelyAttackPenaltyProperty))]
    [Harmony12.HarmonyPatch("GetInt", Harmony12.MethodType.Normal)]
    class Patch_FightingDefensivelyAttackPenaltyProperty_GetInt
    {
        static BlueprintBuff dazzling_defence_buff = Main.library.Get<BlueprintBuff>("9800e6e70ec84664b94b4cf1125e7c42");

        static public void Postfix(FightingDefensivelyAttackPenaltyProperty __instance, UnitEntityData unit, ref int __result)
        {
            Fact fact = unit.Descriptor.GetFact(dazzling_defence_buff);
            if (fact != null)
            {
                __result -= 2;
            }
        }
    }
}
