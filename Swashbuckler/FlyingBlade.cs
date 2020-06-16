using CallOfTheWild;
using CallOfTheWild.NewMechanics;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.View.Animation;
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
            var dagger = library.Get<BlueprintWeaponType>("07cc1a7fceaee5b42b3e43da960fe76d");

            flying_blade_training = CreateFeature("FlyingBladeTrainingSwashbucklerFeature",
                                                  "Flying Blade Training",
                                                  "At 5th level, a flying blade gains a +1 bonus on attack and damage rolls when using daggers or starknives in combat. When a flying blade wields a dagger or starknife, she gains the benefit of the Improved Critical feat with those weapons. Additionally, a flying blade increases the range increment of a thrown dagger or starknife by 5 feet. The increase of range increment stacks with that of precise throw.\n"
                                                  + "Every 4 levels thereafter, the bonus on attack and damage rolls increases by 1, and the range increment increases by 5 feet.",
                                                  "48cb1a6729cf4c8c8f6a36ed20baa3b3",
                                                  dagger.Icon,
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
            var spellstrike = library.Get<BlueprintFeature>("be50f4e97fff8a24ba92561f1694a945");

            flying_blade_mastery = CreateFeature("FlyingBladeMasterySwashbucklerFeature",
                                                 "Flying Blade Mastery",
                                                 "At 20th level, when an attack that a flying blade makes with a dagger or starknife threatens a critical hit, that critical hit is automatically confirmed. Furthermore, the critical modifiers of daggers and starknives increase by 1 (×2 becomes ×3, and so on).",
                                                 "e0885d853c6748bd81dee057cd4d1feb",
                                                 spellstrike.Icon,
                                                 FeatureGroup.None,
                                                 Create<CritAutoconfirmWithFlyingBladeWeapons>(),
                                                 Create<IncreasedCriticalMultiplierWithFlyingBladeWeapons>()
                                                 );
        }

        static void createSubtleThrow()
        {
            var displacement = library.Get<BlueprintAbility>("903092f6488f9ce45a80943923576ab3");

            var subtle_throw_buff_first = CreateBuff("SubtleThrowLevelOneSwashbucklerBuff",
                                                     "Subtle Throw",
                                                     "At 1st level, a flying blade can spend 1 panache point as part of a ranged attack with a dagger or starknife to make it without provoking attacks of opportunity.",
                                                     "09d754756df3471892a80485684a3929",
                                                     displacement.Icon,
                                                     null,
                                                     Create<AddInitiatorAttackWithWeaponTrigger>(a => { a.CheckWeaponCategory = true; a.Category = WeaponCategory.Dagger; a.CheckWeaponRangeType = true; a.RangeType = WeaponRangeType.Ranged; a.Action = CreateActionList(Create<SpendPanache>(s => { s.amount = 1; s.resource = Swashbuckler.panache_resource; })); a.ActionsOnInitiator = true; a.OnlyHit = false; }),
                                                     Create<AddInitiatorAttackWithWeaponTrigger>(a => { a.CheckWeaponCategory = true; a.Category = WeaponCategory.Starknife; a.CheckWeaponRangeType = true; a.RangeType = WeaponRangeType.Ranged; a.Action = CreateActionList(Create<SpendPanache>(s => { s.amount = 1; s.resource = Swashbuckler.panache_resource; })); a.ActionsOnInitiator = true; a.OnlyHit = false; }),
                                                     Create<PointBlankMaster>(p => p.Category = WeaponCategory.Dagger),
                                                     Create<PointBlankMaster>(p => p.Category = WeaponCategory.Starknife)
                                                     );

            var subtle_throw_buff_sixth = CreateBuff("SubtleThrowLevelSixSwashbucklerBuff",
                                                     "Subtle Throw",
                                                     "At 6th level, as a swift action she can spend 1 panache point to make all of her ranged attacks with daggers or starknives without provoking attacks of opportunity until the start of her next turn.",
                                                     "bc48843169584ae1941e90bb77a2983e",
                                                     displacement.Icon,
                                                     null,
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
            var stunning_barrier = library.Get<BlueprintAbility>("a5ec7892fb1c2f74598b3a82f3fd679f");

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
                                                       stunning_barrier.Icon,
                                                       null,
                                                       Create<AddStatBonus>(a => { a.Stat = StatType.AdditionalAttackBonus; a.Descriptor = ModifierDescriptor.UntypedStackable; a.Value = -4; })
                                                       );

            var disrupting_counter_buff = CreateBuff("DisruptingCounterSwashbucklerBuff",
                                                     disrupting_counter_debuff.Name,
                                                     disrupting_counter_debuff.Description,
                                                     "d5c1f919613a464eaa74339e8c8140d4",
                                                     disrupting_counter_debuff.Icon,
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
                                                    disrupting_counter_buff.Icon,
                                                    FeatureGroup.None,
                                                    Helpers.CreateAddFact(disrupting_counter_ability)
                                                    );
        }

        static void createPreciseThrow()
        {
            precise_throw_deed = CreateFeature("PreciseThrowSwashbucklerFeature",
                                               "Precise Throw",
                                               "At 3rd level, as long as she has at least 1 panache point, a flying blade can use her precise strike with a thrown dagger or starknife as long as the target is within 60 feet of her, and she increases the range increment of these weapons by 5 feet.",
                                               "da8e9d51b0754316bf3db2bc58bcb89a",
                                               null,
                                               FeatureGroup.None
                                               );
        }

        static void createTargetedThrow()
        {
            var disarm = library.Get<BlueprintAbility>("45d94c6db453cfc4a9b99b72d6afe6f6"); //Disarm ability
            var immune_to_crits = library.Get<BlueprintFeature>("ced0f4e5d02d5914a9f9ff74acacf26d"); //Immunity to crits feature
            var immune_to_mind_affecting = library.Get<BlueprintFeature>("3eb606c0564d0814ea01a824dbe42fb0"); //Immunity to mind affecting
            var immune_to_trip = library.Get<BlueprintFeature>("c1b26f97b974aec469613f968439e7bb");
            var immune_to_prone = library.Get<BlueprintBuff>("7e3cd4e16a990ab4e9ffa5d9ca3c4870");
            var confusion_buff = library.Get<BlueprintBuff>("886c7407dc629dc499b9f1465ff382df");
            var staggered_buff = library.Get<BlueprintBuff>("df3950af5a783bd4d91ab73eb8fa0fd3");
            var magus_feats = library.Get<BlueprintFeatureSelection>("66befe7b24c42dd458952e3c47c93563");
            var grease = library.Get<BlueprintAbility>("95851f6e85fe87d4190675db0419d112");
            var true_strike = library.Get<BlueprintAbility>("2c38da66e5a599347ac95b3294acbe00");

            var base_name = "Targeted Throw";
            //TODO Projectile for attack
            var targeted_strike_arms_ability = CreateAbility("TargetedThrowArmsSwashbucklerAbility",
                                                             base_name + " - Arms",
                                                             "The target takes no damage from the attack, but it is disarmed.",
                                                             "3e6ff2d5d32a47919fd41178b4584efe",
                                                             magus_feats.Icon,
                                                             AbilityType.Extraordinary,
                                                             CommandType.Standard,
                                                             AbilityRange.Weapon,
                                                             "",
                                                             "",
                                                             Create<AbilityCasterThrowingWeaponCheck>(),
                                                             Create<AbilityTargetHasNoFactUnless>(a => { a.CheckedFacts = new Kingmaker.Blueprints.Facts.BlueprintUnitFact[] { immune_to_crits }; a.UnlessFact = null; }),
                                                             Create<AbilityTargetNotImmuneToPrecision>(),
                                                             Helpers.Create<AbilityDeliverHitWithMeleeWeapon>(),
                                                             Helpers.CreateRunActions(Create<DisarmTarget>()),
                                                             Helpers.Create<AttackAnimation>(),
                                                             Create<AbilityResourceLogic>(a => { a.IsSpendResource = true; a.Amount = 1; a.CostIsCustom = false; a.RequiredResource = Swashbuckler.panache_resource; })
                                                             );
            Common.setAsFullRoundAction(targeted_strike_arms_ability);
            targeted_strike_arms_ability.setMiscAbilityParametersSingleTargetRangedHarmful(works_on_allies: false);
            targeted_strike_arms_ability.NeedEquipWeapons = true;

            var targeted_strike_head_ability = CreateAbility("TargetedThrowHeadSwashbucklerAbility",
                                                             base_name + " - Head",
                                                             "The target is confused for 1 round. This is a mind-affecting effect.",
                                                             "e8153debdf824709851ca03767be5e1c",
                                                             confusion_buff.Icon,
                                                             AbilityType.Extraordinary,
                                                             CommandType.Standard,
                                                             AbilityRange.Weapon,
                                                             "",
                                                             "",
                                                             Create<AbilityCasterThrowingWeaponCheck>(),
                                                             Create<AbilityTargetHasNoFactUnless>(a => { a.CheckedFacts = new BlueprintUnitFact[] { immune_to_crits, immune_to_mind_affecting }; a.UnlessFact = null; }),
                                                             Create<AbilityTargetNotImmuneToPrecision>(),
                                                             Create<AbilityDeliverAttackWithWeaponOnHit>(),
                                                             Create<AbilityResourceLogic>(a => { a.IsSpendResource = true; a.Amount = 1; a.CostIsCustom = false; a.RequiredResource = Swashbuckler.panache_resource; }),
                                                             CreateRunActions(Common.createContextActionApplyBuff(confusion_buff, Helpers.CreateContextDuration(1), dispellable: false))
                                                             );
            Common.setAsFullRoundAction(targeted_strike_head_ability);
            targeted_strike_head_ability.setMiscAbilityParametersSingleTargetRangedHarmful(animation: Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate, animation_style: CastAnimationStyle.CastActionPoint, works_on_allies: false);
            targeted_strike_head_ability.NeedEquipWeapons = true;

            var targeted_strike_legs_ability = CreateAbility("TargetedThrowLegsSwashbucklerAbility",
                                                             base_name + " - Legs",
                                                             "The target is knocked prone. Creatures that are immune to trip attacks are immune to this effect.",
                                                             "d4cee87127aa4b8896c2d6f3f8ece2b9",
                                                             grease.Icon,
                                                             AbilityType.Extraordinary,
                                                             CommandType.Standard,
                                                             AbilityRange.Weapon,
                                                             "",
                                                             "",
                                                             Create<AbilityCasterThrowingWeaponCheck>(),
                                                             Create<AbilityTargetHasNoFactUnless>(a => { a.CheckedFacts = new BlueprintUnitFact[] { immune_to_crits, immune_to_prone }; a.UnlessFact = null; }),
                                                             Create<AbilityTargetNotImmuneToPrecision>(),
                                                             Create<AbilityDeliverAttackWithWeaponOnHit>(),
                                                             Create<AbilityResourceLogic>(a => { a.IsSpendResource = true; a.Amount = 1; a.CostIsCustom = false; a.RequiredResource = Swashbuckler.panache_resource; }),
                                                             CreateRunActions(Helpers.Create<ContextActionKnockdownTarget>())
                                                             );
            Common.setAsFullRoundAction(targeted_strike_legs_ability);
            targeted_strike_legs_ability.setMiscAbilityParametersSingleTargetRangedHarmful(animation: Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate, animation_style: CastAnimationStyle.CastActionPoint, works_on_allies: false);
            targeted_strike_legs_ability.NeedEquipWeapons = true;

            var targeted_strike_torso_ability = CreateAbility("TargetedThrowTorsoSwashbucklerAbility",
                                                             base_name + " - Torso",
                                                             "The target is staggered for 1 round.",
                                                             "43ca1a3ca060458ba6ba1ab19baeea98",
                                                             staggered_buff.Icon,
                                                             AbilityType.Extraordinary,
                                                             CommandType.Standard,
                                                             AbilityRange.Weapon,
                                                             "",
                                                             "",
                                                             Create<AbilityCasterThrowingWeaponCheck>(),
                                                             Create<AbilityTargetHasNoFactUnless>(a => { a.CheckedFacts = new BlueprintUnitFact[] { immune_to_crits }; a.UnlessFact = null; }),
                                                             Create<AbilityTargetNotImmuneToPrecision>(),
                                                             Create<AbilityDeliverAttackWithWeaponOnHit>(),
                                                             Create<AbilityResourceLogic>(a => { a.IsSpendResource = true; a.Amount = 1; a.CostIsCustom = false; a.RequiredResource = Swashbuckler.panache_resource; }),
                                                             CreateRunActions(Common.createContextActionApplyBuff(staggered_buff, Helpers.CreateContextDuration(1), dispellable: false))
                                                             );
            Common.setAsFullRoundAction(targeted_strike_torso_ability);
            targeted_strike_torso_ability.setMiscAbilityParametersSingleTargetRangedHarmful(animation: Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate, animation_style: CastAnimationStyle.CastActionPoint, works_on_allies: false);
            targeted_strike_torso_ability.NeedEquipWeapons = true;

            var wrapper = Common.createVariantWrapper("SwashbucklerTargetedThrowAbility", "5e69192c181b4a0d97cacc72f792b619", targeted_strike_arms_ability, targeted_strike_head_ability, targeted_strike_legs_ability, targeted_strike_torso_ability);
            wrapper.SetName("Targeted Throw");
            wrapper.SetDescription("At 7th level, a flying blade can target individual body parts. This deed functions as the swashbuckler’s targeted strike deed, but the flying blade can also use this deed when making ranged attacks with either a dagger or a starknife as long as the target is within 60 feet of the flying blade.");
            wrapper.SetIcon(true_strike.Icon);

            targeted_throw_deed = CreateFeature("TargetedThrowSwashbucklerFeature",
                                                wrapper.Name,
                                                wrapper.Description,
                                                "e91c94a56c514b3eab13eaded01e1df0",
                                                wrapper.Icon,
                                                FeatureGroup.None,
                                                Helpers.CreateAddFact(wrapper)
                                                );
        }

        static void createBleedingWound()
        {
            bleeding_wound_deed = CreateFeature("BleedingWoundFlyingBladeSwashbucklerFeature",
                                                "Bleeding Throw",
                                                "At 11th level, a flying blade can deal bleed damage as part of an attack. This deed functions as the swashbuckler’s bleeding wound deed, but the flying blade can also use this deed when making ranged attacks with either a dagger or a starknife as long as the target is within 60 feet of the flying blade. This deed alters bleeding wound.",
                                                "a3726674829943fca3c97d519831b67f",
                                                null,
                                                FeatureGroup.None
                                                );
        }

        static void createPerfectThrow()
        {
            var exploit_weakness = library.Get<BlueprintFeature>("374a73288a36e2d4f9e54c75d2e6e573");

            var buff = CreateBuff("PerfectThrowSwashbucklerBuff",
                                  "",
                                  "",
                                  "33970191791941debcecccad73201e70",
                                  null,
                                  null,
                                  Create<AttackTargetsTouchAC>(),
                                  Create<IgnoreAllDR>()
                                  );
            buff.SetBuffFlags(BuffFlags.HiddenInUi);

            var perfect_throw_ability = CreateAbility("PerfectThrowSwashbucklerAbility",
                                                      "Perfect Throw",
                                                      "At 15th level, a flying blade can pool all of her attack potential into a single attack. This deed functions as the swashbuckler’s perfect strike deed, but the flying blade must use this deed when making ranged attacks with either a dagger or a starknife, and she can use this deed only on targets within 60 feet of her.",
                                                      "84b3018b36774f939faf1f527976ca2b",
                                                      exploit_weakness.Icon,
                                                      AbilityType.Extraordinary,
                                                      CommandType.Standard,
                                                      AbilityRange.Weapon,
                                                      "",
                                                      "",
                                                      Create<AbilityCasterThrowingWeaponCheck>(),
                                                      Create<AbilityCasterHasAtLeastOnePanache>(a => a.resource = Swashbuckler.panache_resource),
                                                      Helpers.Create<AttackAnimation>(),
                                                      Helpers.CreateRunActions(Common.createContextActionOnContextCaster(Common.createContextActionApplyBuff(buff, Helpers.CreateContextDuration(1), dispellable: false)),
                                                                               Common.createContextActionAttack(action_on_miss: Helpers.CreateActionList(Common.createContextActionOnContextCaster(Common.createContextActionRemoveBuff(buff)))
                                                                                                                )
                                                                               )
                                                      );
            Common.setAsFullRoundAction(perfect_throw_ability);
            perfect_throw_ability.setMiscAbilityParametersSingleTargetRangedHarmful(works_on_allies: false);
            perfect_throw_ability.NeedEquipWeapons = true;

            perfect_throw_deed = CreateFeature("PerfectThrowSwashbucklerFeature",
                                               perfect_throw_ability.Name,
                                               perfect_throw_ability.Description,
                                               "d60d34a512304308ba15398d335ae737",
                                               perfect_throw_ability.Icon,
                                               FeatureGroup.None,
                                               Helpers.CreateAddFact(perfect_throw_ability)
                                               );
        }
    }
}
