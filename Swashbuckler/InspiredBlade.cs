using CallOfTheWild;
using CallOfTheWild.NewMechanics;
using CallOfTheWild.ResourceMechanics;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
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
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using static CallOfTheWild.Helpers;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;

namespace Derring_Do
{
    public class InspiredBlade
    {
        static LibraryScriptableObject library = Main.library;

        static public BlueprintArchetype inspired_blade;

        static public BlueprintFeature inspired_panache;

        static public BlueprintFeature inspired_finesse;

        static public BlueprintFeature rapier_training;

        static public BlueprintFeature rapier_weapon_mastery;

        static public BlueprintFeature inspired_strike_deed;
        static public BlueprintBuff inspired_strike_attack_buff;
        static public BlueprintBuff inspired_strike_critical_buff;

        static public void create()
        {
            Main.DebugLog("Creating Inspired Blade archetype");
            inspired_blade = Helpers.Create<BlueprintArchetype>(a =>
            {
                a.name = "InspiredBladeArchetype";
                a.LocalizedName = Helpers.CreateString($"{a.name}.Name", "Inspired Blade");
                a.LocalizedDescription = Helpers.CreateString($"{a.name}.Description", "An inspired blade is both a force of personality and a sage of swordplay dedicated to the perfection of combat with the rapier. They use the science and geometry with swordplay to beautiful and deadly effect.");
            });
            SetField(inspired_blade, "m_ParentClass", Swashbuckler.swashbuckler_class);
            inspired_blade.OverrideAttributeRecommendations = true;
            inspired_blade.RecommendedAttributes = new StatType[] { StatType.Dexterity, StatType.Charisma, StatType.Intelligence };
            library.AddAsset(inspired_blade, "9dbb6ac330874b358b2b041287f99d18");

            inspired_blade.ReplaceStartingEquipment = true;
            inspired_blade.StartingItems = new BlueprintItem[]
            {
                library.Get<BlueprintItem>("afbe88d27a0eb544583e00fa78ffb2c7"), //StuddedStandard
                library.Get<BlueprintItem>("78aef04821150bd479314bc974ea73e2"), //Buckler
                library.Get<BlueprintItem>("bc93a78d71bef084fa155e529660ed0d"), //PotionOfShieldOfFaith
                library.Get<BlueprintItem>("d52566ae8cbe8dc4dae977ef51c27d91"), //PotionOfCureLightWounds
            };

            //Create Features
            createInspiredPanache();
            createInspiredFinesse();
            createRapierTraining();
            createRapierWeaponMastery();
            createInspiredStrike();

            inspired_blade.RemoveFeatures = new LevelEntry[] { Helpers.LevelEntry(1, Swashbuckler.panache, Swashbuckler.swashbuckler_finesse),
                                                               Helpers.LevelEntry(5, Swashbuckler.swashbuckler_weapon_training),
                                                               Helpers.LevelEntry(9, Swashbuckler.swashbuckler_weapon_training),
                                                               Helpers.LevelEntry(11, Swashbuckler.bleeding_wound_deed),
                                                               Helpers.LevelEntry(13, Swashbuckler.swashbuckler_weapon_training),
                                                               Helpers.LevelEntry(17, Swashbuckler.swashbuckler_weapon_training),
                                                               Helpers.LevelEntry(20, Swashbuckler.swashbuckler_weapon_mastery),
                                                             };

            inspired_blade.AddFeatures = new LevelEntry[] { Helpers.LevelEntry(1, inspired_panache, inspired_finesse),
                                                            Helpers.LevelEntry(5, rapier_training),
                                                            Helpers.LevelEntry(9, rapier_training),
                                                            Helpers.LevelEntry(11, inspired_strike_deed),
                                                            Helpers.LevelEntry(13, rapier_training),
                                                            Helpers.LevelEntry(17, rapier_training),
                                                            Helpers.LevelEntry(20, rapier_weapon_mastery),
                                                          };

            Swashbuckler.swashbuckler_progression.UIDeterminatorsGroup = Swashbuckler.swashbuckler_progression.UIDeterminatorsGroup.AddToArray(inspired_panache, inspired_finesse);
            Swashbuckler.swashbuckler_progression.UIGroups = Swashbuckler.swashbuckler_progression.UIGroups.AddToArray(CreateUIGroup(rapier_training, rapier_weapon_mastery));
            Swashbuckler.swashbuckler_class.Archetypes = Swashbuckler.swashbuckler_class.Archetypes.AddToArray(inspired_blade);
        }

