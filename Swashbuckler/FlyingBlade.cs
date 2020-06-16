using CallOfTheWild;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using static CallOfTheWild.Helpers;
using static Kingmaker.Designers.Mechanics.Facts.AttackTypeAttackBonus;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;

namespace Derring_Do
{
    class FlyingBlade
    {
        static LibraryScriptableObject library = Main.library;

        static public BlueprintArchetype flying_blade;

        static public BlueprintFeature flying_panache;

        static public BlueprintFeature flying_blade_training;

        static public BlueprintFeature flying_blade_mastery;

        static public BlueprintFeature subtle_throw_deed_first;
        static public BlueprintFeature subtle_throw_deed_sixth;

        static public BlueprintFeature disrupting_counter_deed;

        static public BlueprintFeature precise_throw_deed;

        static public BlueprintFeature targeted_throw_deed;

        static public BlueprintFeature bleeding_wound_deed;

        static public BlueprintFeature perfect_throw_deed;

        static public void create()
        {
            Main.DebugLog("Creating Flying Blade archetype");
            flying_blade = Helpers.Create<BlueprintArchetype>(a =>
            {
                a.name = "FlyingBladeArchetype";
                a.LocalizedName = Helpers.CreateString($"{a.name}.Name", "Flying Blade");
                a.LocalizedDescription = Helpers.CreateString($"{a.name}.Description", "While most swashbucklers prefer their battles up close, others prefer dealing death from a distance.");
            });
            SetField(flying_blade, "m_ParentClass", Swashbuckler.swashbuckler_class);
            library.AddAsset(flying_blade, "fbe20107f5f24a47aed0dc5836862758");

            flying_blade.ReplaceStartingEquipment = true;
            flying_blade.StartingItems = new BlueprintItem[]
            {
                library.Get<BlueprintItem>("afbe88d27a0eb544583e00fa78ffb2c7"), //StuddedStandard
                library.Get<BlueprintItem>("aa514dbf4c3d61f4e9c0738bd4d373cb"), //DaggerStandard
                library.Get<BlueprintItem>("aa514dbf4c3d61f4e9c0738bd4d373cb"), //DaggerStandard
                library.Get<BlueprintItem>("bc93a78d71bef084fa155e529660ed0d"), //PotionOfShieldOfFaith
                library.Get<BlueprintItem>("d52566ae8cbe8dc4dae977ef51c27d91"), //PotionOfCureLightWounds
            };

            //Create Features
            createFlyingPanache();
            createFlyingBladeTraining();
            createFlyingBladeMastery();
            createSubtleThrow();
            createDisruptingCounter();
            createPreciseThrow();
            createTargetedThrow();
            createBleedingWound();
            createPerfectThrow();

            flying_blade.RemoveFeatures = new LevelEntry[] { Helpers.LevelEntry(1, Swashbuckler.panache, Swashbuckler.dodging_panache_deed),
                                                             Helpers.LevelEntry(3, Swashbuckler.kip_up_deed, Swashbuckler.menacing_swordplay_deed),
                                                             Helpers.LevelEntry(5, Swashbuckler.swashbuckler_weapon_training),
                                                             Helpers.LevelEntry(7, Swashbuckler.targeted_strike_deed),
                                                             Helpers.LevelEntry(9, Swashbuckler.swashbuckler_weapon_training),
                                                             Helpers.LevelEntry(11, Swashbuckler.bleeding_wound_deed),
                                                             Helpers.LevelEntry(13, Swashbuckler.swashbuckler_weapon_training),
                                                             Helpers.LevelEntry(15, Swashbuckler.perfect_thrust_deed),
                                                             Helpers.LevelEntry(17, Swashbuckler.swashbuckler_weapon_training),
                                                             Helpers.LevelEntry(20, Swashbuckler.swashbuckler_weapon_mastery),
                                                           };

            flying_blade.AddFeatures = new LevelEntry[] { Helpers.LevelEntry(1, flying_panache, subtle_throw_deed_first),
                                                          Helpers.LevelEntry(3, disrupting_counter_deed, precise_throw_deed),
                                                          Helpers.LevelEntry(5, flying_blade_training),
                                                          Helpers.LevelEntry(6, subtle_throw_deed_sixth),
                                                          Helpers.LevelEntry(7, targeted_throw_deed),
                                                          Helpers.LevelEntry(9, flying_blade_training),
                                                          Helpers.LevelEntry(11, bleeding_wound_deed),
                                                          Helpers.LevelEntry(13, flying_blade_training),
                                                          Helpers.LevelEntry(15, perfect_throw_deed),
                                                          Helpers.LevelEntry(17, flying_blade_training),
                                                          Helpers.LevelEntry(20, flying_blade_mastery),
                                                        };

            Swashbuckler.swashbuckler_progression.UIDeterminatorsGroup = Swashbuckler.swashbuckler_progression.UIDeterminatorsGroup.AddToArray(flying_panache);
            Swashbuckler.swashbuckler_progression.UIGroups = Swashbuckler.swashbuckler_progression.UIGroups.AddToArray(CreateUIGroup(flying_blade_training, flying_blade_mastery));
            Swashbuckler.swashbuckler_progression.UIGroups = Swashbuckler.swashbuckler_progression.UIGroups.AddToArray(CreateUIGroup(subtle_throw_deed_first, subtle_throw_deed_sixth));
            Swashbuckler.swashbuckler_class.Archetypes = Swashbuckler.swashbuckler_class.Archetypes.AddToArray(flying_blade);
        }

