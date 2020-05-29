using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Localization;
using Kingmaker.RuleSystem;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using static Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilityResourceLogic;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.PubSubSystem;
using Newtonsoft.Json;
using CallOfTheWild;
using static CallOfTheWild.Helpers;
using CallOfTheWild.WeaponTrainingMechanics;
using CallOfTheWild.ResourceMechanics;
using CallOfTheWild.NewMechanics;
using CallOfTheWild.HoldingItemsMechanics;
using CallOfTheWild.StatReplacementMechanics;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using System.Runtime.CompilerServices;
using Harmony12;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Designers.EventConditionActionSystem.Actions;

namespace Derring_Do
{
    public class Swashbuckler
    {
        static LibraryScriptableObject library => Main.library;

        static public BlueprintCharacterClass swashbuckler_class;

        static public BlueprintProgression swashbuckler_progression;

        //DONE
        static public BlueprintFeature swashbuckler_proficiencies;

        //TEST - DEEDS
        static public BlueprintFeature panache;

        //TEST - DEEDS
        static public BlueprintAbilityResource panache_resource;

        //DONE
        static public BlueprintFeatureSelection fighter_feat;

        //DONE
        static public BlueprintFeature swashbuckler_fighter_feat_prerequisite_replacement;

        //TODO - TWEAK CHA SWAP TO JUST BE FOR COMBAT FEATS
        static public BlueprintFeature swashbuckler_finesse;

        //TODO
        static public BlueprintFeature charmed_life;

        //DONE - TEST
        static public BlueprintFeature nimble_unlock;

        //DONE - TEST
        static public BlueprintFeature swashbuckler_weapon_training;

        //DONE - WORKING
        static public BlueprintFeature swashbuckler_weapon_mastery;

        // TODO: Deeds

        internal static void createSwashbucklerClass()
        {
            
            var duelist_class = GetClass("4e0ea99612ae87a499c7fb0588e31828");
            var fighter_class = GetClass("48ac8db94d5de7645906c7d0ad3bcfbd");
            var magus_class = GetClass("45a4607686d96a1498891b3286121780");
            var rogue_class = GetClass("299aa766dee3cbf4790da4efb8c72484");

            swashbuckler_class = Create<BlueprintCharacterClass>();
            swashbuckler_class.name = "SwashbucklerClass";
            library.AddAsset(swashbuckler_class, "b1c7989ee7f04af8b11309f02b34cb4a");

            swashbuckler_class.LocalizedName = CreateString("Swashbuckler.Name", "Swashbuckler");
            swashbuckler_class.LocalizedDescription = CreateString("Swashbuckler.Description",
                                                                 "Whereas many warriors brave battle encased in suits of armor and wielding large and powerful weapons, swashbucklers rely on speed, agility, and panache. Swashbucklers dart in and out of the fray, wearing down opponents with lunges and feints, all while foiling the powerful attacks against them with a flick of the wrist and a flash of the blade. Their deft parries and fatal ripostes are carnage elevated to an art form. Some may be arrogant and devil-may-care, but behind this veneer lie people deeply dedicated to their craft. Those of smaller races are particularly driven to prove that the right mix of discipline and daring is the perfect counter to size and strength, and enjoy nothing more than taking down lumbering brutes and bullies.\n"
                                                                 + "Role: Combining fancy footwork with quick and precise lunges, swashbucklers dart in and out of battle, harassing and thwarting their opponents. These fast and agile combatants serve as protectors for spellcasters and flank mates for rogues and slayers, while waiting for the opportunity to show panache and score the killing blow on some lumbering hulk. Swashbucklers often face death with wry humor, mocking it with jabbing wit."
                                                                 );

            swashbuckler_class.m_Icon = duelist_class.Icon;
            swashbuckler_class.SkillPoints = duelist_class.SkillPoints;
            swashbuckler_class.HitDie = DiceType.D10;
            swashbuckler_class.BaseAttackBonus = fighter_class.BaseAttackBonus;
            swashbuckler_class.FortitudeSave = rogue_class.FortitudeSave;
            swashbuckler_class.ReflexSave = rogue_class.ReflexSave;
            swashbuckler_class.WillSave = rogue_class.WillSave;
            swashbuckler_class.ClassSkills = new StatType[] { StatType.SkillAthletics, StatType.SkillMobility, StatType.SkillKnowledgeWorld, StatType.SkillPerception, StatType.SkillPersuasion };
            swashbuckler_class.StartingGold = fighter_class.StartingGold;
            swashbuckler_class.PrimaryColor = fighter_class.PrimaryColor;
            swashbuckler_class.SecondaryColor = fighter_class.SecondaryColor;
            swashbuckler_class.RecommendedAttributes = new StatType[] { StatType.Dexterity, StatType.Charisma };
            swashbuckler_class.NotRecommendedAttributes = new StatType[0];
            swashbuckler_class.EquipmentEntities = magus_class.EquipmentEntities;
            swashbuckler_class.MaleEquipmentEntities = magus_class.MaleEquipmentEntities;
            swashbuckler_class.FemaleEquipmentEntities = magus_class.FemaleEquipmentEntities;
            swashbuckler_class.ComponentsArray = magus_class.ComponentsArray;
            // TODO: Starting equipment
            swashbuckler_class.StartingItems = magus_class.StartingItems;
            swashbuckler_class.StartingGold = magus_class.StartingGold;

            createSwashbucklerProgression();
            swashbuckler_class.Progression = swashbuckler_progression;

            // TODO: Archetypes

            swashbuckler_class.Archetypes = new BlueprintArchetype[] { };
            RegisterClass(swashbuckler_class);
        }