        static void createInspiredPanache()
        {
            inspired_panache = CreateFeature("InspiredPanacheFeature",
                                             "Inspired Panache",
                                             "Each day, an inspired blade gains a number of panache points equal to her Charisma modifier (minimum 1) plus her Intelligence modifier (minimum 1), instead of just her Charisma modifier.\n"
                                             + "Unlike other swashbucklers, an inspired blade regains no panache from scoring a killing blow. She regains panache only from scoring a critical hit with a rapier.",
                                             "8b7d121bdeec4050a31271768cb43b61",
                                             null,
                                             FeatureGroup.None,
                                             Swashbuckler.panache_resource.CreateAddAbilityResource(),
                                             Create<RestorePanacheAttackRollTrigger>(a => 
                                             { a.IsInspiredBlade = true; a.CriticalHit = true; a.Action = CreateActionList(Swashbuckler.restore_panache); a.deadly_stab_buff = Swashbuckler.deadly_stab_buff; a.DuelistWeapon = false; a.CheckWeaponCategory = true; a.Category = WeaponCategory.Rapier; }),
                                             Helpers.Create<ContextIncreaseResourceAmount>(c =>
                                             {
                                                 c.Value = Helpers.CreateContextValue(AbilityRankType.ProjectilesCount);
                                                 c.Resource = Swashbuckler.panache_resource;
                                             }),
                                             Helpers.CreateContextRankConfig(type: AbilityRankType.ProjectilesCount, baseValueType: ContextRankBaseValueType.StatBonus, stat: StatType.Intelligence, min: 1),
                                             Helpers.Create<RecalculateOnStatChange>(r => r.Stat = StatType.Intelligence),
                                             CreateAddFacts(Swashbuckler.panache_gain_base)
                                             );
        }

        static void createInspiredFinesse()
        {
            var weapon_finesse = library.Get<BlueprintFeature>("90e54424d682d104ab36436bd527af09");
            var weapon_focus = library.Get<BlueprintFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e");
            var weapon_focus_rapier = library.Get<BlueprintParametrizedFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e");

            inspired_finesse = CreateFeature("InspiredFinesseSwashbucklerFeature",
                                             "Inspired Finesse",
                                             "At 1st level, an inspired blade gains the benefits of Weapon Finesse with the rapier (this ability counts as having the Weapon Finesse feat for the purpose of meeting feat prerequisites) and gains Weapon Focus (rapier) as a bonus feat.",
                                             "541b48b726104373ae3f8780bbdb8be7",
                                             weapon_focus.Icon,
                                             FeatureGroup.None,
                                             Create<AttackStatReplacementForRapier>(),
                                             Common.createAddParametrizedFeatures(weapon_focus_rapier, WeaponCategory.Rapier)
                                             );
            library.Get<BlueprintFeature>("90e54424d682d104ab36436bd527af09").AddComponent(Create<FeatureReplacement>(f => f.replacement_feature = inspired_finesse)); // weapon finesse
            weapon_finesse.GetComponent<RecommendationNoFeatFromGroup>().Features = weapon_finesse.GetComponent<RecommendationNoFeatFromGroup>().Features.AddToArray(inspired_finesse);
        }

        static void createRapierTraining()
        {
            var weapon_training = library.Get<BlueprintFeatureSelection>("b8cecf4e5e464ad41b79d5b42b76b399");
            var arcane_pool = library.Get<BlueprintFeature>("3ce9bb90749c21249adc639031d5eed1");
            var improved_critical = library.Get<BlueprintParametrizedFeature>("f4201c85a991369408740c6888362e20");

            rapier_training = CreateFeature("RapierTrainingSwashbucklerFeature",
                                            "Rapier Training",
                                            "At 5th level, an inspired blade gains a +1 bonus on attack rolls and a +2 bonus on damage rolls with rapiers. While wielding a rapier, she gains the benefit of the Improved Critical feat. These attack and damage bonuses increase by 1 for every 4 levels beyond 5th (to a maximum of +4 on attack rolls and +5 on damage rolls at 17th level).",
                                            "6b75c9ebc84748b0a404a00a280c2d15",
                                            arcane_pool.Icon,
                                            FeatureGroup.None,
                                            Create<WeaponTraining>(),
                                            Create<WeaponTrainingBonuses>(w => { w.Stat = StatType.AdditionalAttackBonus; w.Descriptor = ModifierDescriptor.UntypedStackable; }),
                                            Create<WeaponTrainingBonuses>(w => { w.Stat = StatType.AdditionalDamage; w.Descriptor = ModifierDescriptor.UntypedStackable; }),
                                            Create<ImprovedCriticalOnWieldingRapier>(),
                                            Create<ContextWeaponCategoryDamageBonus>(c => { c.categories = new WeaponCategory[] { WeaponCategory.Rapier }; c.Value = 1; })
                                            );

            rapier_training.Ranks = 4;
            rapier_training.ReapplyOnLevelUp = true;
            rapier_training.AddComponent(CreateContextRankConfig(ContextRankBaseValueType.FeatureRank, feature: rapier_training));
            improved_critical.GetComponent<RecommendationNoFeatFromGroup>().Features = improved_critical.GetComponent<RecommendationNoFeatFromGroup>().Features.AddToArray(rapier_training);
        }