        static void createFlyingPanache()
        {
            flying_panache = CreateFeature("FlyingPanacheFeature",
                                           "Flying Panache",
                                           "Unlike other swashbucklers, a flying blade regains panache only when she confirms a critical hit or makes a killing blow with a dagger or starknife.",
                                           "c04ca98650154007b8efc0dd8813f04f",
                                           null,
                                           FeatureGroup.None,
                                           Swashbuckler.panache_resource.CreateAddAbilityResource(),
                                           Create<RestorePanacheAttackRollTrigger>(a =>
                                           { a.CriticalHit = true; a.Action = CreateActionList(Swashbuckler.restore_panache); a.deadly_stab_buff = Swashbuckler.deadly_stab_buff; a.DuelistWeapon = false; a.CheckWeaponCategory = true; a.Category = WeaponCategory.Dagger; }),
                                           Create<RestorePanacheAttackRollTrigger>(a =>
                                           { a.CriticalHit = true; a.Action = CreateActionList(Swashbuckler.restore_panache); a.deadly_stab_buff = Swashbuckler.deadly_stab_buff; a.DuelistWeapon = false; a.CheckWeaponCategory = true; a.Category = WeaponCategory.Starknife; }),
                                           Create<RestorePanacheAttackRollTrigger>(a =>
                                           { a.ReduceHPToZero = true; a.Action = CreateActionList(Swashbuckler.restore_panache); a.DuelistWeapon = false; a.CheckWeaponCategory = true; a.Category = WeaponCategory.Dagger; }),
                                           Create<RestorePanacheAttackRollTrigger>(a =>
                                           { a.ReduceHPToZero = true; a.Action = CreateActionList(Swashbuckler.restore_panache); a.DuelistWeapon = false; a.CheckWeaponCategory = true; a.Category = WeaponCategory.Starknife; }),
                                           CreateAddFacts(Swashbuckler.panache_gain_base)
                                           );
        }