        static BlueprintCharacterClass[] getSwashbucklerArray()
        {
            return new BlueprintCharacterClass[] { swashbuckler_class };
        }

        static void createSwashbucklerProgression()
        {
            createSwashbucklerProficiencies();
            createNimble();
            createPanachePoolAndDeeds();
            createSwashbucklerFinesse();
            createCharmedLife();
            createSwashbucklerFighterFeatPrerequisiteReplacement();
            fighter_feat = library.CopyAndAdd<BlueprintFeatureSelection>("41c8486641f7d6d4283ca9dae4147a9f", "SwashbucklerBonusFeat", "043bee82f2dc47039e59d40d826e4bf8");
            fighter_feat.SetDescription("At 4th level and every 4 levels thereafter, a swashbuckler gains a bonus feat in addition to those gained from normal advancement. These bonus feats must be selected from those listed as combat feats.");
            createSwashbucklerWeaponTraining();
            createSwashbucklerWeaponMastery();
            // TODO: create methods for Swashbuckler abilities

            swashbuckler_progression = CreateProgression("SwashbucklerProgression",
                                                           swashbuckler_class.Name,
                                                           swashbuckler_class.Description,
                                                           "4fc04fcd05cb4a07a450a2fb96a6cc11",
                                                           swashbuckler_class.Icon,
                                                           FeatureGroup.None);
            swashbuckler_progression.Classes = getSwashbucklerArray();

            swashbuckler_progression.LevelEntries = new LevelEntry[] { LevelEntry(1, swashbuckler_proficiencies, swashbuckler_finesse, panache),
                                                                       LevelEntry(2, charmed_life),
                                                                       LevelEntry(3, nimble_unlock),
                                                                       LevelEntry(4, fighter_feat, swashbuckler_fighter_feat_prerequisite_replacement),
                                                                       LevelEntry(5, swashbuckler_weapon_training),
                                                                       LevelEntry(6, charmed_life),
                                                                       LevelEntry(7, nimble_unlock),
                                                                       LevelEntry(8, fighter_feat),
                                                                       LevelEntry(9, swashbuckler_weapon_training),
                                                                       LevelEntry(10, charmed_life),
                                                                       LevelEntry(11, nimble_unlock),
                                                                       LevelEntry(12, fighter_feat),
                                                                       LevelEntry(13, swashbuckler_weapon_training),
                                                                       LevelEntry(14, charmed_life),
                                                                       LevelEntry(15, nimble_unlock),
                                                                       LevelEntry(16, fighter_feat),
                                                                       LevelEntry(17, swashbuckler_weapon_training),
                                                                       LevelEntry(18, charmed_life),
                                                                       LevelEntry(19, nimble_unlock),
                                                                       LevelEntry(20, fighter_feat, swashbuckler_weapon_mastery),
                                                                       };
            swashbuckler_progression.UIDeterminatorsGroup = new BlueprintFeatureBase[] { swashbuckler_proficiencies, swashbuckler_finesse, panache };
            swashbuckler_progression.UIGroups = new UIGroup[] { CreateUIGroup(fighter_feat),
                                                                CreateUIGroup(nimble_unlock),
                                                                CreateUIGroup(swashbuckler_weapon_training),
                                                                CreateUIGroup(charmed_life)
                                                                };
        }

