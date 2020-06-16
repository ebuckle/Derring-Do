using CallOfTheWild;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using static CallOfTheWild.Helpers;

namespace Derring_Do
{
    class FlyingBlade
    {
        static LibraryScriptableObject library = Main.library;

        static public BlueprintArchetype flying_blade;

        static public BlueprintFeature flying_panache;

        static public BlueprintFeature flying_blade_training;

        static public BlueprintFeature flying_blade_mastery;

        static public BlueprintFeature subtle_throw_deed;

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

            flying_blade.AddFeatures = new LevelEntry[] { Helpers.LevelEntry(1, flying_panache, subtle_throw_deed),
                                                          Helpers.LevelEntry(3, disrupting_counter_deed, precise_throw_deed),
                                                          Helpers.LevelEntry(5, flying_blade_training),
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
                                                 "",
                                                 "e0885d853c6748bd81dee057cd4d1feb",
                                                 null,
                                                 FeatureGroup.None
                                                 );
        }

        static void createSubtleThrow()
        {
            subtle_throw_deed = CreateFeature("SubtleThrowSwashbucklerFeature",
                                              "Subtle Throw",
                                              "",
                                              "4509f2bc6afe49eaac90d635adb4d2eb",
                                              null,
                                              FeatureGroup.None
                                              );
        }

        static void createDisruptingCounter()
        {
            disrupting_counter_deed = CreateFeature("DisruptingCounterSwashbucklerFeature",
                                  "Disrupting Counter",
                                  "",
                                  "7b0c9b9de8104d1b9b948969a9d1257b",
                                  null,
                                  FeatureGroup.None
                                  );
        }

        static void createPreciseThrow()
        {
            precise_throw_deed = CreateFeature("PreciseThrowSwashbucklerFeature",
                      "Precise Throw",
                      "",
                      "da8e9d51b0754316bf3db2bc58bcb89a",
                      null,
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
