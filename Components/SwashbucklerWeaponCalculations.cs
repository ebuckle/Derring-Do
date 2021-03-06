﻿using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
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

            if (isSwashbucklerWeapon(evt.Weapon.Blueprint, evt.Initiator.Descriptor) && flag)
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
            if (evt.Weapon != null && isSwashbucklerWeapon(evt.Weapon.Blueprint, evt.Initiator.Descriptor))
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
            if (evt.Weapon != null && isSwashbucklerWeapon(evt.Weapon.Blueprint, evt.Initiator.Descriptor))
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
            if (isSwashbucklerWeapon(evt.Weapon.Blueprint, evt.Initiator.Descriptor))
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
        static BlueprintFeature swashbuckler_weapon_training = Swashbuckler.swashbuckler_weapon_training;
        static BlueprintFeature rapier_training = InspiredBlade.rapier_training;
        static BlueprintFeature flying_blade_training = FlyingBlade.flying_blade_training;

        static public void Postfix(UnitPartWeaponTraining __instance, ItemEntityWeapon weapon, ref int __result)
        {
            if (weapon == null)
            {
                return;
            }

            var fact = __instance.Owner.GetFact(swashbuckler_weapon_training);

            if (fact != null && isSwashbucklerWeapon(weapon.Blueprint, __instance.Owner.Unit.Descriptor))
            {
                var rank = fact.GetRank();

                if (rank > __result)
                {
                    __result = rank;
                }
            }

            fact = __instance.Owner.GetFact(rapier_training);

            if (fact != null && weapon.Blueprint.Category == WeaponCategory.Rapier)
            {
                var rank = fact.GetRank();

                if (rank > __result)
                {
                    __result = rank;
                }
            }

            fact = __instance.Owner.GetFact(flying_blade_training);

            if (fact != null && (weapon.Blueprint.Category == WeaponCategory.Dagger || weapon.Blueprint.Category == WeaponCategory.Starknife))
            {
                var rank = fact.GetRank();

                if (rank > __result)
                {
                    __result = rank;
                }
            }
        }
    }

    // Inspired Blade

    [ComponentName("Replace attack stat for rapier")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class AttackStatReplacementForRapier: RuleInitiatorLogicComponent<RuleCalculateAttackBonusWithoutTarget>
    {
        private StatType ReplacementStat = StatType.Dexterity;

        public override void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            ModifiableValueAttributeStat stat1 = this.Owner.Stats.GetStat(evt.AttackBonusStat) as ModifiableValueAttributeStat;
            ModifiableValueAttributeStat stat2 = this.Owner.Stats.GetStat(this.ReplacementStat) as ModifiableValueAttributeStat;
            bool flag = stat2 != null && stat1 != null && stat2.Bonus >= stat1.Bonus;

            if (evt.Weapon.Blueprint.Category == WeaponCategory.Rapier && flag)
            {
                evt.AttackBonusStat = this.ReplacementStat;
            }
        }

        public override void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
        }
    }

    [ComponentName("Add Improved Critical if owner is wielding a rapier")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class ImprovedCriticalOnWieldingRapier : RuleInitiatorLogicComponent<RuleCalculateWeaponStats>
    {
        public override void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if (evt.Weapon != null && evt.Weapon.Blueprint.Category == WeaponCategory.Rapier)
            {
                evt.DoubleCriticalEdge = true;
            }
        }

        public override void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }
    }

    [ComponentName("Increase critical multiplier and threat range by one if owner is wielding a rapier")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class IncreasedCriticalMultiplierAndThreatWithRapiers : RuleInitiatorLogicComponent<RuleCalculateWeaponStats>
    {
        public override void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if (evt.Weapon != null && evt.Weapon.Blueprint.Category == WeaponCategory.Rapier)
            {
                evt.AdditionalCriticalMultiplier = 1;
                evt.CriticalEdgeBonus = 1;
            }
        }

        public override void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }
    }

    [ComponentName("Crits with a rapier are autoconfirmed")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class CritAutoconfirmWithRapiers : RuleInitiatorLogicComponent<RuleAttackRoll>
    {
        public override void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (evt.Weapon.Blueprint.Category == WeaponCategory.Rapier)
            {
                evt.AutoCriticalConfirmation = true;
            }
        }
        public override void OnEventDidTrigger(RuleAttackRoll evt)
        {
        }
    }

    // Flying Blade

    [ComponentName("Add Improved Critical if owner is wielding a dagger or starknife")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class ImprovedCriticalOnFlyingBladeWeapons : RuleInitiatorLogicComponent<RuleCalculateWeaponStats>
    {
        public override void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if (evt.Weapon != null && (evt.Weapon.Blueprint.Category == WeaponCategory.Dagger || evt.Weapon.Blueprint.Category == WeaponCategory.Starknife))
            {
                evt.DoubleCriticalEdge = true;
            }
        }

        public override void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }
    }

    [Harmony12.HarmonyPatch(typeof(ItemEntityWeapon))]
    [Harmony12.HarmonyPatch("AttackRange", Harmony12.MethodType.Getter)]
    class Patch__ItemEntityWeapon__AttackRange
    {
        static BlueprintFeature flying_blade_training = FlyingBlade.flying_blade_training;
        static BlueprintFeature precise_throw_deed = FlyingBlade.precise_throw_deed;

        static public void Postfix(ItemEntityWeapon __instance, ref Feet __result)
        {
            if (__instance.Wielder.Unit.Descriptor.HasFact(flying_blade_training) && (__instance.Blueprint.Category == WeaponCategory.Dagger || __instance.Blueprint.Category == WeaponCategory.Starknife)) 
            {
                if (__instance.Blueprint.IsRanged)
                {
                    var fact = __instance.Wielder.Unit.Descriptor.GetFact(flying_blade_training);
                    if (fact != null && fact.GetRank() > 0)
                    {
                        var new_range = (fact.GetRank() * 5) + __result.Value;
                        __result = FeetExtension.Feet(new_range);
                    }
                    fact = __instance.Wielder.Unit.Descriptor.GetFact(precise_throw_deed);
                    if (fact != null && __instance.Wielder.Resources.GetResourceAmount(Swashbuckler.panache_resource) > 0)
                    {
                        var new_range = 5 + __result.Value;
                        __result = FeetExtension.Feet(new_range);
                    }
                }
            }
        }
    }

    [ComponentName("Crits with a dagger or starknife are autoconfirmed")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class CritAutoconfirmWithFlyingBladeWeapons : RuleInitiatorLogicComponent<RuleAttackRoll>
    {
        public override void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (evt.Weapon.Blueprint.Category == WeaponCategory.Dagger || evt.Weapon.Blueprint.Category == WeaponCategory.Starknife)
            {
                evt.AutoCriticalConfirmation = true;
            }
        }
        public override void OnEventDidTrigger(RuleAttackRoll evt)
        {
        }
    }

    [ComponentName("Increase critical multiplier by one if owner is wielding a dagger or starknife")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class IncreasedCriticalMultiplierWithFlyingBladeWeapons : RuleInitiatorLogicComponent<RuleCalculateWeaponStats>
    {
        public override void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if (evt.Weapon != null && (evt.Weapon.Blueprint.Category == WeaponCategory.Dagger || evt.Weapon.Blueprint.Category == WeaponCategory.Starknife))
            {
                evt.AdditionalCriticalMultiplier = 1;
            }
        }

        public override void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }
    }
}