        static void createSwashbucklerProficiencies()
        {
            swashbuckler_proficiencies = library.CopyAndAdd<BlueprintFeature>("a23591cc77086494ba20880f87e73970", "SwashbucklerProficiencies", "c8935bfe531e48699201aaa47ff8a556"); //from fighter
            swashbuckler_proficiencies.ReplaceComponent<AddFacts>(c => c.Facts = c.Facts.RemoveFromArray(library.Get<BlueprintFeature>("46f4fb320f35704488ba3d513397789d"))); //medium armor proficiency
            swashbuckler_proficiencies.ReplaceComponent<AddFacts>(c => c.Facts = c.Facts.RemoveFromArray(library.Get<BlueprintFeature>("1b0f68188dcc435429fb87a022239681"))); //heavy armor proficiency
            swashbuckler_proficiencies.ReplaceComponent<AddFacts>(c => c.Facts = c.Facts.RemoveFromArray(library.Get<BlueprintFeature>("cb8686e7357a68c42bdd9d4e65334633"))); //shields proficiency
            swashbuckler_proficiencies.ReplaceComponent<AddFacts>(c => c.Facts = c.Facts.RemoveFromArray(library.Get<BlueprintFeature>("6105f450bb2acbd458d277e71e19d835"))); //tower shields proficiency
            swashbuckler_proficiencies.AddComponent(Common.createAddArmorProficiencies(ArmorProficiencyGroup.Buckler));
            swashbuckler_proficiencies.SetNameDescription("Swashbuckler Proficiencies",
                                                          "Swashbucklers are proficient with simple and martial weapons, as well as light armor and bucklers."
                                                          );
        }

