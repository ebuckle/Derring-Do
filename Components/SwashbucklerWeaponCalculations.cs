using CallOfTheWild;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using static Derring_Do.Extensions;

namespace Derring_Do
{
    [ComponentName("Replace attack stat for swashbuckler weapon")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class AttackStatReplacementForSwashbucklerWeapon : RuleInitiatorLogicComponent<RuleCalculateAttackBonusWithoutTarget>
    {
        private StatType ReplacementStat = StatType.Dexterity;

        public override void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            ModifiableValueAttributeStat stat1 = this.Owner.Stats.GetStat(evt.AttackBonusStat) as ModifiableValueAttributeStat;
            ModifiableValueAttributeStat stat2 = this.Owner.Stats.GetStat(this.ReplacementStat) as ModifiableValueAttributeStat;
            bool flag = stat2 != null && stat1 != null && stat2.Bonus >= stat1.Bonus;

            if (isLightOrOneHandedPiercingWeapon(evt.Weapon.Blueprint, evt.Initiator.Descriptor))
            {
                evt.AttackBonusStat = this.ReplacementStat;
            }
        }

        public override void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
        }
    }

    [ComponentName("Add Improved Critical if owner is wielding a swashbuckler weapon")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class ImprovedCriticalOnWieldingSwashbucklerWeapon : RuleInitiatorLogicComponent<RuleCalculateWeaponStats>
    {
        public override void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if (evt.Weapon != null && isLightOrOneHandedPiercingWeapon(evt.Weapon.Blueprint, evt.Initiator.Descriptor))
            {
                evt.DoubleCriticalEdge = true;
            }
        }

        public override void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }
    }

    [ComponentName("Increase critical multiplier by one if owner is wielding a swashbuckler weapon")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class IncreasedCriticalMultiplierWithSwashbucklerWeapon : RuleInitiatorLogicComponent<RuleCalculateWeaponStats>
    {
        public override void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if (evt.Weapon != null && isLightOrOneHandedPiercingWeapon(evt.Weapon.Blueprint, evt.Initiator.Descriptor))
            {
                evt.AdditionalCriticalMultiplier = 1;
            }
        }

        public override void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }
    }

    [ComponentName("Crits with a swashbuckler weapon are autoconfirmed")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class CritAutoconfirmWithSwashbucklerWeapons : RuleInitiatorLogicComponent<RuleAttackRoll>
    {
        public override void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (isLightOrOneHandedPiercingWeapon(evt.Weapon.Blueprint, evt.Initiator.Descriptor))
            {
                evt.AutoCriticalConfirmation = true;
            }
        }
        public override void OnEventDidTrigger(RuleAttackRoll evt)
        {
        }
    }

    [Harmony12.HarmonyPatch(typeof(UnitPartWeaponTraining))]
    [Harmony12.HarmonyPatch("GetWeaponRank", Harmony12.MethodType.Normal)]
    [Harmony12.HarmonyPatch(new Type[] { typeof(ItemEntityWeapon) })]
    class Patch_UnitPartWeaponTraining_GetWeaponRank
    {
        static BlueprintFeature swashbuckler_weapon_training = Main.library.Get<BlueprintFeature>("ac1b0c88a06346b4a0fe35465a74daff");

        static public void Postfix(UnitPartWeaponTraining __instance, ItemEntityWeapon weapon, ref int __result)
        {
            if (weapon == null)
            {
                return;
            }

            if (!isLightOrOneHandedPiercingWeapon(weapon.Blueprint, __instance.Owner.Unit.Descriptor))
            {
                return;
            }

            var fact = __instance.Owner.GetFact(swashbuckler_weapon_training);

            if (fact == null)
            {
                return;
            }
            var rank = fact.GetRank();

            if (rank > __result)
            {
                __result = rank;
            }
        }
    }
}