        static void createRapierWeaponMastery()
        {
            var arcane_accuracy = library.Get<BlueprintFeature>("2eacbdbf1c4f4134aa7fea99ab8763dc");

            rapier_weapon_mastery = CreateFeature("RapierWeaponMasterySwashbucklerFeature",
                                                  "Rapier Weapon Mastery",
                                                  "At 20th level, when an inspired blade threatens a critical hit with a rapier, that critical hit is automatically confirmed. Furthermore, the critical threat range increases by 1 (this increase to the critical threat range stacks with the increase from rapier training, to a total threat range of 13–20), and the critical modifier of the weapon increases by 1 (×2 becomes ×3, for example).",
                                                  "dc5787dcd28649ec8c45a11280aa56fe",
                                                  arcane_accuracy.Icon,
                                                  FeatureGroup.None,
                                                  Create<CritAutoconfirmWithRapiers>(),
                                                  Create<IncreasedCriticalMultiplierAndThreatWithRapiers>()
                                                  );
        }

        static void createInspiredStrike()
        {
            var arcane_weapon_speed = library.Get<BlueprintBuff>("f9e6281bffd7030499e2ab469e15f1a7");
            var arcane_weapon_keen = library.Get<BlueprintBuff>("49083bf0cdd00ec4dacbffb4be26e69a");

            inspired_strike_attack_buff = CreateBuff("InspiredStrikeAttackBonusSwashbucklerBuff",
                                                     "Inspired Strike (Attack Bonus)",
                                                     "At 11th level, an inspired blade can spend 1 panache point when making an attack with a rapier to gain an insight bonus on that attack roll equal to her Intelligence modifier (minimum +1).",
                                                     "6c5fd556bc4f4392ace2156c97537fc7",
                                                     arcane_weapon_speed.Icon,
                                                     null, //TODO fx
                                                     Create<InspiredStrikeLogic>()
                                                     );

            var apply_buff = Common.createContextActionApplyBuff(inspired_strike_attack_buff, CreateContextDuration(1), dispellable: false);

            var inspired_strike_attack_ability = CreateAbility("InspiredStrikeAttackBonusSwashbucklerAbility",
                                                               inspired_strike_attack_buff.Name,
                                                               inspired_strike_attack_buff.Description,
                                                               "5ff08b489cc641bf8463029c2ec7d58b",
                                                               inspired_strike_attack_buff.Icon,
                                                               AbilityType.Extraordinary,
                                                               CommandType.Free,
                                                               AbilityRange.Personal,
                                                               "",
                                                               "",
                                                               CreateRunActions(new GameAction[] { apply_buff }),
                                                               Create<AbilityResourceLogic>(a => { a.IsSpendResource = true; a.Amount = 1; a.CostIsCustom = false; a.RequiredResource = Swashbuckler.panache_resource; })
                                                               );

            inspired_strike_critical_buff = CreateBuff("InspiredStrikeCriticalBonusSwashbucklerBuff",
                                                       "Inspired Strike (Critical Bonus)",
                                                       "When an inspired blade hits with an attack augmented by inspired strike, she can spend 1 additional panache point to make the hit a critical threat, though if she does so, she does not regain panache if she confirms that critical threat.",
                                                       "8d45cc0c4fde464db38389b35e6d59e6",
                                                       arcane_weapon_keen.Icon,
                                                       null //TODO fx
                                                       );

            var inspired_strike_crit_ability = CreateActivatableAbility("InspiredStrikeCriticalBonusSwashbucklerAbility",
                                                                        inspired_strike_critical_buff.Name,
                                                                        inspired_strike_critical_buff.Description,
                                                                        "86e1b4dcd2d948d98154795e594a8c30",
                                                                        inspired_strike_critical_buff.Icon,
                                                                        inspired_strike_critical_buff,
                                                                        AbilityActivationType.Immediately,
                                                                        CommandType.Free,
                                                                        null,
                                                                        Helpers.CreateActivatableResourceLogic(Swashbuckler.panache_resource, ActivatableAbilityResourceLogic.ResourceSpendType.Never),
                                                                        Common.createActivatableAbilityRestrictionHasFact(inspired_strike_attack_buff)
                                                                        );

            inspired_strike_deed = CreateFeature("InspiredStrikeSwashbucklerFeature",
                                                 "Inspired Strike",
                                                 "At 11th level, an inspired blade can spend 1 panache point when making an attack with a rapier to gain an insight bonus on that attack roll equal to her Intelligence modifier (minimum +1). When an inspired blade hits with an attack augmented by inspired strike, she can spend 1 additional panache point to make the hit a critical threat, though if she does so, she does not regain panache if she confirms that critical threat. The cost of this deed cannot be reduced by abilities such as Signature Deed.",
                                                 "bd648a4dc1f64001af8b658e194057e2",
                                                 arcane_weapon_speed.Icon,
                                                 FeatureGroup.None,
                                                 CreateAddFacts(new BlueprintUnitFact[] { inspired_strike_attack_ability, inspired_strike_crit_ability })
                                                 );
        }
    }
}