        static void createFlyingBladeTraining()
        {
            var improved_critical = library.Get<BlueprintParametrizedFeature>("f4201c85a991369408740c6888362e20");

            flying_blade_training = CreateFeature("FlyingBladeTrainingSwashbucklerFeature",
                                                  "Flying Blade Training",
                                                  "At 5th level, a flying blade gains a +1 bonus on attack and damage rolls when using daggers or starknives in combat. When a flying blade wields a dagger or starknife, she gains the benefit of the Improved Critical feat with those weapons. Additionally, a flying blade increases the range increment of a thrown dagger or starknife by 5 feet. The increase of range increment stacks with that of precise throw.\n"
                                                  + "Every 4 levels thereafter, the bonus on attack and damage rolls increases by 1, and the range increment increases by 5 feet.",
                                                  "48cb1a6729cf4c8c8f6a36ed20baa3b3",
                                                  null, //TODO new icon
                                                  FeatureGroup.None,
                                                  Create<WeaponTraining>(),
                                                  Create<WeaponTrainingBonuses>(w => { w.Stat = StatType.AdditionalAttackBonus; w.Descriptor = ModifierDescriptor.UntypedStackable; }),
                                                  Create<WeaponTrainingBonuses>(w => { w.Stat = StatType.AdditionalDamage; w.Descriptor = ModifierDescriptor.UntypedStackable; }),
                                                  Create<ImprovedCriticalOnFlyingBladeWeapons>()
                                                  );

            flying_blade_training.Ranks = 4;
            flying_blade_training.ReapplyOnLevelUp = true;
            flying_blade_training.AddComponent(CreateContextRankConfig(ContextRankBaseValueType.FeatureRank, feature: flying_blade_training));
            improved_critical.GetComponent<RecommendationNoFeatFromGroup>().Features = improved_critical.GetComponent<RecommendationNoFeatFromGroup>().Features.AddToArray(flying_blade_training);
        }

        static void createFlyingBladeMastery()
        {
            flying_blade_mastery = CreateFeature("FlyingBladeMasterySwashbucklerFeature",
                                                 "Flying Blade Mastery",
                                                 "At 20th level, when an attack that a flying blade makes with a dagger or starknife threatens a critical hit, that critical hit is automatically confirmed. Furthermore, the critical modifiers of daggers and starknives increase by 1 (×2 becomes ×3, and so on).",
                                                 "e0885d853c6748bd81dee057cd4d1feb",
                                                 null, //TODO icon
                                                 FeatureGroup.None,
                                                 Create<CritAutoconfirmWithFlyingBladeWeapons>(),
                                                 Create<IncreasedCriticalMultiplierWithFlyingBladeWeapons>()
                                                 );
        }