        static void createPanachePoolAndDeeds()
        {
            panache_resource = CreateAbilityResource("PanacheResource", "Panache", "", "2087ab6ed0df4c8480379105bc0962a7", null);
            panache_resource.AddComponent(Create<MinResourceAmount>(m => m.value = 1));
            panache_resource.SetIncreasedByStat(0, StatType.Charisma);

            var regainPanache = Create<ContextRestoreResource>(c => { c.amount = 1; c.Resource = panache_resource; });

            panache = CreateFeature("PanacheFeature",
                                    "Panache",
                                    "More than just a lightly armored warrior, a swashbuckler is a daring combatant. She fights with panache: a fluctuating measure of a swashbuckler’s ability to perform amazing actions in combat. At the start of each day, a swashbuckler gains a number of panache points equal to her Charisma modifier (minimum 1). Her panache goes up or down throughout the day, but usually cannot go higher than her Charisma modifier (minimum 1), though feats and magic items can affect this maximum. A swashbuckler spends panache to accomplish deeds, and regains panache in the following ways."
                                    + "Critical Hit with a Light or One-Handed Piercing Melee Weapon: Each time the swashbuckler confirms a critical hit with a light or one-handed piercing melee weapon, she regains 1 panache point. Confirming a critical hit on a helpless or unaware creature or a creature that has fewer Hit Dice than half the swashbuckler’s character level doesn’t restore panache."
                                    + "Killing Blow with a Light or One-Handed Piercing Melee Weapon: When the swashbuckler reduces a creature to 0 or fewer hit points with a light or one - handed piercing melee weapon attack while in combat, she regains 1 panache point. Destroying an unattended object, reducing a helpless or unaware creature to 0 or fewer hit points, or reducing a creature that has fewer Hit Dice than half the swashbuckler’s character level to 0 or fewer hit points doesn’t restore any panache.",
                                    "2d4095f5959e458b918515b8a21e3a54",
                                    null, //TODO: icon
                                    FeatureGroup.None,
                                    CallOfTheWild.Helpers.CreateAddAbilityResource(panache_resource),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), critical_hit: true, weapon_category: WeaponCategory.Dagger),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), critical_hit: true, weapon_category: WeaponCategory.PunchingDagger),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), critical_hit: true, weapon_category: WeaponCategory.LightPick),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), critical_hit: true, weapon_category: WeaponCategory.Sai),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), critical_hit: true, weapon_category: WeaponCategory.Shortspear),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), critical_hit: true, weapon_category: WeaponCategory.Trident),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), critical_hit: true, weapon_category: WeaponCategory.HeavyPick),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), critical_hit: true, weapon_category: WeaponCategory.Rapier),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), critical_hit: true, weapon_category: WeaponCategory.Starknife),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), critical_hit: true, weapon_category: WeaponCategory.Estoc),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), critical_hit: true, weapon_category: WeaponCategory.Tongi),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), reduce_hp_to_zero: true, weapon_category: WeaponCategory.Dagger),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), reduce_hp_to_zero: true, weapon_category: WeaponCategory.PunchingDagger),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), reduce_hp_to_zero: true, weapon_category: WeaponCategory.LightPick),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), reduce_hp_to_zero: true, weapon_category: WeaponCategory.Sai),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), reduce_hp_to_zero: true, weapon_category: WeaponCategory.Shortspear),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), reduce_hp_to_zero: true, weapon_category: WeaponCategory.Trident),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), reduce_hp_to_zero: true, weapon_category: WeaponCategory.HeavyPick),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), reduce_hp_to_zero: true, weapon_category: WeaponCategory.Rapier),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), reduce_hp_to_zero: true, weapon_category: WeaponCategory.Starknife),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), reduce_hp_to_zero: true, weapon_category: WeaponCategory.Estoc),
                                    Common.createAddInitiatorAttackWithWeaponTriggerWithCategory(CreateActionList(regainPanache), reduce_hp_to_zero: true, weapon_category: WeaponCategory.Tongi)
                                    );

            //TODO: Deeds
        }

        static void createSwashbucklerFinesse()
        {
            var weapon_finesse = library.Get<BlueprintFeature>("90e54424d682d104ab36436bd527af09");

            swashbuckler_finesse = CreateFeature("SwashbucklerFinesseSwashbucklerFeature",
                                                 "Swashbuckler Finesse",
                                                 "At 1st level, a swashbuckler gains the benefits of the Weapon Finesse feat with light or one-handed piercing melee weapons, and she can use her Charisma score in place of Intelligence as a prerequisite for combat feats. This ability counts as having the Weapon Finesse feat for purposes of meeting feat prerequisites.",
                                                 "969cac9599204955b2b86996c7834ae1",
                                                 weapon_finesse.Icon,
                                                 FeatureGroup.None,
                                                 Create<AttackStatReplacementForWeaponCategory>(c =>
                                                                                                {
                                                                                                    c.categories = new WeaponCategory[] { WeaponCategory.Dagger, WeaponCategory.PunchingDagger, WeaponCategory.LightPick, WeaponCategory.Sai, WeaponCategory.Shortspear, WeaponCategory.Trident, WeaponCategory.HeavyPick, WeaponCategory.Rapier, WeaponCategory.Starknife, WeaponCategory.Estoc, WeaponCategory.Tongi };
                                                                                                    c.ReplacementStat = StatType.Dexterity;
                                                                                                }
                                                                                                ),
                                                 Create<ReplaceStatForPrerequisites>(r => { r.OldStat = StatType.Intelligence; r.NewStat = StatType.Charisma; r.Policy = ReplaceStatForPrerequisites.StatReplacementPolicy.NewStat; })
                                                 );
            library.Get<BlueprintFeature>("90e54424d682d104ab36436bd527af09").AddComponent(Create<FeatureReplacement>(f => f.replacement_feature = swashbuckler_finesse)); // weapon finesse
        }

        static void createCharmedLife()
        {
            var charmed_life_resource = CreateAbilityResource("SwashbucklerCharmedLifeResource", "", "", "cdaca00ebd144d8b870390fb6a09640f", null);
            charmed_life_resource.SetIncreasedByLevelStartPlusDivStep(3, 2, 0, 4, 1, 0, 0.0f, getSwashbucklerArray());

            var consume_resource = CreateActionList(Common.createContextActionSpendResource(charmed_life_resource, 1));

            var charmed_life_buff = CreateBuff("CharmedLifeSwashbucklerBuff",
                                               "Charmed Life",
                                               "At 2nd level, the swashbuckler gains a knack for getting out of trouble. Three times per day as an immediate action before attempting a saving throw, she can add her Charisma modifier to the result of the save. She must choose to do this before the roll is made. At 6th level and every 4 levels thereafter, the number of times she can do this per day increases by one (to a maximum of 7 times per day at 18th level).",
                                               "ab653c76fc2744d9b51b40b0cffc9f3d",
                                               null, //TODO Icon
                                               null,
                                               CreateAddContextStatBonus(StatType.SaveFortitude, ModifierDescriptor.UntypedStackable, rankType: AbilityRankType.StatBonus),
                                               CreateAddContextStatBonus(StatType.SaveReflex, ModifierDescriptor.UntypedStackable, rankType: AbilityRankType.StatBonus),
                                               CreateAddContextStatBonus(StatType.SaveWill, ModifierDescriptor.UntypedStackable, rankType: AbilityRankType.StatBonus),
                                               CreateContextRankConfig(baseValueType: ContextRankBaseValueType.StatBonus, stat: StatType.Charisma, min: 0, type: AbilityRankType.StatBonus),
                                               Create<CallOfTheWild.NewMechanics.AddInitiatorSavingThrowTrigger>(a => { a.Action = consume_resource; a.OnFail = true; a.OnPass = true; })
                                               );

            var charmed_life_ability = CreateActivatableAbility("CharmedLifeSwashbucklerToggleAbility",
                                                                charmed_life_buff.Name,
                                                                charmed_life_buff.Description,
                                                                "8f39d4572fa944cbbaad256c44bc5fcc",
                                                                charmed_life_buff.Icon,
                                                                charmed_life_buff,
                                                                AbilityActivationType.Immediately,
                                                                UnitCommand.CommandType.Swift,
                                                                null,
                                                                CallOfTheWild.Helpers.CreateActivatableResourceLogic(charmed_life_resource, ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                                                                );
            charmed_life_ability.DeactivateImmediately = true;

            charmed_life = CreateFeature("CharmedLifeSwashbucklerFeature",
                                         "Charmed Life",
                                         "At 2nd level, the swashbuckler gains a knack for getting out of trouble. Three times per day as an immediate action before attempting a saving throw, she can add her Charisma modifier to the result of the save. She must choose to do this before the roll is made. At 6th level and every 4 levels thereafter, the number of times she can do this per day increases by one (to a maximum of 7 times per day at 18th level).",
                                         "d96ad2e0ec4945d5b643119c1b173eaf",
                                         null, //TODO icon
                                         FeatureGroup.None,
                                         CallOfTheWild.Helpers.CreateAddAbilityResource(charmed_life_resource),
                                         CallOfTheWild.Helpers.CreateAddFact(charmed_life_ability)
                                         );

            charmed_life.HideInCharacterSheetAndLevelUp = true;
        }

        static void createNimble()
        {
            var nimble_bonus = CreateFeature("NimbleSwashbucklerFeature",
                                             "Nimble",
                                             "At 3rd level, a swashbuckler gains a +1 dodge bonus to AC while wearing light or no armor. Anything that causes the swashbuckler to lose her Dexterity bonus to AC also causes her to lose this dodge bonus. This bonus increases by 1 for every 4 levels beyond 3rd (to a maximum of +5 at 19th level).",
                                             "c9fc9521fe41460aa0a52c0a8405c811",
                                             null,
                                             FeatureGroup.None,
                                             CreateAddContextStatBonus(StatType.AC, ModifierDescriptor.Dodge),
                                             CreateContextRankConfig(baseValueType: ContextRankBaseValueType.ClassLevel, classes: getSwashbucklerArray(),
                                                                     progression: ContextRankProgression.DelayedStartPlusDivStep,
                                                                     startLevel: 3, stepLevel: 4)
                                             );

            nimble_bonus.HideInCharacterSheetAndLevelUp = true;

            nimble_unlock = CreateFeature(nimble_bonus.name + "Unlock",
                                          nimble_bonus.Name,
                                          nimble_bonus.Description,
                                          "221d8fee280b48eda3a871fe96c32eb1",
                                          nimble_bonus.Icon,
                                          FeatureGroup.None
                                          );

            nimble_unlock.AddComponent(Create<SwashbucklerNoArmorOrLightArmorNimbleFeatureUnlock>(c => c.NewFact = nimble_bonus));
            nimble_unlock.Ranks = 5;
            nimble_unlock.ReapplyOnLevelUp = true;
        }

        static void createSwashbucklerFighterFeatPrerequisiteReplacement()
        {
            var fighter_class = library.Get<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var fighter_training = library.Get<BlueprintFeature>("2b636b9e8dd7df94cbd372c52237eebf");
            swashbuckler_fighter_feat_prerequisite_replacement = CreateFeature("SwashbucklerFighterFeatPrerequisiteReplacement",
                                                                               "Fighter Training",
                                                                               "Swashbuckler levels are considered fighter levels for the purpose of meeting combat feat prerequisites.",
                                                                               "6769d198234a407a866d7c1345432d32",
                                                                               fighter_training.Icon,
                                                                               FeatureGroup.None,
                                                                               Common.createClassLevelsForPrerequisites(fighter_class, swashbuckler_class)
                                                                               );
        }

        static void createSwashbucklerWeaponTraining()
        {
            swashbuckler_weapon_training = CreateFeature("SwashbucklerWeaponTrainingSwashbucklerFeature",
                                                         "Swashbuckler Weapon Training",
                                                         "At 5th level, a swashbuckler gains a +1 bonus on attack and damage rolls with one-handed or light piercing melee weapons. While wielding such a weapon, she gains the benefit of the Improved Critical feat. These attack and damage bonuses increase by 1 for every 4 levels beyond 5th level (to a maximum of +4 at 17th level).",
                                                         "ac1b0c88a06346b4a0fe35465a74daff",
                                                         null, //TODO: Icon
                                                         FeatureGroup.None,
                                                         Create<WeaponTraining>(),
                                                         Create<WeaponTrainingBonuses>(w => { w.Stat = StatType.AdditionalAttackBonus; w.Descriptor = ModifierDescriptor.UntypedStackable; }),
                                                         Create<WeaponTrainingBonuses>(w => { w.Stat = StatType.AdditionalDamage; w.Descriptor = ModifierDescriptor.UntypedStackable; }),
                                                         Create<ImprovedCriticalOnWieldingSwashbucklerWeapon>()
                                                         );

            swashbuckler_weapon_training.Ranks = 4;
            swashbuckler_weapon_training.ReapplyOnLevelUp = true;
            swashbuckler_weapon_training.AddComponent(CreateContextRankConfig(ContextRankBaseValueType.FeatureRank, feature: swashbuckler_weapon_training));
        }

        static void createSwashbucklerWeaponMastery()
        {
            swashbuckler_weapon_mastery = CreateFeature("SwashbucklerWeaponMasterySwashbucklerFeature",
                                                        "Swashbuckler Weapon Mastery",
                                                        "At 20th level, when a swashbuckler threatens a critical hit with a light or one-handed piercing melee weapon, that critical is automatically confirmed. Furthermore, the critical modifiers of such weapons increase by 1 (×2 becomes ×3, and so on).",
                                                        "8878bda013664dc89339352271d005fc",
                                                        null, //TODO Icon
                                                        FeatureGroup.None,
                                                        Create<CritAutoconfirmWithSwashbucklerWeapons>(),
                                                        Create<IncreasedCriticalMultiplierWithSwashbucklerWeapon>()
                                                        );
        }

        //COMPONENTS AND HELPERS

        // TODO - DERVISH DANCE ETC EDGE CASES
        static bool isLightOrOneHandedPiercingWeapon(BlueprintItemWeapon weapon)
        {
            if (weapon.IsMelee && (weapon.DamageType.Physical.Form == PhysicalDamageForm.Piercing && (weapon.Type.IsLight || !weapon.Type.IsTwoHanded)))
            {
                return true;
            }
            return false;
        }

        [ComponentName("Add feature if owner has no armor or light armor")]
        [AllowedOn(typeof(BlueprintUnitFact))]
        [AllowMultipleComponents]
        public class SwashbucklerNoArmorOrLightArmorNimbleFeatureUnlock : OwnedGameLogicComponent<UnitDescriptor>, IUnitActiveEquipmentSetHandler, IUnitEquipmentHandler, IGlobalSubscriber
        {
            public BlueprintUnitFact NewFact;
            [JsonProperty]
            private Fact m_AppliedFact;

            public override void OnFactActivate()
            {
                this.CheckEligibility();
            }

            public override void OnFactDeactivate()
            {
                this.RemoveFact();
            }

            public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit)
            {
                this.CheckEligibility();
            }

            public void CheckEligibility()
            {
                if ((!this.Owner.Body.Armor.HasArmor || !this.Owner.Body.Armor.Armor.Blueprint.IsArmor) || (this.Owner.Body.Armor.Armor.Blueprint.ProficiencyGroup == ArmorProficiencyGroup.Light))
                {
                    this.AddFact();
                }
                else
                {
                    this.RemoveFact();
                }
            }

            public void AddFact()
            {
                if (this.m_AppliedFact != null)
                    return;
                this.m_AppliedFact = this.Owner.AddFact(this.NewFact, (MechanicsContext)null, (FeatureParam)null);
            }

            public void RemoveFact()
            {
                if (this.m_AppliedFact == null)
                    return;
                this.Owner.RemoveFact(this.m_AppliedFact);
                this.m_AppliedFact = (Fact)null;
            }

            public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem)
            {
                if (slot.Owner != this.Owner)
                    return;
                this.CheckEligibility();
            }

            public new void OnTurnOn()
            {
                this.CheckEligibility();
            }
        }

        [ComponentName("Add Improved Critical if owner is wielding a swashbuckler weapon")]
        [AllowedOn(typeof(BlueprintUnitFact))]
        public class ImprovedCriticalOnWieldingSwashbucklerWeapon : RuleInitiatorLogicComponent<RuleCalculateWeaponStats>
        {
            public override void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
            {
                if (evt.Weapon != null && isLightOrOneHandedPiercingWeapon(evt.Weapon.Blueprint))
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
                if (evt.Weapon != null && isLightOrOneHandedPiercingWeapon(evt.Weapon.Blueprint))
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
                if (isLightOrOneHandedPiercingWeapon(evt.Weapon.Blueprint))
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

                if (!isLightOrOneHandedPiercingWeapon(weapon.Blueprint))
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
}