        static void createSubtleThrow()
        {
            var subtle_throw_buff_first = CreateBuff("SubtleThrowLevelOneSwashbucklerBuff",
                                                     "Subtle Throw",
                                                     "At 1st level, a flying blade can spend 1 panache point as part of a ranged attack with a dagger or starknife to make it without provoking attacks of opportunity.",
                                                     "09d754756df3471892a80485684a3929",
                                                     null, //TODO icon
                                                     null, //TODO fx
                                                     Create<AddInitiatorAttackWithWeaponTrigger>(a => { a.CheckWeaponCategory = true; a.Category = WeaponCategory.Dagger; a.CheckWeaponRangeType = true; a.RangeType = WeaponRangeType.Ranged; a.Action = CreateActionList(Create<SpendPanache>(s => { s.amount = 1; s.resource = Swashbuckler.panache_resource; })); a.ActionsOnInitiator = true; a.OnlyHit = false; }),
                                                     Create<AddInitiatorAttackWithWeaponTrigger>(a => { a.CheckWeaponCategory = true; a.Category = WeaponCategory.Starknife; a.CheckWeaponRangeType = true; a.RangeType = WeaponRangeType.Ranged; a.Action = CreateActionList(Create<SpendPanache>(s => { s.amount = 1; s.resource = Swashbuckler.panache_resource; })); a.ActionsOnInitiator = true; a.OnlyHit = false; }),
                                                     Create<PointBlankMaster>(p => p.Category = WeaponCategory.Dagger),
                                                     Create<PointBlankMaster>(p => p.Category = WeaponCategory.Starknife)
                                                     );

            var subtle_throw_buff_sixth = CreateBuff("SubtleThrowLevelSixSwashbucklerBuff",
                                                     "Subtle Throw",
                                                     "At 6th level, as a swift action she can spend 1 panache point to make all of her ranged attacks with daggers or starknives without provoking attacks of opportunity until the start of her next turn.",
                                                     "bc48843169584ae1941e90bb77a2983e",
                                                     null, //TODO icon
                                                     null, //TODO fx
                                                     Create<PointBlankMaster>(p => p.Category = WeaponCategory.Dagger),
                                                     Create<PointBlankMaster>(p => p.Category = WeaponCategory.Starknife)
                                                     );

            var apply_buff = Common.createContextActionApplyBuff(subtle_throw_buff_sixth, Helpers.CreateContextDuration(1), dispellable: false);

            var subtle_throw_ability_first = CreateActivatableAbility("SubtleThrowLevelOneSwashbucklerAbility",
                                                                      subtle_throw_buff_first.Name,
                                                                      subtle_throw_buff_first.Description,
                                                                      "0670250481f945949f3a97af84b08cd0",
                                                                      subtle_throw_buff_first.Icon,
                                                                      subtle_throw_buff_first,
                                                                      AbilityActivationType.Immediately,
                                                                      CommandType.Free,
                                                                      null,
                                                                      CallOfTheWild.Helpers.CreateActivatableResourceLogic(Swashbuckler.panache_resource, ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                                                                      );
            subtle_throw_ability_first.DeactivateImmediately = true;

            var subtle_throw_ability_sixth = CreateAbility("SubtleThrowLevelSixSwashbucklerAbility",
                                                           subtle_throw_buff_sixth.Name,
                                                           subtle_throw_buff_sixth.Description,
                                                           "5d8c41e4bc6c450e8d151a1d4509f06f",
                                                           subtle_throw_buff_sixth.Icon,
                                                           AbilityType.Extraordinary,
                                                           CommandType.Swift,
                                                           Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal,
                                                           "",
                                                           "",
                                                           CreateRunActions(new GameAction[] { apply_buff }),
                                                           Create<AbilityResourceLogic>(a => { a.IsSpendResource = true; a.Amount = 1; a.CostIsCustom = false; a.RequiredResource = Swashbuckler.panache_resource; })
                                                           );

            subtle_throw_deed_first = CreateFeature("SubtleThrowLevelOneSwashbucklerFeature",
                                                    "Subtle Throw",
                                                    subtle_throw_ability_first.Description + "\n"
                                                    + subtle_throw_ability_sixth.Description,
                                                    "4509f2bc6afe49eaac90d635adb4d2eb",
                                                    subtle_throw_ability_first.Icon, //TODO icon
                                                    FeatureGroup.None,
                                                    Helpers.CreateAddFact(subtle_throw_ability_first)
                                                    );

            subtle_throw_deed_sixth = CreateFeature("SubtleThrowLevelSixSwashbucklerFeature",
                                                    "Subtle Throw",
                                                    subtle_throw_deed_first.Description,
                                                    "9a664fd5d8b647c38357dcb7fc8acf5d",
                                                    subtle_throw_ability_sixth.Icon, //TODO icon
                                                    FeatureGroup.None,
                                                    Helpers.CreateAddFact(subtle_throw_ability_sixth)
                                                    );
        }

        static void createDisruptingCounter()
        {
            var disrupting_counter_enemy_flag = CreateBuff("DisruptingCounterEnemyFlag",
                                                           "",
                                                           "",
                                                           "9482251d85c9400d98a3b39b0e0e0ca8",
                                                           null,
                                                           null
                                                           );
            disrupting_counter_enemy_flag.SetBuffFlags(BuffFlags.HiddenInUi);

            var disrupting_counter_debuff = CreateBuff("DisruptingCounterEnemySwashbucklerBuff",
                                                       "Disrupting Counter",
                                                       "At 3rd level, when an opponent makes a melee attack against her, she can spend 1 panache point to make an attack of opportunity against the attacking foe. This attack of opportunity can be made with either a dagger or a starknife. If the attack hits, the opponent takes a –4 penalty on all attack rolls until the end of its turn.",
                                                       "356d8f82251448a9b7650b63800a6901",
                                                       null, //TODO icon
                                                       null,
                                                       Create<AddStatBonus>(a => { a.Stat = StatType.AdditionalAttackBonus; a.Descriptor = ModifierDescriptor.UntypedStackable; a.Value = -4; })
                                                       );

            var disrupting_counter_buff = CreateBuff("DisruptingCounterSwashbucklerBuff",
                                                     disrupting_counter_debuff.Name,
                                                     disrupting_counter_debuff.Description,
                                                     "d5c1f919613a464eaa74339e8c8140d4",
                                                     disrupting_counter_debuff.Icon, //TODO icon
                                                     null,
                                                     Create<DisruptingCounterTargetLogic>(d => d.enemy_flag = disrupting_counter_enemy_flag),
                                                     Create<DisruptingCounterAOOLogic>(d => { d.debuff = disrupting_counter_debuff; d.enemy_flag = disrupting_counter_enemy_flag; })
                                                     );

            var disrupting_counter_ability = CreateActivatableAbility("DisruptingCounterSwashbucklerAbility",
                                                                      disrupting_counter_debuff.Name,
                                                                      disrupting_counter_debuff.Description,
                                                                      "b389c906b0a844eba6bfedc2fb6eeeb7",
                                                                      disrupting_counter_debuff.Icon,
                                                                      disrupting_counter_buff,
                                                                      AbilityActivationType.Immediately,
                                                                      CommandType.Free,
                                                                      null,
                                                                      CallOfTheWild.Helpers.CreateActivatableResourceLogic(Swashbuckler.panache_resource, ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                                                                      );
            disrupting_counter_ability.DeactivateImmediately = true;

            disrupting_counter_deed = CreateFeature("DisruptingCounterSwashbucklerFeature",
                                                    disrupting_counter_buff.Name,
                                                    disrupting_counter_buff.Description,
                                                    "7b0c9b9de8104d1b9b948969a9d1257b",
                                                    null,
                                                    FeatureGroup.None,
                                                    Helpers.CreateAddFact(disrupting_counter_ability)
                                                    );
        }

        static void createPreciseThrow()
        {
            precise_throw_deed = CreateFeature("PreciseThrowSwashbucklerFeature",
                                               "Precise Throw",
                                               "At 3rd level, as long as she has at least 1 panache point, a flying blade can use her precise strike with a thrown dagger or starknife as long as the target is within 60 feet of her, and she increases the range increment of these weapons by 5 feet. She can spend 1 panache point when she throws a dagger or a starknife to ignore all range increment penalties with that ranged attack. This deed replaces menacing swordplay.",
                                               "da8e9d51b0754316bf3db2bc58bcb89a",
                                               null, //TODO icon
                                               FeatureGroup.None
                                               );
        }

        static void createTargetedThrow()
        {
            targeted_throw_deed = CreateFeature("TargetedThrowSwashbucklerFeature",
                                              "Targeted Throw",
                                              "",
                                              "e91c94a56c514b3eab13eaded01e1df0",
                                              null,
                                              FeatureGroup.None
                                              );
        }

        static void createBleedingWound()
        {
            bleeding_wound_deed = CreateFeature("BleedingWoundFlyingBladeSwashbucklerFeature",
                                              "Bleeding Wound",
                                              "",
                                              "a3726674829943fca3c97d519831b67f",
                                              null,
                                              FeatureGroup.None
                                              );
        }

        static void createPerfectThrow()
        {
            perfect_throw_deed = CreateFeature("PerfectThrowSwashbucklerFeature",
                                  "Perfect Throw",
                                  "",
                                  "d60d34a512304308ba15398d335ae737",
                                  null,
                                  FeatureGroup.None
                                  );
        }
    }
}
