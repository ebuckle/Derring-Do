using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.PubSubSystem;
using Newtonsoft.Json;
using CallOfTheWild;
using static CallOfTheWild.Helpers;
using CallOfTheWild.ResourceMechanics;
using CallOfTheWild.NewMechanics;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using System.Linq;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.ElementsSystem;
using Kingmaker.Controllers.Combat;
using Kingmaker;
using Kingmaker.UnitLogic.ActivatableAbilities.Restrictions;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers;
using System.Collections.Generic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.View.Animation;
using CallOfTheWild.BleedMechanics;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.Controllers.Units;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Harmony12;
using Kingmaker.UnitLogic.Class.Kineticist.Properties;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Designers.EventConditionActionSystem.Events;

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
        static public ContextRestoreResource restore_panache;

        //TEST - DEEDS
        static public BlueprintAbilityResource panache_resource;

        static public BlueprintFeature deeds;

        //DONE
        static public BlueprintFeatureSelection fighter_feat;

        //DONE
        static public BlueprintFeature swashbuckler_fighter_feat_prerequisite_replacement;

        //TODO - TWEAK CHA SWAP TO JUST BE FOR COMBAT FEATS
        static public BlueprintFeature swashbuckler_finesse;

        //TODO
        static public BlueprintFeature charmed_life;

        //DONE - WORKING
        static public BlueprintFeature nimble_unlock;

        //DONE - WORKING
        static public BlueprintFeature swashbuckler_weapon_training;

        //DONE - WORKING
        static public BlueprintFeature swashbuckler_weapon_mastery;

        // TODO: Deeds
        static public BlueprintFeature CONSUME_PANACHE_DUMMY;

        //TODO: Test
        static public BlueprintFeature derring_do_deed;

        //TODO: Testing
        static public BlueprintFeature dodging_panache_deed;

        //TODO: Testing - 2nd attack roll?
        static public BlueprintFeature opportune_parry_and_riposte_deed;

        //TODO: Testing - refine animation
        static public BlueprintFeature kip_up_deed;

        //DONE
        static public BlueprintFeature menacing_swordplay_deed;

        //DONE working
        static public BlueprintFeature precise_strike_deed;

        //DONE WORKING
        static public BlueprintFeature swashbuckler_initiative_deed;

        //TODO - Figure out
        static public BlueprintFeature swashbucklers_grace_deed;

        static public BlueprintFeature superior_feint_deed;

        static public BlueprintFeature targeted_strike_deed;

        static public BlueprintFeature bleeding_wound_deed;

        static public BlueprintFeature evasive_deed;

        static public BlueprintFeature subtle_blade_deed;

        static public BlueprintFeature dizzying_defence_deed;

        static public BlueprintFeature perfect_thrust_deed;

        static public BlueprintFeature swashbucklers_edge_deed;

        static public BlueprintFeature cheat_death_deed;

        static public BlueprintFeature deadly_stab_deed;

        static public BlueprintFeature stunning_stab_deed;

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
            createDeeds();
            createSwashbucklerFinesse();
            createCharmedLife();
            createSwashbucklerFighterFeatPrerequisiteReplacement();
            fighter_feat = library.CopyAndAdd<BlueprintFeatureSelection>("41c8486641f7d6d4283ca9dae4147a9f", "SwashbucklerBonusFeat", "043bee82f2dc47039e59d40d826e4bf8");
            fighter_feat.SetDescription("At 4th level and every 4 levels thereafter, a swashbuckler gains a bonus feat in addition to those gained from normal advancement. These bonus feats must be selected from those listed as combat feats.");
            createSwashbucklerWeaponTraining();
            createSwashbucklerWeaponMastery();
            // TODO: create methods for Swashbuckler abilities




            //DEBUG ONLY
            createDummyConsumePanache();

            //DEEDS
            
            createDerringDoDeed();
            createDodgingPanacheDeed();
            createOpportuneParryAndRiposte();
            createKipUpDeed();
            createMenacingSwordplayDeed();
            createPreciseStrikeDeed();
            createSwashbucklerInitiativeDeed();
            createSwashbucklersGraceDeed();
            createSuperiorFeintDeed();
            createTargetedStrikeDeed();
            createBleedingWoundDeed();
            createEvasiveDeed();
            createSubtleBlade();
            createDizzyingDefence();
            createPerfectThrust();
            createSwashbucklersEdge();
            createCheatDeath();

            swashbuckler_progression = CreateProgression("SwashbucklerProgression",
                                                           swashbuckler_class.Name,
                                                           swashbuckler_class.Description,
                                                           "4fc04fcd05cb4a07a450a2fb96a6cc11",
                                                           swashbuckler_class.Icon,
                                                           FeatureGroup.None);
            swashbuckler_progression.Classes = getSwashbucklerArray();

            swashbuckler_progression.LevelEntries = new LevelEntry[] { LevelEntry(1, swashbuckler_proficiencies, swashbuckler_finesse, panache, deeds, derring_do_deed, dodging_panache_deed, opportune_parry_and_riposte_deed, CONSUME_PANACHE_DUMMY),
                                                                       LevelEntry(2, charmed_life),
                                                                       LevelEntry(3, nimble_unlock, kip_up_deed, menacing_swordplay_deed, precise_strike_deed, swashbuckler_initiative_deed),
                                                                       LevelEntry(4, fighter_feat, swashbuckler_fighter_feat_prerequisite_replacement),
                                                                       LevelEntry(5, swashbuckler_weapon_training),
                                                                       LevelEntry(6, charmed_life),
                                                                       LevelEntry(7, nimble_unlock, swashbucklers_grace_deed, superior_feint_deed, targeted_strike_deed),
                                                                       LevelEntry(8, fighter_feat),
                                                                       LevelEntry(9, swashbuckler_weapon_training),
                                                                       LevelEntry(10, charmed_life),
                                                                       LevelEntry(11, nimble_unlock, bleeding_wound_deed, evasive_deed, subtle_blade_deed),
                                                                       LevelEntry(12, fighter_feat),
                                                                       LevelEntry(13, swashbuckler_weapon_training),
                                                                       LevelEntry(14, charmed_life),
                                                                       LevelEntry(15, nimble_unlock, dizzying_defence_deed, perfect_thrust_deed, swashbucklers_edge_deed),
                                                                       LevelEntry(16, fighter_feat),
                                                                       LevelEntry(17, swashbuckler_weapon_training),
                                                                       LevelEntry(18, charmed_life),
                                                                       LevelEntry(19, nimble_unlock, cheat_death_deed),
                                                                       LevelEntry(20, fighter_feat, swashbuckler_weapon_mastery),
                                                                       };
            swashbuckler_progression.UIDeterminatorsGroup = new BlueprintFeatureBase[] { swashbuckler_proficiencies, swashbuckler_finesse, panache, deeds };
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

        //TODO rename
        static void createPanachePoolAndDeeds()
        {
            panache_resource = CreateAbilityResource("PanacheResource", "Panache", "", "2087ab6ed0df4c8480379105bc0962a7", null);
            panache_resource.AddComponent(Create<MinResourceAmount>(m => m.value = 1));
            panache_resource.SetIncreasedByStat(0, StatType.Charisma);

            restore_panache = Create<ContextRestoreResource>(c => { c.amount = 1; c.Resource = panache_resource; });

            panache = CreateFeature("PanacheFeature",
                                    "Panache",
                                    "More than just a lightly armored warrior, a swashbuckler is a daring combatant. She fights with panache: a fluctuating measure of a swashbuckler’s ability to perform amazing actions in combat. At the start of each day, a swashbuckler gains a number of panache points equal to her Charisma modifier (minimum 1). Her panache goes up or down throughout the day, but usually cannot go higher than her Charisma modifier (minimum 1), though feats and magic items can affect this maximum. A swashbuckler spends panache to accomplish deeds, and regains panache in the following ways."
                                    + "Critical Hit with a Light or One-Handed Piercing Melee Weapon: Each time the swashbuckler confirms a critical hit with a light or one-handed piercing melee weapon, she regains 1 panache point. Confirming a critical hit on a helpless or unaware creature or a creature that has fewer Hit Dice than half the swashbuckler’s character level doesn’t restore panache."
                                    + "Killing Blow with a Light or One-Handed Piercing Melee Weapon: When the swashbuckler reduces a creature to 0 or fewer hit points with a light or one - handed piercing melee weapon attack while in combat, she regains 1 panache point. Destroying an unattended object, reducing a helpless or unaware creature to 0 or fewer hit points, or reducing a creature that has fewer Hit Dice than half the swashbuckler’s character level to 0 or fewer hit points doesn’t restore any panache.",
                                    "2d4095f5959e458b918515b8a21e3a54",
                                    null, //TODO: icon
                                    FeatureGroup.None,
                                    panache_resource.CreateAddAbilityResource()
                                    //TODO - cleanup to use boolean method instead
                                    /*
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
                                    */
                                    );

            //TODO: Deeds
        }

        static void createDeeds()
        {
            deeds = CreateFeature("DeedsSwashbucklerFeature",
                                  "Deeds",
                                  "Swashbucklers spend panache points to accomplish deeds. Most deeds grant the swashbuckler a momentary bonus or effect, but some provide longer-lasting effects. Some deeds remain in effect while the swashbuckler has at least 1 panache point, but do not require expending panache to be maintained. A swashbuckler can only perform deeds of her level or lower. Unless otherwise noted, a deed can be performed multiple successive times, as long as the swashbuckler has or spends the required number of panache points to perform the deed.",
                                  "df3a37fa67524ed097e070be2ed8f706",
                                  null, //TODO Icon
                                  FeatureGroup.None
                                  );
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

            var consume_resource = Common.createContextActionSpendResource(charmed_life_resource, 1);

            var restore_resource = Create<ContextRestoreResource>(c => { c.amount = 1; c.Resource = charmed_life_resource; });

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
                                               Create<CallOfTheWild.NewMechanics.AddInitiatorSavingThrowTrigger>(a => { a.Action = Helpers.CreateActionList(consume_resource, CallOfTheWild.Helpers.Create<ContextActionRemoveSelf>()); a.OnFail = true; a.OnPass = true; })
                                               );

            var apply_buff = Common.createContextActionApplyBuff(charmed_life_buff, CreateContextDuration(1), dispellable: false);

            var charmed_life_ability = CreateAbility("CharmedLifeSwashbucklerAbility",
                                                     charmed_life_buff.Name,
                                                     charmed_life_buff.Description,
                                                     "8f39d4572fa944cbbaad256c44bc5fcc",
                                                     charmed_life_buff.Icon,
                                                     AbilityType.Extraordinary,
                                                     CommandType.Swift,
                                                     AbilityRange.Personal,
                                                     oneRoundDuration,
                                                     "",
                                                     CreateRunActions(new GameAction[] { apply_buff, restore_resource }),
                                                     Create<AbilityResourceLogic>(a => { a.IsSpendResource = true; a.Amount = 1; a.CostIsCustom = false; a.RequiredResource = charmed_life_resource; })
                                                     );
            charmed_life_ability.setMiscAbilityParametersSelfOnly();

            charmed_life = CreateFeature("CharmedLifeSwashbucklerFeature",
                                         "Charmed Life",
                                         "At 2nd level, the swashbuckler gains a knack for getting out of trouble. Three times per day as an immediate action before attempting a saving throw, she can add her Charisma modifier to the result of the save. She must choose to do this before the roll is made. At 6th level and every 4 levels thereafter, the number of times she can do this per day increases by one (to a maximum of 7 times per day at 18th level).",
                                         "d96ad2e0ec4945d5b643119c1b173eaf",
                                         null, //TODO icon
                                         FeatureGroup.None,
                                         CallOfTheWild.Helpers.CreateAddAbilityResource(charmed_life_resource),
                                         CallOfTheWild.Helpers.CreateAddFact(charmed_life_ability)
                                         );
            charmed_life.ReapplyOnLevelUp = true;
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

        //DEEDS
        static void createDerringDoDeed()
        {
            var derring_do_buff = CreateBuff("DerringDoSwashbucklerBuff",
                                             "Derring-Do",
                                             "At 1st level, a swashbuckler can spend 1 panache point when she makes an Athletics or Mobility check to roll 1d6 and add the result to the check. She can do this after she makes the check but before the result is revealed. If the result of the d6 roll is a natural 6, she rolls another 1d6 and adds it to the check. She can continue to do this as long as she rolls natural 6s, up to a number of times equal to her Dexterity modifier (minimum 1).",
                                             "1ec72614c5fe40deba1fd3553124389b",
                                             null, // TODO - icon
                                             null,
                                             Create<AddExplodingD6sToDerringDoSkillChecks>()
                                             );

            var derring_do_ability = CreateActivatableAbility("DerringDoSwashbucklerToggleAbility",
                                                              derring_do_buff.Name,
                                                              derring_do_buff.Description,
                                                              "82ead78f2a1d4d2488a618aeef891c84",
                                                              derring_do_buff.Icon,
                                                              derring_do_buff,
                                                              AbilityActivationType.Immediately,
                                                              UnitCommand.CommandType.Free,
                                                              null,
                                                              CallOfTheWild.Helpers.CreateActivatableResourceLogic(panache_resource, ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                                                              );
            derring_do_ability.DeactivateImmediately = true;

            derring_do_deed = CreateFeature("DerringDoSwashbucklerFeature",
                                            derring_do_buff.Name,
                                            derring_do_buff.Description,
                                            "9633db59939c4a73bbd780809c6c9c98",
                                            derring_do_buff.Icon,
                                            FeatureGroup.None,
                                            CallOfTheWild.Helpers.CreateAddAbilityResource(panache_resource),
                                            CallOfTheWild.Helpers.CreateAddFact(derring_do_ability)
                                            );
        }

        static void createDodgingPanacheDeed()
        {
            var dodging_panache_buff = CreateBuff("DodgingPanacheSwashbucklerBuff",
                                                  "Dodging Panache",
                                                  "At 1st level, when an opponent attempts a melee attack against the swashbuckler, the swashbuckler can as an immediate action spend 1 panache point to gain a dodge bonus to AC equal to her Charisma modifier (minimum 0) against the triggering attack. The swashbuckler can only perform this deed while wearing light or no armor.",
                                                  "c466576d4a784d3f94300e08afa57784",
                                                  null, //TODO - icon
                                                  null, //TODO - animation
                                                  Create<AddACBonusOnAttackAndConsumePanache>()
                                                  );
            var apply_buff = Common.createContextActionApplyBuff(dodging_panache_buff, CreateContextDuration(1), dispellable: false);

            var dodging_panache_ability = CreateAbility("DodgingPanacheSwashbucklerAbility",
                                                        dodging_panache_buff.Name,
                                                        dodging_panache_buff.Description,
                                                        "59d6029a297c4798b0d2c44b68b983a6",
                                                        dodging_panache_buff.Icon,
                                                        AbilityType.Extraordinary,
                                                        CommandType.Swift,
                                                        AbilityRange.Personal,
                                                        oneRoundDuration,
                                                        "",
                                                        CreateRunActions(new GameAction[] { apply_buff, restore_panache }),
                                                        Create<AbilityCasterLightOrNoArmorCheck>(),
                                                        Create<AbilityResourceLogic>(a => { a.IsSpendResource = true; a.Amount = 1; a.CostIsCustom = false; a.RequiredResource = panache_resource; })
                                                        );
            dodging_panache_ability.setMiscAbilityParametersSelfOnly();

            dodging_panache_deed = CreateFeature("DodgingPanacheSwashbucklerFeature",
                                                 dodging_panache_buff.Name,
                                                 dodging_panache_buff.Description,
                                                 "316d0a03c3fc4b539edadbaa2089c747",
                                                 dodging_panache_buff.Icon,
                                                 FeatureGroup.None,
                                                 CallOfTheWild.Helpers.CreateAddAbilityResource(panache_resource),
                                                 CallOfTheWild.Helpers.CreateAddFact(dodging_panache_ability)
                                                 );
        }

        static void createOpportuneParryAndRiposte()
        {
            var opportune_parry_and_riposte_buff = CreateBuff("OpportuneParryAndRiposteSwashbucklerBuff",
                                                              "Opportune Parry and Riposte",
                                                              "At 1st level, when an opponent makes a melee attack against the swashbuckler, she can spend 1 panache point and expend a use of an attack of opportunity to attempt to parry that attack. The swashbuckler makes an attack roll as if she were making an attack of opportunity; for each size category the attacking creature is larger than the swashbuckler, the swashbuckler takes a –2 penalty on this roll. If her result is greater than the attacking creature’s result, the creature’s attack automatically misses. The swashbuckler must declare the use of this ability after the creature’s attack is announced, but before its attack roll is made. Upon performing a successful parry and if she has at least 1 panache point, the swashbuckler can as an immediate action make an attack against the creature whose attack she parried, provided that creature is within her reach. This deed’s cost cannot be reduced by any ability or effect that reduces the number of panache points a deed costs.",
                                                              "a48086ca55a7472284780720a79deb03",
                                                              null, //TODO icon
                                                              null, //TODO fx
                                                              Create<SwashbucklerParryAndRiposte>(s => s.AttackerCondition = null)
                                                              );

            var apply_buff = Common.createContextActionApplyBuff(opportune_parry_and_riposte_buff, CreateContextDuration(1), dispellable: false);

            var opportune_parry_and_riposte_ability = CreateAbility("OpportuneParryAndRiposteSwashbucklerAbility",
                                                                    opportune_parry_and_riposte_buff.Name,
                                                                    opportune_parry_and_riposte_buff.Description,
                                                                    "5b048c5e44904f5382d968d0d2f561d2",
                                                                    opportune_parry_and_riposte_buff.Icon, //TODO Icon
                                                                    AbilityType.Extraordinary,
                                                                    CommandType.Swift,
                                                                    AbilityRange.Personal,
                                                                    oneRoundDuration,
                                                                    "",
                                                                    CreateRunActions(new GameAction[] { apply_buff, restore_panache }),
                                                                    Create<AbilityResourceLogic>(a => { a.IsSpendResource = true; a.Amount = 1; a.CostIsCustom = false; a.RequiredResource = panache_resource; })
                                                                    );
            opportune_parry_and_riposte_ability.setMiscAbilityParametersSelfOnly();

            opportune_parry_and_riposte_deed = CreateFeature("OpportuneParryAndRiposteSwashbucklerFeature",
                                                             opportune_parry_and_riposte_buff.Name,
                                                             opportune_parry_and_riposte_buff.Description,
                                                             "8b609cbb9b754b0da070af3933cd2f69",
                                                             opportune_parry_and_riposte_buff.Icon,
                                                             FeatureGroup.None,
                                                             CallOfTheWild.Helpers.CreateAddFact(opportune_parry_and_riposte_ability)
                                                             );
        }

        static void createKipUpDeed()
        {
            var kip_up_buff = CreateBuff("KipUpSwashbucklerBuff",
                                         "Kip-Up",
                                         "At 3rd level, while the swashbuckler has at least 1 panache point, she can kip-up from prone as a move action without provoking an attack of opportunity. She can kip-up as a swift action instead by spending 1 panache point.",
                                         "e9e1fe13e22241938599e713869e343e",
                                         null, //TODO icon
                                         null, //TODO fx
                                         AddMechanicsFeature.MechanicsFeatureType.GetUpWithoutAttackOfOpportunity.CreateAddMechanics()
                                         );

            var kip_up_toggle_ability = CreateActivatableAbility("KipUpActivatableSwashbucklerAbility",
                                                                 kip_up_buff.Name,
                                                                 kip_up_buff.Description,
                                                                 "a28cb94820194c94b6d6e28199a4aebe",
                                                                 null,
                                                                 kip_up_buff,
                                                                 AbilityActivationType.Immediately,
                                                                 CommandType.Free,
                                                                 null,
                                                                 CallOfTheWild.Helpers.CreateActivatableResourceLogic(panache_resource, ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                                                                 );
            kip_up_toggle_ability.IsOnByDefault = true;
            kip_up_toggle_ability.DeactivateIfOwnerDisabled = false;
            kip_up_toggle_ability.DeactivateIfCombatEnded = false;
            kip_up_toggle_ability.DeactivateIfOwnerUnconscious = false;

            var kip_up_swift_activatable_ability = CreateActivatableAbility("KipUpSwiftActivatableSwashbucklerAbility",
                                                                            kip_up_buff.Name + " (Swift Action)",
                                                                            kip_up_buff.Description,
                                                                            "0483e5e0c19d4fd59a1cb381adebc619",
                                                                            null, //TODO icon
                                                                            null,
                                                                            AbilityActivationType.Immediately,
                                                                            CommandType.Swift,
                                                                            null,
                                                                            Create<RestrictionHasUnitProne>(),
                                                                            CallOfTheWild.Helpers.CreateActivatableResourceLogic(panache_resource, ActivatableAbilityResourceLogic.ResourceSpendType.TurnOn),
                                                                            CreateAddFactContextActions(CallOfTheWild.Helpers.Create<GetUpFromProne>())
                                                                            );

            kip_up_deed = CreateFeature("KipUpSwashbucklerFeature",
                                        kip_up_buff.Name,
                                        kip_up_buff.Description,
                                        "af26a97f3bf9470aa08bbe78967184f1",
                                        null, //TODO icon
                                        FeatureGroup.None,
                                        CallOfTheWild.Helpers.CreateAddFact(kip_up_toggle_ability),
                                        CallOfTheWild.Helpers.CreateAddFact(kip_up_swift_activatable_ability)
                                        );
        }

        static void createMenacingSwordplayDeed()
        {
            var cornugon_smash = library.Get<BlueprintFeature>("ceea53555d83f2547ae5fc47e0399e14");
            var demoralize_action = ((Conditional)cornugon_smash.GetComponent<AddInitiatorAttackWithWeaponTrigger>().Action.Actions[0]).IfTrue.Actions[0];

            var menacing_swordplay_buff = CreateBuff("MenacingSwordplaySwashbucklerBuff",
                                                     "Menacing Swordplay",
                                                     "At 3rd level, while she has at least 1 panache point, when a swashbuckler hits an opponent with a light or one-handed piercing melee weapon, she can choose to use Intimidate to demoralize that opponent as a swift action instead of a standard action.",
                                                     "0a1b5e625521446d9e4f2d0f30eb758a",
                                                     cornugon_smash.Icon,
                                                     null,
                                                     Create<IndimidateOnHitWithSwashbucklerWeapon>(i => i.demoralize_action = demoralize_action)
                                                     );

            var menacing_swordplay_toggle_ability = CreateActivatableAbility("MenacingSwordplaySwashbucklerActivatableAbility",
                                                                             menacing_swordplay_buff.Name,
                                                                             menacing_swordplay_buff.Description,
                                                                             "478130ab2ed84753876a8fda836c7813",
                                                                             menacing_swordplay_buff.Icon,
                                                                             menacing_swordplay_buff,
                                                                             AbilityActivationType.Immediately,
                                                                             CommandType.Free,
                                                                             null,
                                                                             CallOfTheWild.Helpers.CreateActivatableResourceLogic(panache_resource, ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                                                                             );
            menacing_swordplay_toggle_ability.IsOnByDefault = true;
            menacing_swordplay_toggle_ability.DeactivateIfOwnerDisabled = false;
            menacing_swordplay_toggle_ability.DeactivateIfCombatEnded = false;
            menacing_swordplay_toggle_ability.DeactivateIfOwnerUnconscious = false;

            menacing_swordplay_deed = CreateFeature("MenacingSwordplaySwashbucklerFeature",
                                                    menacing_swordplay_buff.Name,
                                                    menacing_swordplay_buff.Description,
                                                    "511b63199c564db3b9d51811430e804a",
                                                    menacing_swordplay_buff.Icon,
                                                    FeatureGroup.None,
                                                    CallOfTheWild.Helpers.CreateAddFact(menacing_swordplay_toggle_ability)
                                                    );
        }

        static void createPreciseStrikeDeed()
        {
            var precise_strike_buff = CreateBuff("PreciseStrikeSwashbucklerBuff",
                                                 "Precise Strike",
                                                 "At 3rd level, while she has at least 1 panache point, a swashbuckler gains the ability to strike precisely with a light or one-handed piercing melee weapon (though not natural weapon attacks), adding her swashbuckler level to the damage dealt. To use this deed, a swashbuckler cannot attack with a weapon in her other hand or use a shield other than a buckler. Any creature that is immune to sneak attacks is immune to the additional damage granted by precise strike, and any item or ability that protects a creature from critical hits also protects a creature from the additional damage of a precise strike. This additional damage is precision damage, and isn’t multiplied on a critical hit. As a swift action, a swashbuckler can spend 1 panache point to double her precise strike’s damage bonus on the next attack. This benefit must be used before the end of her turn, or it is lost. This deed’s cost cannot be reduced by any ability or effect that reduces the amount of panache points a deed costs (such as the Signature Deed feat).",
                                                 "8fa7914d1d734478b9f863e7a514427e",
                                                 null, //TODO icon
                                                 null, //TODO fx
                                                 Create<AddBonusPrecisionDamageToSwashbucklerWeapons>(a => a.is_passive = false)
                                                 );

            var apply_buff = Common.createContextActionApplyBuff(precise_strike_buff, Helpers.CreateContextDuration(), dispellable: false, duration_seconds: 4);

            var precise_strike_ability = CreateAbility("PreciseStrikeSwashbucklerAbility",
                                                       precise_strike_buff.Name,
                                                       precise_strike_buff.Description,
                                                       "349bd539fb6741c1b32a3b6164362559",
                                                       precise_strike_buff.Icon,
                                                       AbilityType.Extraordinary,
                                                       CommandType.Swift,
                                                       AbilityRange.Personal,
                                                       "",
                                                       "",
                                                       CreateRunActions(new GameAction[] { apply_buff }),
                                                       Create<AbilityResourceLogic>(a => { a.IsSpendResource = true; a.Amount = 1; a.CostIsCustom = false; a.RequiredResource = panache_resource; })
                                                       );

            precise_strike_deed = CreateFeature("PreciseStrikeSwashbucklerFeature",
                                                precise_strike_buff.Name,
                                                precise_strike_buff.Description,
                                                "03b1034003bc4d8083e03c83604484ab",
                                                precise_strike_buff.Icon,
                                                FeatureGroup.None,
                                                CallOfTheWild.Helpers.CreateAddFact(precise_strike_ability),
                                                Create<AddBonusPrecisionDamageToSwashbucklerWeapons>(a => a.is_passive = true)
                                                );
        }

        static void createSwashbucklerInitiativeDeed()
        {
            swashbuckler_initiative_deed = CreateFeature("SwashbucklerInitiativeDeedSwashbucklerFeature",
                                                         "Swashbuckler Initiative",
                                                         "At 3rd level, while the swashbuckler has at least 1 panache point, she gains a +2 bonus on initiative checks.",
                                                         "7afa78acfbe142ff99125c2a2e253362",
                                                         null, //TODO icon
                                                         FeatureGroup.None
                                                         );

            swashbuckler_initiative_deed.AddComponent(Create<AddStaticBonusOnInitiativeCheckIfResourceAvailable>(a => { a.amount = 1; a.bonus = 2; a.resource = panache_resource; }));
        }

        static void createSwashbucklersGraceDeed()
        {
            swashbucklers_grace_deed = CreateFeature("SwashbucklersGraceSwashbucklerFeature",
                                         "Swashbuckler's Grace",
                                         "At 7th level, while the swashbuckler has at least 1 panache point, she takes no penalty for moving at full speed when she uses Acrobatics to attempt to move through a threatened area or an enemy’s space.",
                                         "dc10b82b73684935a25891aabaee5cf2",
                                         null, //TODO icon
                                         FeatureGroup.None
                                         );
            
            var mobility_buff = library.Get<BlueprintBuff>("9dc2afb96879cfd4bb7aed475ed51002");
            mobility_buff.RemoveComponents<AddCondition>();
            mobility_buff.AddComponent(Create<AddCondition>(a => a.Condition = UnitCondition.UseMobilityToNegateAttackOfOpportunity));
        }

        static void createSuperiorFeintDeed()
        {
            var superior_feint_debuff = CreateBuff("SuperiorFeintEnemyDebuff",
                                                   "Superior Feint",
                                                   "Target is is denied its Dexterity bonus to AC.",
                                                   "9bfd81378c0c4a798115fda7f36e43f2",
                                                   null, //TODO icon
                                                   null, //TODO fx
                                                   Common.createAddCondition(UnitCondition.LoseDexterityToAC)
                                                   );

            var apply_buff = Common.createContextActionApplyBuff(superior_feint_debuff, Helpers.CreateContextDuration(1), dispellable: false);

            var superior_feint_ability = CreateAbility("SuperiorFeintSwashbucklerAbility",
                                                       "Superior Feint",
                                                       "At 7th level, a swashbuckler with at least 1 panache point can, as a standard action, purposefully miss a creature she could make a melee attack against with a wielded light or one-handed piercing weapon. When she does, the creature is denied its Dexterity bonus to AC until the start of the swashbuckler’s next turn.",
                                                       "ed17ecaa24934fb68c7182d05ded73ad",
                                                       null, //TODO icon
                                                       AbilityType.Extraordinary,
                                                       CommandType.Standard,
                                                       AbilityRange.Weapon,
                                                       Helpers.oneRoundDuration,
                                                       "",
                                                       Helpers.CreateRunActions(apply_buff),
                                                       Create<AbilityCasterSwashbucklerWeaponCheck>(),
                                                       Create<AbilityCasterHasAtLeastOnePanache>(),
                                                       Helpers.Create<AttackAnimation>()
                                                       );
            superior_feint_ability.setMiscAbilityParametersTouchHarmful(works_on_allies: false);

            superior_feint_deed = CreateFeature("SuperiorFeintSwashbucklerFeature",
                                                superior_feint_debuff.Name,
                                                "At 7th level, a swashbuckler with at least 1 panache point can, as a standard action, purposefully miss a creature she could make a melee attack against with a wielded light or one-handed piercing weapon. When she does, the creature is denied its Dexterity bonus to AC until the start of the swashbuckler’s next turn.",
                                                "5f24450e21af46598dd7409f42f4edc5",
                                                superior_feint_debuff.Icon,
                                                FeatureGroup.None,
                                                Helpers.CreateAddFact(superior_feint_ability)
                                                );
        }

        static void createTargetedStrikeDeed()
        {
            var disarm = library.Get<BlueprintAbility>("45d94c6db453cfc4a9b99b72d6afe6f6"); //Disarm ability
            var immune_to_crits = library.Get<BlueprintFeature>("ced0f4e5d02d5914a9f9ff74acacf26d"); //Immunity to crits feature
            var immune_to_mind_affecting = library.Get<BlueprintFeature>("3eb606c0564d0814ea01a824dbe42fb0"); //Immunity to mind affecting
            var immune_to_trip = library.Get<BlueprintFeature>("c1b26f97b974aec469613f968439e7bb");
            var immune_to_prone = library.Get<BlueprintBuff>("7e3cd4e16a990ab4e9ffa5d9ca3c4870");
            var confusion_buff = library.Get<BlueprintBuff>("886c7407dc629dc499b9f1465ff382df");
            var staggered_buff = library.Get<BlueprintBuff>("df3950af5a783bd4d91ab73eb8fa0fd3");

            var base_name = "Targeted Strike";

            var targeted_strike_arms_ability = CreateAbility("TargetedStrikeArmsSwashbucklerAbility",
                                                             base_name + " - Arms",
                                                             "The target takes no damage from the attack, but it is disarmed.",
                                                             "80bbdc295bd649bea280c7ed23ad5c37",
                                                             disarm.Icon, //TODO icon
                                                             AbilityType.Extraordinary,
                                                             CommandType.Standard,
                                                             AbilityRange.Weapon,
                                                             "",
                                                             "",
                                                             Create<AbilityCasterSwashbucklerWeaponCheck>(),
                                                             Create<AbilityTargetHasNoFactUnless>(a => { a.CheckedFacts = new Kingmaker.Blueprints.Facts.BlueprintUnitFact[] { immune_to_crits }; a.UnlessFact = null; }),
                                                             Create<AbilityTargetNotImmuneToPrecision>(),
                                                             Helpers.Create<AbilityDeliverHitWithMeleeWeapon>(),
                                                             Helpers.CreateRunActions(Create<DisarmTarget>()),
                                                             Helpers.Create<AttackAnimation>(),
                                                             Create<AbilityResourceLogic>(a => { a.IsSpendResource = true; a.Amount = 1; a.CostIsCustom = false; a.RequiredResource = panache_resource; })
                                                             );
            Common.setAsFullRoundAction(targeted_strike_arms_ability);
            targeted_strike_arms_ability.setMiscAbilityParametersTouchHarmful(works_on_allies: false);

            var targeted_strike_head_ability = CreateAbility("TargetedStrikeHeadSwashbucklerAbility",
                                                             base_name + " - Head",
                                                             "The target is confused for 1 round. This is a mind-affecting effect.",
                                                             "075780d93e7f4d35beca760afe16764b",
                                                             confusion_buff.Icon, //TODO icon
                                                             AbilityType.Extraordinary,
                                                             CommandType.Standard,
                                                             AbilityRange.Weapon,
                                                             "",
                                                             "",
                                                             Create<AbilityCasterSwashbucklerWeaponCheck>(),
                                                             Create<AbilityTargetHasNoFactUnless>(a => { a.CheckedFacts = new BlueprintUnitFact[] { immune_to_crits, immune_to_mind_affecting }; a.UnlessFact = null; }),
                                                             Create<AbilityTargetNotImmuneToPrecision>(),
                                                             Create<AbilityDeliverAttackWithWeaponOnHit>(),
                                                             Create<AbilityResourceLogic>(a => { a.IsSpendResource = true; a.Amount = 1; a.CostIsCustom = false; a.RequiredResource = panache_resource; }),
                                                             CreateRunActions(Common.createContextActionApplyBuff(confusion_buff, Helpers.CreateContextDuration(1), dispellable: false))
                                                             );
            Common.setAsFullRoundAction(targeted_strike_head_ability);
            targeted_strike_head_ability.setMiscAbilityParametersTouchHarmful(animation: Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate, animation_style: CastAnimationStyle.CastActionPoint, works_on_allies: false);

            var targeted_strike_legs_ability = CreateAbility("TargetedStrikesLegsSwashbucklerAbility",
                                                             base_name + " - Legs",
                                                             "The target is knocked prone. Creatures that are immune to trip attacks are immune to this effect.",
                                                             "00be497d03e34c26a1954f76cd3c6a48",
                                                             null, //TODO icon
                                                             AbilityType.Extraordinary,
                                                             CommandType.Standard,
                                                             AbilityRange.Weapon,
                                                             "",
                                                             "",
                                                             Create<AbilityCasterSwashbucklerWeaponCheck>(),
                                                             Create<AbilityTargetHasNoFactUnless>(a => { a.CheckedFacts = new BlueprintUnitFact[] { immune_to_crits, immune_to_prone }; a.UnlessFact = null; }),
                                                             Create<AbilityTargetNotImmuneToPrecision>(),
                                                             Create<AbilityDeliverAttackWithWeaponOnHit>(),
                                                             Create<AbilityResourceLogic>(a => { a.IsSpendResource = true; a.Amount = 1; a.CostIsCustom = false; a.RequiredResource = panache_resource; }),
                                                             CreateRunActions(Helpers.Create<ContextActionKnockdownTarget>())
                                                             );
            Common.setAsFullRoundAction(targeted_strike_legs_ability);
            targeted_strike_legs_ability.setMiscAbilityParametersTouchHarmful(animation: Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate, animation_style: CastAnimationStyle.CastActionPoint, works_on_allies: false);

            var targeted_strike_torso_ability = CreateAbility("TargetedStrikeTorsoSwashbucklerAbility",
                                                             base_name + " - Torso",
                                                             "The target is staggered for 1 round.",
                                                             "a5206e32a38a4cb69f1e414abe5cb491",
                                                             null, //TODO icon
                                                             AbilityType.Extraordinary,
                                                             CommandType.Standard,
                                                             AbilityRange.Weapon,
                                                             "",
                                                             "",
                                                             Create<AbilityCasterSwashbucklerWeaponCheck>(),
                                                             Create<AbilityTargetHasNoFactUnless>(a => { a.CheckedFacts = new BlueprintUnitFact[] { immune_to_crits }; a.UnlessFact = null; }),
                                                             Create<AbilityTargetNotImmuneToPrecision>(),
                                                             Create<AbilityDeliverAttackWithWeaponOnHit>(),
                                                             Create<AbilityResourceLogic>(a => { a.IsSpendResource = true; a.Amount = 1; a.CostIsCustom = false; a.RequiredResource = panache_resource; }),
                                                             CreateRunActions(Common.createContextActionApplyBuff(staggered_buff, Helpers.CreateContextDuration(1), dispellable: false))
                                                             );
            Common.setAsFullRoundAction(targeted_strike_torso_ability);
            targeted_strike_torso_ability.setMiscAbilityParametersTouchHarmful(animation: Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate, animation_style: CastAnimationStyle.CastActionPoint, works_on_allies: false);

            var wrapper = Common.createVariantWrapper("SwashbucklerTargetedStrikeAbility", "a7322f3c039547aa84ff4e4b6d817433", targeted_strike_arms_ability, targeted_strike_head_ability, targeted_strike_legs_ability, targeted_strike_torso_ability);
            wrapper.SetName("Targeted Strike");
            wrapper.SetDescription("At 7th level, as a full-round action the swashbuckler can spend 1 panache point to make an attack with a single light or one-handed piercing melee weapon that cripples part of a foe’s body. The swashbuckler chooses a part of the body to target. If the attack succeeds, in addition to the attack’s normal damage, the target suffers one of the following effects based on the part of the body targeted. If a creature doesn’t have one of the listed body locations, that body part cannot be targeted. Creatures that are immune to sneak attacks are also immune to targeted strikes. Items or abilities that protect a creature from critical hits also protect a creature from targeted strikes.");

            targeted_strike_deed = CreateFeature("TargetedStrikeSwashbucklerFeature",
                                                 base_name,
                                                 "At 7th level, as a full-round action the swashbuckler can spend 1 panache point to make an attack with a single light or one-handed piercing melee weapon that cripples part of a foe’s body. The swashbuckler chooses a part of the body to target. If the attack succeeds, in addition to the attack’s normal damage, the target suffers one of the following effects based on the part of the body targeted. If a creature doesn’t have one of the listed body locations, that body part cannot be targeted. Creatures that are immune to sneak attacks are also immune to targeted strikes. Items or abilities that protect a creature from critical hits also protect a creature from targeted strikes.",
                                                 "a28dc4a06df947f8b519fed5b4e72153",
                                                 null, //TODO icon
                                                 FeatureGroup.None,
                                                 Helpers.CreateAddFact(wrapper)
                                                 );
        }

        static void createBleedingWoundDeed()
        {
            var bleed1d6 = library.Get<BlueprintBuff>("75039846c3d85d940aa96c249b97e562");
            var icon = NewSpells.deadly_juggernaut.Icon;
            var spend_one_panache = Create<SpendPanache>(s => s.amount = 1);
            var spend_two_panache = Create<SpendPanache>(s => s.amount = 2);
            int bleeding_wound_group = 110103; //Quick and dirty

            // Basic Bleed
            var bleeding_buff = Helpers.CreateBuff("BleedingWoundBleedSwashbucklerBuff",
                                                   "Bleeding Wound",
                                                   "At 11th level, when the swashbuckler hits a living creature with a light or one-handed piercing melee weapon attack, as a free action she can spend 1 panache point to have that attack deal additional bleed damage. The amount of bleed damage dealt is equal to the swashbuckler’s Dexterity modifier (minimum 1). Alternatively, the swashbuckler can spend 2 panache points to deal 1 point of Strength, Dexterity, or Constitution bleed damage instead (swashbuckler’s choice). Creatures that are immune to sneak attacks are also immune to these types of bleed damage.",
                                                   "0a604a29042d4621a94f734657403702",
                                                   icon,
                                                   null,
                                                   Helpers.Create<BleedBuff>(b => b.dice_value = Helpers.CreateContextDiceValue(DiceType.Zero, 0, Helpers.CreateContextValue(AbilityRankType.Default))),
                                                   Helpers.CreateContextRankConfig(ContextRankBaseValueType.StatBonus, stat: StatType.Dexterity, min: 0),
                                                   Helpers.CreateSpellDescriptor(SpellDescriptor.Bleed),
                                                   bleed1d6.GetComponent<CombatStateTrigger>(),
                                                   bleed1d6.GetComponent<AddHealTrigger>()
                                                   );
            var apply_buff = Common.createContextActionApplyBuff(bleeding_buff, Helpers.CreateContextDuration(), dispellable: false, is_permanent: true);
            var bleeding_wound_buff = Helpers.CreateBuff("BleedingWoundAttackSwashbucklerBuff",
                                                         "Bleeding Wound",
                                                         bleeding_buff.Description,
                                                         "064247db294242ae9dca76a20a929832",
                                                         icon,
                                                         null,
                                                         Create<ApplySwashbucklerBleedOnHit>(a => { a.need_resource = 1; a.Action = Helpers.CreateActionList(apply_buff); })
                                                         );
            var bleeding_wound_toggle = Helpers.CreateActivatableAbility("BleedingWoundSwashbucklerToggleAbility",
                                                                         bleeding_wound_buff.Name,
                                                                         bleeding_wound_buff.Description,
                                                                         "719db741eb4e498bb42d718f5be28e94",
                                                                         bleeding_wound_buff.Icon,
                                                                         bleeding_wound_buff,
                                                                         AbilityActivationType.Immediately,
                                                                         UnitCommand.CommandType.Free,
                                                                         null,
                                                                         CallOfTheWild.Helpers.CreateActivatableResourceLogic(panache_resource, ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                                                                         );
            bleeding_wound_toggle.Group = (ActivatableAbilityGroup)bleeding_wound_group;
            bleeding_wound_toggle.DeactivateImmediately = true;

            //Strength Bleed
            var str_damage = Helpers.CreateActionDealDamage(StatType.Strength, Helpers.CreateContextDiceValue(DiceType.Zero, 0, 1), IgnoreCritical: true);
            var bleeding_strength_buff = Helpers.CreateBuff("BleedingWoundStrengthSwashbucklerBuff",
                                                            "Bleeding Wound - Strength",
                                                            "At 11th level, when the swashbuckler hits a living creature with a light or one-handed piercing melee weapon attack, as a free action she can spend 1 panache point to have that attack deal additional bleed damage. The amount of bleed damage dealt is equal to the swashbuckler’s Dexterity modifier (minimum 1). Alternatively, the swashbuckler can spend 2 panache points to deal 1 point of Strength, Dexterity, or Constitution bleed damage instead (swashbuckler’s choice). Creatures that are immune to sneak attacks are also immune to these types of bleed damage.",
                                                            "22c2497635924200b46abd02ec77598a",
                                                            icon,
                                                            null,
                                                            Helpers.CreateAddFactContextActions(newRound: str_damage),
                                                            Helpers.CreateSpellDescriptor(SpellDescriptor.Bleed),
                                                            bleed1d6.GetComponent<CombatStateTrigger>(),
                                                            bleed1d6.GetComponent<AddHealTrigger>()
                                                            );
            var apply_strength_buff = Common.createContextActionApplyBuff(bleeding_strength_buff, Helpers.CreateContextDuration(), dispellable: false, is_permanent: true);
            var bleeding_wound_strength_buff = Helpers.CreateBuff("BleedingWoundStrengthAttackSwashbucklerBuff",
                                                                  "Bleeding Wound - Strength",
                                                                  bleeding_strength_buff.Description,
                                                                  "3f8b685f59bd4923a30cb04769216509",
                                                                  icon,
                                                                  null,
                                                                  Create<ApplySwashbucklerBleedOnHit>(a => { a.need_resource = 2; a.Action = Helpers.CreateActionList(apply_strength_buff); })
                                                                  );
            var bleeding_wound_strength_toggle = Helpers.CreateActivatableAbility("BleedingWoundStrengthSwashbucklerToggleAbility",
                                                                                  bleeding_wound_strength_buff.Name,
                                                                                  bleeding_wound_strength_buff.Description,
                                                                                  "525d3567d3a54623a5c7867dcd3d20de",
                                                                                  bleeding_wound_strength_buff.Icon,
                                                                                  bleeding_wound_strength_buff,
                                                                                  AbilityActivationType.Immediately,
                                                                                  UnitCommand.CommandType.Free,
                                                                                  null,
                                                                                  CallOfTheWild.Helpers.CreateActivatableResourceLogic(panache_resource, ActivatableAbilityResourceLogic.ResourceSpendType.Never),
                                                                                  Helpers.Create<RestrictionHasEnoughResource>(r => { r.resource = panache_resource; r.amount = 2; })
                                                                                  );
            bleeding_wound_strength_toggle.Group = (ActivatableAbilityGroup)bleeding_wound_group;
            bleeding_wound_strength_toggle.DeactivateImmediately = true;

            //Dexterity Bleed
            var dex_damage = Helpers.CreateActionDealDamage(StatType.Dexterity, Helpers.CreateContextDiceValue(DiceType.Zero, 0, 1), IgnoreCritical: true);
            var bleeding_dexterity_buff = Helpers.CreateBuff("BleedingWoundDexteritySwashbucklerBuff",
                                                             "Bleeding Wound - Dexterity",
                                                             "At 11th level, when the swashbuckler hits a living creature with a light or one-handed piercing melee weapon attack, as a free action she can spend 1 panache point to have that attack deal additional bleed damage. The amount of bleed damage dealt is equal to the swashbuckler’s Dexterity modifier (minimum 1). Alternatively, the swashbuckler can spend 2 panache points to deal 1 point of Strength, Dexterity, or Constitution bleed damage instead (swashbuckler’s choice). Creatures that are immune to sneak attacks are also immune to these types of bleed damage.",
                                                             "abda243ff272482d83ef77c89977db2f",
                                                             icon,
                                                             null,
                                                             Helpers.CreateAddFactContextActions(newRound: dex_damage),
                                                             Helpers.CreateSpellDescriptor(SpellDescriptor.Bleed),
                                                             bleed1d6.GetComponent<CombatStateTrigger>(),
                                                             bleed1d6.GetComponent<AddHealTrigger>()
                                                             );
            var apply_dexterity_buff = Common.createContextActionApplyBuff(bleeding_dexterity_buff, Helpers.CreateContextDuration(), dispellable: false, is_permanent: true);
            var bleeding_wound_dexterity_buff = Helpers.CreateBuff("BleedingWoundDexterityAttackSwashbucklerBuff",
                                                                   "Bleeding Wound - Dexterity",
                                                                   bleeding_dexterity_buff.Description,
                                                                   "0dc0a9feb1d54c129dedf07f93b29186",
                                                                   icon,
                                                                   null,
                                                                   Create<ApplySwashbucklerBleedOnHit>(a => { a.need_resource = 2; a.Action = Helpers.CreateActionList(apply_dexterity_buff); })
                                                                   );
            var bleeding_wound_dexterity_toggle = Helpers.CreateActivatableAbility("BleedingWoundDexteritySwashbucklerToggleAbility",
                                                                                   bleeding_wound_dexterity_buff.Name,
                                                                                   bleeding_wound_dexterity_buff.Description,
                                                                                   "d0f1aede8d78495e955f1a83b6b76060",
                                                                                   bleeding_wound_dexterity_buff.Icon,
                                                                                   bleeding_wound_dexterity_buff,
                                                                                   AbilityActivationType.Immediately,
                                                                                   UnitCommand.CommandType.Free,
                                                                                   null,
                                                                                   CallOfTheWild.Helpers.CreateActivatableResourceLogic(panache_resource, ActivatableAbilityResourceLogic.ResourceSpendType.Never),
                                                                                   Helpers.Create<RestrictionHasEnoughResource>(r => { r.resource = panache_resource; r.amount = 2; })
                                                                                   );
            bleeding_wound_dexterity_toggle.Group = (ActivatableAbilityGroup)bleeding_wound_group;
            bleeding_wound_dexterity_toggle.DeactivateImmediately = true;

            //Constitution Bleed
            var con_damage = Helpers.CreateActionDealDamage(StatType.Constitution, Helpers.CreateContextDiceValue(DiceType.Zero, 0, 1), IgnoreCritical: true);
            var bleeding_constitution_buff = Helpers.CreateBuff("BleedingWoundConstitutionSwashbucklerBuff",
                                                                "Bleeding Wound - Constitution",
                                                                "At 11th level, when the swashbuckler hits a living creature with a light or one-handed piercing melee weapon attack, as a free action she can spend 1 panache point to have that attack deal additional bleed damage. The amount of bleed damage dealt is equal to the swashbuckler’s Dexterity modifier (minimum 1). Alternatively, the swashbuckler can spend 2 panache points to deal 1 point of Strength, Dexterity, or Constitution bleed damage instead (swashbuckler’s choice). Creatures that are immune to sneak attacks are also immune to these types of bleed damage.",
                                                                "f32256d2e55249ab82054ca797219bc1",
                                                                icon,
                                                                null,
                                                                Helpers.CreateAddFactContextActions(newRound: con_damage),
                                                                Helpers.CreateSpellDescriptor(SpellDescriptor.Bleed),
                                                                bleed1d6.GetComponent<CombatStateTrigger>(),
                                                                bleed1d6.GetComponent<AddHealTrigger>()
                                                                );
            var apply_constitution_buff = Common.createContextActionApplyBuff(bleeding_constitution_buff, Helpers.CreateContextDuration(), dispellable: false, is_permanent: true);
            var bleeding_wound_constitution_buff = Helpers.CreateBuff("BleedingWoundConstitutionAttackSwashbucklerBuff",
                                                                      "Bleeding Wound - Constitution",
                                                                      bleeding_constitution_buff.Description,
                                                                      "9ec3a07dda584a0986350e7c624f9755",
                                                                      icon,
                                                                      null,
                                                                      Create<ApplySwashbucklerBleedOnHit>(a => { a.need_resource = 2; a.Action = Helpers.CreateActionList(apply_constitution_buff); })
                                                                      );
            var bleeding_wound_constitution_toggle = Helpers.CreateActivatableAbility("BleedingWoundConstitutionSwashbucklerToggleAbility",
                                                                                      bleeding_wound_constitution_buff.Name,
                                                                                      bleeding_wound_constitution_buff.Description,
                                                                                      "3371cb969f2e47a5915ff41e21f4e4a2",
                                                                                      bleeding_wound_constitution_buff.Icon,
                                                                                      bleeding_wound_constitution_buff,
                                                                                      AbilityActivationType.Immediately,
                                                                                      UnitCommand.CommandType.Free,
                                                                                      null,
                                                                                      CallOfTheWild.Helpers.CreateActivatableResourceLogic(panache_resource, ActivatableAbilityResourceLogic.ResourceSpendType.Never),
                                                                                      Helpers.Create<RestrictionHasEnoughResource>(r => { r.resource = panache_resource; r.amount = 2; })
                                                                                      );
            bleeding_wound_constitution_toggle.Group = (ActivatableAbilityGroup)bleeding_wound_group;
            bleeding_wound_constitution_toggle.DeactivateImmediately = true;

            bleeding_wound_deed = CreateFeature("BleedingWoundSwashbucklerFeature",
                                                bleeding_buff.Name,
                                                bleeding_buff.Description,
                                                "d6ba68729e3343348f06fd3521a26d6a",
                                                bleeding_buff.Icon,
                                                FeatureGroup.None,
                                                CallOfTheWild.Helpers.CreateAddFact(bleeding_wound_toggle),
                                                CallOfTheWild.Helpers.CreateAddFact(bleeding_wound_strength_toggle),
                                                CallOfTheWild.Helpers.CreateAddFact(bleeding_wound_dexterity_toggle),
                                                CallOfTheWild.Helpers.CreateAddFact(bleeding_wound_constitution_toggle)
                                                );
        }

        static void createEvasiveDeed()
        {
            var evasion = library.Get<BlueprintFeature>("576933720c440aa4d8d42b0c54b77e80");
            var uncanny_dodge = library.Get<BlueprintFeature>("3c08d842e802c3e4eb19d15496145709");
            var improved_uncanny_dodge = library.Get<BlueprintFeature>("485a18c05792521459c7d06c63128c79");

            var evasive_buff = CreateBuff("EvasiveSwashbucklerBuff",
                                          "Evasive",
                                          "At 11th level, while a swashbuckler has at least 1 panache point, she gains the benefits of the evasion, uncanny dodge, and improved uncanny dodge rogue class features. She uses her swashbuckler level as her rogue level for improved uncanny dodge.",
                                          "689349bd582c4a13bbf3f9eee76bd038",
                                          improved_uncanny_dodge.Icon,
                                          null,
                                          CreateAddFacts(new BlueprintUnitFact[] { evasion, uncanny_dodge, improved_uncanny_dodge })
                                          );

            var evasive_hidden_toggle = CreateActivatableAbility("EvasiveHiddenSwashbucklerToggleAbility",
                                                                 evasive_buff.Name,
                                                                 evasive_buff.Description,
                                                                 "d73a856ad4c549698b863ede6d29f5a2",
                                                                 evasive_buff.Icon,
                                                                 evasive_buff,
                                                                 AbilityActivationType.Immediately,
                                                                 CommandType.Free,
                                                                 null,
                                                                 CallOfTheWild.Helpers.CreateActivatableResourceLogic(panache_resource, ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                                                                 );
            evasive_hidden_toggle.IsOnByDefault = true;
            evasive_hidden_toggle.DeactivateIfOwnerDisabled = false;
            evasive_hidden_toggle.DeactivateIfCombatEnded = false;
            evasive_hidden_toggle.DeactivateIfOwnerUnconscious = false;
            evasive_hidden_toggle.DeactivateImmediately = false;

            evasive_deed = CreateFeature("EvasiveSwashbucklerFeature",
                                         evasive_buff.Name,
                                         evasive_buff.Description,
                                         "4f5cf35a3d8047f9b3dc8258c97eb2f4",
                                         evasive_buff.Icon,
                                         FeatureGroup.None,
                                         CallOfTheWild.Helpers.CreateAddFact(evasive_hidden_toggle)
                                         );
        }

        static void createSubtleBlade()
        {
            subtle_blade_deed = CreateFeature("SubtleBladeSwashbucklerFeature",
                                              "Subtle Blade",
                                              "At 11th level, while a swashbuckler has at least 1 panache point, she is immune to disarm combat maneuvers made against a light or one-handed piercing melee weapon she is wielding.",
                                              "987932d860cf432fae1d9ead0e9a11d1",
                                              null, //TODO icon
                                              FeatureGroup.None,
                                              Create<SwashbucklerWeaponDisarmImmune>()
                                              );
        }

        static void createDizzyingDefence()
        {
            var fight_defensively_buff = library.Get<BlueprintBuff>("6ffd93355fb3bcf4592a5d976b1d32a9");

            dizzying_defence_deed = CreateFeature("DizzyingDefenceSwashbucklerFeature",
                                                  "Dizzying Defence",
                                                  "At 15th level, while wielding a light or one-handed piercing melee weapon in one hand, the swashbuckler can spend 1 panache point to take the fighting defensively action as a swift action instead of a standard action. When fighting defensively in this manner, the dodge bonus to AC gained from that action increases to +4, and the penalty to attack rolls is reduced to –2.",
                                                  "49b5a4448e924bd2bd58289facd6ad7b",
                                                  null, //TODO icon
                                                  FeatureGroup.None
                                                  );

            var dizzying_defence_buff = CreateBuff("DizzyingDefenceToggleSwashbucklerBuff",
                                                   dizzying_defence_deed.Name,
                                                   dizzying_defence_deed.Description,
                                                   "9800e6e70ec84664b94b4cf1125e7c42",
                                                   dizzying_defence_deed.Icon,
                                                   null
                                                   );

            var dizzying_defence_toggle = Helpers.CreateActivatableAbility("DizzyingDefenceSwashbucklerActivatableAbility",
                                                                           dizzying_defence_deed.Name,
                                                                           dizzying_defence_deed.Description,
                                                                           "34bcda568152458abd30c30bc5dcda47",
                                                                           dizzying_defence_deed.Icon,
                                                                           dizzying_defence_buff,
                                                                           AbilityActivationType.Immediately,
                                                                           CommandType.Free,
                                                                           null,
                                                                           CallOfTheWild.Helpers.CreateActivatableResourceLogic(panache_resource, ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                                                                           );
            dizzying_defence_toggle.DeactivateImmediately = true;

            dizzying_defence_deed.AddComponent(Helpers.CreateAddFact(dizzying_defence_toggle));

            var actions = fight_defensively_buff.GetComponent<AddFactContextActions>();
            var conditional = Helpers.CreateConditional(Common.createContextConditionCasterHasFact(dizzying_defence_buff),
                                                        Common.createContextActionSpendResource(panache_resource, 1)
                                                        );
            var new_actions = CallOfTheWild.ExtensionMethods.AddToArray(actions.Activated.Actions, conditional);
            actions.Activated.Actions = new_actions;

            // TODO - AC Buff
        }

        static void createPerfectThrust()
        {
            var buff = CreateBuff("PerfectThrustSwashbucklerBuff",
                                  "",
                                  "",
                                  "5819e91fcc644de9af51c13263979a3b",
                                  null,
                                  null,
                                  Create<AttackTargetsTouchAC>(),
                                  Create<IgnoreAllDR>()
                                  );
            buff.SetBuffFlags(BuffFlags.HiddenInUi);

            var perfect_thrust_ability = CreateAbility("PerfectThrustSwashbucklerAbility",
                                                       "Perfect Thrust",
                                                       "At 15th level, while the swashbuckler has at least 1 panache point, she can as a full-round action make a perfect thrust, pooling all of her attack potential into a single melee attack made with a light or one-handed piercing melee weapon. When she does, she makes the attack against the target’s touch AC, and ignores all damage reduction.",
                                                       "b451b15ae03e4c839742e77e9ed63826",
                                                       null, //TODO icon
                                                       AbilityType.Extraordinary,
                                                       CommandType.Standard,
                                                       AbilityRange.Weapon,
                                                       "",
                                                       "",
                                                       Create<AbilityCasterSwashbucklerWeaponCheck>(),
                                                       Create<AbilityCasterHasAtLeastOnePanache>(),
                                                       Helpers.Create<AttackAnimation>(),
                                                       Helpers.CreateRunActions(Common.createContextActionOnContextCaster(Common.createContextActionApplyBuff(buff, Helpers.CreateContextDuration(1), dispellable: false)),
                                                                                Common.createContextActionAttack(Helpers.CreateActionList(Common.createContextActionOnContextCaster(Common.createContextActionRemoveBuff(buff))),
                                                                                                                 Helpers.CreateActionList(Common.createContextActionOnContextCaster(Common.createContextActionRemoveBuff(buff)))
                                                                                                                 )
                                                                                )
                                                       );
            Common.setAsFullRoundAction(perfect_thrust_ability);
            perfect_thrust_ability.setMiscAbilityParametersTouchHarmful(works_on_allies: false);

            perfect_thrust_deed = CreateFeature("PerfectThrustSwashbucklerFeature",
                                                perfect_thrust_ability.Name,
                                                perfect_thrust_ability.Description,
                                                "3b2f14ae7c664eb58b59389200e29b20",
                                                perfect_thrust_ability.Icon,
                                                FeatureGroup.None,
                                                Helpers.CreateAddFact(perfect_thrust_ability)
                                                );
        }

        static void createSwashbucklersEdge()
        {
            var swashbucklers_edge_buff = CreateBuff("SwashbucklersEdgeSwashbucklerBuff",
                                                     "Swashbuckler's Edge",
                                                     "At 15th level, while the swashbuckler has at least 1 panache point, she can take 10 on any Athletics or Mobility check, even while distracted or in immediate danger. She can use this ability in conjunction with the derring-do deed.",
                                                     "a2c643b0efad440aa3a38c48f9f59c0b",
                                                     null, //TODO icon
                                                     null,
                                                     Helpers.Create<ModifyD20>(m => { m.Replace = true; m.SpecificSkill = true; m.Rule = RuleType.SkillCheck; m.Skill = new StatType[] { StatType.SkillAthletics, StatType.SkillMobility }; m.Roll = 10; })
                                                     );

            var swashbucklers_edge_toggle_ability = CreateActivatableAbility("SwashbucklersEdgeSwashbucklerActivatableAbility",
                                                                             swashbucklers_edge_buff.Name,
                                                                             swashbucklers_edge_buff.Description,
                                                                             "0939dc9b6e6b47eabf965b4e0d4d3a22",
                                                                             swashbucklers_edge_buff.Icon,
                                                                             swashbucklers_edge_buff,
                                                                             AbilityActivationType.Immediately,
                                                                             CommandType.Free,
                                                                             null,
                                                                             CallOfTheWild.Helpers.CreateActivatableResourceLogic(panache_resource, ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                                                                             );
            swashbucklers_edge_toggle_ability.IsOnByDefault = true;
            swashbucklers_edge_toggle_ability.DeactivateIfOwnerDisabled = false;
            swashbucklers_edge_toggle_ability.DeactivateIfCombatEnded = false;
            swashbucklers_edge_toggle_ability.DeactivateIfOwnerUnconscious = false;

            swashbucklers_edge_deed = CreateFeature("SwashbucklersEdgeSwashbucklerFeature",
                                                    swashbucklers_edge_buff.Name,
                                                    swashbucklers_edge_buff.Description,
                                                    "bb4bd22679b0461aa0118b46fda3e6af",
                                                    swashbucklers_edge_buff.Icon,
                                                    FeatureGroup.None,
                                                    CallOfTheWild.Helpers.CreateAddFact(swashbucklers_edge_toggle_ability)
                                                    );
        }

        static void createCheatDeath()
        {
            var cheat_death_buff = CreateBuff("CheatDeathSwashbucklerBuff",
                                              "Cheat Death",
                                              "At 19th level, whenever the swashbuckler is reduced to 0 hit points or fewer, she can spend all of her remaining panache to instead be reduced to 1 hit point. She must have at least 1 panache point to spend. Effects that kill the swashbuckler outright without dealing hit point damage are not affected by this ability.",
                                              "5f28a8261ca1470c9b40eeceddff8c5c",
                                              null, // TODO - icon
                                              null,
                                              Create<ReduceIncomingKillingBlow>()
                                              );

            var cheat_death_ability = CreateActivatableAbility("CheatDeathSwashbucklerToggleAbility",
                                                               cheat_death_buff.Name,
                                                               cheat_death_buff.Description,
                                                               "d8319eea70394d768bf44806fd41b600",
                                                               cheat_death_buff.Icon,
                                                               cheat_death_buff,
                                                               AbilityActivationType.Immediately,
                                                               UnitCommand.CommandType.Free,
                                                               null,
                                                               CallOfTheWild.Helpers.CreateActivatableResourceLogic(panache_resource, ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                                                               );
            cheat_death_ability.DeactivateImmediately = false;

            cheat_death_deed = CreateFeature("CheatDeathSwashbucklerFeature",
                                             cheat_death_buff.Name,
                                             cheat_death_buff.Description,
                                             "be66318f75354d5cbed1eb3e60603ac6",
                                             cheat_death_buff.Icon,
                                             FeatureGroup.None,
                                             CallOfTheWild.Helpers.CreateAddFact(cheat_death_ability)
                                             );
        }

        static void createDummyConsumePanache()
        {
            var CONSUME_PANACHE_DUMMY_ABILITY = CreateAbility("DUMMYCONSUMEPANACHEABILITY",
                                                              "CONSUME PANACHE",
                                                              "CONSUME 1 PANACHE - TESTING ONLY",
                                                              "bf8adf354ae947338247ed22f334a671",
                                                              null,
                                                              AbilityType.Extraordinary,
                                                              CommandType.Free,
                                                              AbilityRange.Personal,
                                                              "",
                                                              "",
                                                              Create<AbilityResourceLogic>(a => { a.IsSpendResource = true; a.Amount = 1; a.CostIsCustom = false; a.RequiredResource = panache_resource; })
                                                              );

            var REGAIN_PANACHE_DUMMY_ABILITY = CreateAbility("DUMMYREGAINPANACHEABILITY",
                                                             "REGAIN PANACHE",
                                                             "REGAIN 1 PANACHE - TESTING ONLY",
                                                             "eae08c25bbdf46918b0570e7094c0fdf",
                                                             null,
                                                             AbilityType.Extraordinary,
                                                             CommandType.Free,
                                                             AbilityRange.Personal,
                                                             "",
                                                             "",
                                                             CreateRunActions(restore_panache)
                                                             );

            CONSUME_PANACHE_DUMMY = CreateFeature("DUMMYCONSUMEPANACHEFEATURE",
                                                  "CONSUME PANACHE",
                                                  "CONSUME 1 PANACHE - TESTING ONLY",
                                                  "a04d66b87034493f8391f1e0614004f9",
                                                  null,
                                                  FeatureGroup.None,
                                                  Helpers.CreateAddFact(CONSUME_PANACHE_DUMMY_ABILITY),
                                                  Helpers.CreateAddFact(REGAIN_PANACHE_DUMMY_ABILITY),
                                                  CallOfTheWild.Helpers.CreateAddAbilityResource(panache_resource)
                                                  );
        }

        //COMPONENTS AND HELPERS
        //TODO - move components to Components.cs
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


        // TODO - CONSIDER HARD CODING VALUES SINCE ONLY USED ONCE
        [ComponentName("Add Static Bonus On Initiative Check If ResourceAvailable")]
        [AllowedOn(typeof(BlueprintUnitFact))]
        [AllowMultipleComponents]
        public class AddStaticBonusOnInitiativeCheckIfResourceAvailable : RuleInitiatorLogicComponent<RuleInitiativeRoll>
        {
            public BlueprintAbilityResource resource;
            public int amount;
            public int bonus;

            private int getResourceAmount(RuleInitiativeRoll evt)
            {
                // TODO - Cost reduction feats
                return amount > 0 ? amount : 0;
            }

            public override void OnEventAboutToTrigger(RuleInitiativeRoll evt)
            {
                Main.logger.Log("About to roll initiative");

                int need_resource = getResourceAmount(evt);

                if (evt.Initiator.Descriptor.Resources.GetResourceAmount(resource) < need_resource)
                {
                    Main.logger.Log("Not enough resource - had " + evt.Initiator.Descriptor.Resources.GetResourceAmount(resource) + " and needed " + need_resource);
                    return;
                }
                Main.logger.Log("Adding bonus of " + bonus + " to the roll.");
                evt.AddTemporaryModifier(Owner.Stats.Initiative.AddModifier(bonus, this, ModifierDescriptor.UntypedStackable));
            }

            public override void OnEventDidTrigger(RuleInitiativeRoll evt)
            {
            }
        }

        [ComponentName("Add Exploding D6s on Derring-Do Skills and Spend Panache")]
        [AllowedOn(typeof(BlueprintUnitFact))]
        [AllowedOn(typeof(BlueprintBuff))]
        [AllowMultipleComponents]
        public class AddExplodingD6sToDerringDoSkillChecks : RuleInitiatorLogicComponent<RuleSkillCheck>
        {
            public BlueprintAbilityResource resource = panache_resource;
            private int cost = 1;
            private int will_spend = 0;
            private StatType[] stats = { StatType.SkillMobility, StatType.SkillAthletics };

            private int getResourceCost(RuleSkillCheck evt)
            {
                // TODO - Cost reduction feats
                return cost > 0 ? cost : 0;
            }

            private int calculateExplodingDice(RuleSkillCheck evt)
            {
                int total = 0;
                DiceFormula dice_formula = new DiceFormula(1, DiceType.D6);
                RuleRollDice rule = new RuleRollDice(evt.Initiator, dice_formula);
                int roll = this.Fact.MaybeContext.TriggerRule<RuleRollDice>(rule).Result;
                total += roll;
                Main.logger.Log("Roll 1 was a " + roll);
                if (roll == 6)
                {
                    Main.logger.Log("Exploding!");
                    int attempts = Owner.Stats.Dexterity.Bonus > 0 ? Owner.Stats.Dexterity.Bonus : 1;
                    for (int x = 0; x < attempts; x++)
                    {
                        rule = new RuleRollDice(evt.Initiator, dice_formula);
                        roll = this.Fact.MaybeContext.TriggerRule<RuleRollDice>(rule).Result;
                        total += roll;
                        Main.logger.Log("Extra attempt " + x + " was a " + roll);
                        if (roll != 6)
                        {
                            break;
                        }
                    }
                }
                return total;
            }

            public override void OnEventAboutToTrigger(RuleSkillCheck evt)
            {
                Main.logger.Log("About to roll Derring-Do check");
                will_spend = 0;

                if (!stats.Contains(evt.StatType))
                {
                    Main.logger.Log("Not an Athletics or Mobility check");
                    return;
                }
                Main.logger.Log("Is an Athletics or Mobility check");

                int need_resource = getResourceCost(evt);
                Main.logger.Log("Will cost " + need_resource);

                if (evt.Initiator.Descriptor.Resources.GetResourceAmount(resource) < need_resource)
                {
                    Main.logger.Log("Not enough resource - had " + evt.Initiator.Descriptor.Resources.GetResourceAmount(resource) + " and needed " + need_resource);
                    return;
                }

                will_spend = need_resource;

                int result = calculateExplodingDice(evt);
                Main.logger.Log("Adding bonus of " + result + " to check");
                evt.Bonus.AddModifier(result, this, ModifierDescriptor.UntypedStackable);
            }

            public override void OnEventDidTrigger(RuleSkillCheck evt)
            {
                if (will_spend > 0)
                {
                    Main.logger.Log("Spending " + will_spend + " panache");
                    evt.Initiator.Descriptor.Resources.Spend(resource, will_spend);
                }
                will_spend = 0;
            }
        }

        [ComponentName("Add Dodge Bonus to AC Against Attack")]
        [AllowedOn(typeof(BlueprintUnitFact))]
        [AllowedOn(typeof(BlueprintBuff))]
        [AllowMultipleComponents]
        public class AddACBonusOnAttackAndConsumePanache : RuleTargetLogicComponent<RuleAttackWithWeapon>
        {
            private int cost = 1;
            private int bonus;
            private int will_spend = 0;
            public BlueprintAbilityResource resource = panache_resource;
            public ActionList ActionOnSelf = CreateActionList(Create<ContextActionRemoveSelf>());

            private int getResourceCost(RuleAttackWithWeapon evt)
            {
                // TODO - Cost reduction feats
                return cost > 0 ? cost : 0;
            }

            public override void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
            {
                Main.logger.Log("About to be attacked");
                if (!evt.Weapon.Blueprint.IsMelee)
                {
                    Main.logger.Log("Attack was not with a melee weapon");
                    return;
                }
                will_spend = 0;
                int need_resource = getResourceCost(evt);
                if (evt.Target.Descriptor.Resources.GetResourceAmount(resource) < need_resource)
                {
                    Main.logger.Log("Not enough resource - had " + evt.Target.Descriptor.Resources.GetResourceAmount(resource) + " and needed " + need_resource);
                    return;
                }

                will_spend = need_resource;

                bonus = Owner.Stats.Charisma.Bonus > 0 ? Owner.Stats.Charisma.Bonus : 0;
                Main.logger.Log("Adding bonus of " + bonus + " to AC");
                evt.AddTemporaryModifier(evt.Target.Stats.AC.AddModifier(bonus, this, ModifierDescriptor.Dodge));
            }

            public override void OnEventDidTrigger(RuleAttackWithWeapon evt)
            {
                if (will_spend > 0)
                {
                    Main.logger.Log("Spending " + will_spend + " panache");
                    evt.Target.Descriptor.Resources.Spend(resource, will_spend);
                }
                IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
                if (factContextOwner != null)
                {
                    factContextOwner.RunActionInContext(this.ActionOnSelf, evt.Initiator);
                }
                will_spend = 0;
            }
        }

        [ComponentName("Check Caster is Wearing Light or No Armor and is at Light Load")]
        [AllowedOn(typeof(BlueprintAbility))]
        public class AbilityCasterLightOrNoArmorCheck: BlueprintComponent, IAbilityCasterChecker
        {
            public bool CorrectCaster(UnitEntityData caster)
            {
                if ((!caster.Body.Armor.HasArmor || !caster.Body.Armor.Armor.Blueprint.IsArmor) || (caster.Body.Armor.Armor.Blueprint.ProficiencyGroup == ArmorProficiencyGroup.Light))
                {
                    return true;
                }
                return false;
            }
            public string GetReason()
            {
                return "Require light or no armor";
            }
        }

        [ComponentName("Check Caster is Wielding a Swashbuckler Weapon")]
        [AllowedOn(typeof(BlueprintAbility))]
        [AllowedOn(typeof(BlueprintComponent))]
        public class AbilityCasterSwashbucklerWeaponCheck : BlueprintComponent, IAbilityCasterChecker
        {
            public bool CorrectCaster(UnitEntityData caster)
            {
                return (isLightOrOneHandedPiercingWeapon(caster.Body.PrimaryHand.Weapon.Blueprint) || isLightOrOneHandedPiercingWeapon(caster.Body.SecondaryHand.Weapon.Blueprint));
            }
            public string GetReason()
            {
                return "Require light or one-handed piercing weapon";
            }
        }

        [ComponentName("Check Caster has at least One Panache")]
        [AllowedOn(typeof(BlueprintAbility))]
        public class AbilityCasterHasAtLeastOnePanache : BlueprintComponent, IAbilityCasterChecker
        {
            private BlueprintAbilityResource resource = panache_resource;

            public bool CorrectCaster(UnitEntityData caster)
            {
                return (caster.Descriptor.Resources.GetResourceAmount(resource) > 0);
            }
            public string GetReason()
            {
                return "Require at least 1 panache point";
            }
        }

        [ComponentName("Check Caster is Prone")]
        [AllowedOn(typeof(BlueprintComponent))]
        public class AbilityCasterIsProne : BlueprintComponent, IAbilityCasterChecker
        {
            public bool CorrectCaster(UnitEntityData caster)
            {
                if (!caster.View.IsProne)
                {
                    return false;
                }
                return true;
            }
            public string GetReason()
            {
                return "Must be prone";
            }
        }

        [ComponentName("Swashbuckler Parry and Riposte")]
        [AllowedOn(typeof(BlueprintBuff))]
        public class SwashbucklerParryAndRiposte : OwnedGameLogicComponent<UnitDescriptor>, IGlobalRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, IGlobalRulebookSubscriber
        {
            public void OnEventAboutToTrigger(RuleAttackRoll evt)
            {
                Main.logger.Log("Incoming attack!");
                if (evt.Target.Descriptor.Resources.GetResourceAmount(resource) < cost)
                {
                    return;
                }
                if (!evt.Weapon.Blueprint.IsMelee || evt.Parry != null || !base.Owner.Unit.IsEnemy(evt.Initiator))
                {
                    return;
                }
                if (evt.Target != base.Owner.Unit)
                {
                    return;
                }
                if (!base.Owner.Unit.IsReach(evt.Target, base.Owner.Body.PrimaryHand))
                {
                    return;
                }
                //TODO - Conditions?
                /*
                if (this.AttackerCondition.HasConditions)
                {
                    MechanicsContext maybeContext = base.Fact.MaybeContext;
                    using ((maybeContext != null) ? maybeContext.GetDataScope(evt.Initiator) : null)
                    {
                        if (!this.AttackerCondition.Check(null))
                        {
                            return;
                        }
                    }
                }
                */
                evt.TryParry(base.Owner.Unit, base.Owner.Body.PrimaryHand.Weapon, 0);
                if (evt.Parry == null)
                {
                    return;
                }
                ModifiableValue additionalAttackBonus = base.Owner.Stats.AdditionalAttackBonus;
                int num = evt.Initiator.Descriptor.State.Size - base.Owner.State.Size;
                if (num > 0)
                {
                    int value = -2 * num;
                    evt.AddTemporaryModifier(additionalAttackBonus.AddModifier(value, this, ModifierDescriptor.Penalty));
                }
            }

            public void OnEventDidTrigger(RuleAttackRoll evt)
            {
                RuleAttackRoll.ParryData parry = evt.Parry;
                if (((parry != null) ? parry.Initiator : null) != base.Owner.Unit)
                {
                    return;
                }

                if (!evt.Parry.IsTriggered)
                {
                    return;
                }

                evt.Target.Descriptor.Resources.Spend(resource, cost);

                if (evt.Result == AttackResult.Parried && evt.Target.Descriptor.Resources.GetResourceAmount(resource) >= cost)
                {
                    Game.Instance.CombatEngagementController.ForceAttackOfOpportunity(base.Owner.Unit, evt.Initiator);
                }

                //base.Owner.RemoveFact(base.Fact); Unsure what this does in context of original parry

                IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
                if (factContextOwner != null)
                {
                    factContextOwner.RunActionInContext(CreateActionList(Create<ContextActionRemoveSelf>()), evt.Initiator);
                }
            }

            //TODO Conditions
            public ConditionsChecker AttackerCondition;

            private enum TargetType
            {
                Self
            }

            private BlueprintAbilityResource resource = panache_resource;
            private int cost = 1;
        }

        public class GetUpFromProne : ContextAction
        {
            public override string GetCaption()
            {
                return "Get up from prone";
            }

            public override void RunAction()
            {
                var owner = this.Context.MaybeOwner;
                if (owner == null)
                {
                    return;
                };
                owner.Descriptor.State.Prone.ShouldBeActive = false;
                owner.Descriptor.State.Prone.Active = false;
                owner.Descriptor.State.RemoveCondition(UnitCondition.Prone);
                owner.Descriptor.State.Prone.Duration = TimeSpan.Zero;
                owner.View.LeaveProneState();
                owner.View.AnimationManager.StandUpImmediately();
            }
        }

        [AllowMultipleComponents]
        [ComponentName("AA restriction unit prone")]
        public class RestrictionHasUnitProne : ActivatableAbilityRestriction
        {
            public override bool IsAvailable()
            {
                return base.Owner.Unit.View.IsProne;
            }
        }

        [ComponentName("Intimidate on hit if owner has panache")]
        [AllowedOn(typeof(BlueprintUnitFact))]
        public class IndimidateOnHitWithSwashbucklerWeapon : RuleInitiatorLogicComponent<RuleAttackRoll>
        {
            private BlueprintAbilityResource resource = panache_resource;
            private int need_resource = 1;
            public GameAction demoralize_action;

            public override void OnEventAboutToTrigger(RuleAttackRoll evt)
            {
            }
            public override void OnEventDidTrigger(RuleAttackRoll evt)
            {
                if (evt.Initiator.Descriptor.Resources.GetResourceAmount(resource) < need_resource)
                {
                    Main.logger.Log("Not enough resource - had " + evt.Target.Descriptor.Resources.GetResourceAmount(resource) + " and needed " + need_resource);
                    return;
                }

                if (evt.Initiator.CombatState.Cooldown.SwiftAction != 0.0f)
                {
                    Main.logger.Log("Swift action on Cooldown");
                    return;
                }

                Main.logger.Log("Cooldown was " + evt.Initiator.CombatState.Cooldown.SwiftAction);

                if (!isLightOrOneHandedPiercingWeapon(evt.Weapon.Blueprint))
                {
                    return;
                }

                if (!evt.IsHit)
                {
                    return;
                }

                IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
                if (factContextOwner != null)
                {
                    evt.Initiator.CombatState.Cooldown.SwiftAction = 6.0f;
                    factContextOwner.RunActionInContext(Helpers.CreateActionList(demoralize_action), evt.Target);
                }
            }
        }

        [ComponentName("Add bonus precision damage on swashbuckler weapons")]
        [AllowedOn(typeof(BlueprintUnitFact))]
        public class AddBonusPrecisionDamageToSwashbucklerWeapons : RuleInitiatorLogicComponent<RuleAttackRoll>
        {
            private BlueprintAbilityResource resource = panache_resource;
            private int need_resource = 1;
            public bool is_passive;

            public override void OnEventAboutToTrigger(RuleAttackRoll evt)
            {
                if (is_passive && evt.Initiator.Descriptor.Resources.GetResourceAmount(resource) < need_resource)
                {
                    Main.logger.Log("Not enough resource - had " + evt.Target.Descriptor.Resources.GetResourceAmount(resource) + " and needed " + need_resource);
                    return;
                }

                ItemEntityWeapon weapon = evt.Weapon;
                bool flag = isLightOrOneHandedPiercingWeapon(weapon.Blueprint);
                bool flag2 = base.Owner.Body.SecondaryHand.HasWeapon && base.Owner.Body.SecondaryHand.MaybeWeapon != base.Owner.Body.EmptyHandWeapon;
                bool flag3 = base.Owner.Body.SecondaryHand.HasShield;
                if (flag3)
                {
                    ArmorProficiencyGroup proficiencyGroup = base.Owner.Body.SecondaryHand.MaybeShield.Blueprint.Type.ProficiencyGroup;
                    flag3 = !(proficiencyGroup == ArmorProficiencyGroup.Buckler);
                }
                if (flag && !flag2 && !flag3)
                {
                    evt.PreciseStrike += base.Owner.Progression.GetClassLevel(swashbuckler_class);
                }
            }

            public override void OnEventDidTrigger(RuleAttackRoll evt)
            {
                if (is_passive)
                {
                    return;
                }
                IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
                if (factContextOwner != null)
                {
                    factContextOwner.RunActionInContext(CreateActionList(Create<ContextActionRemoveSelf>()), evt.Initiator);
                }
            }
        }

        public class DisarmTarget : ContextAction
        {
            public override string GetCaption()
            {
                return "Disarm target";
            }

            public override void RunAction()
            {
                if (base.Target.Unit == null)
                {
                    return;
                }

                ItemEntityWeapon maybeWeapon = base.Target.Unit.Body.PrimaryHand.MaybeWeapon;
                ItemEntityWeapon maybeWeapon2 = base.Target.Unit.Body.SecondaryHand.MaybeWeapon;
                if (maybeWeapon != null && !maybeWeapon.Blueprint.IsUnarmed && !maybeWeapon.Blueprint.IsNatural)
                {
                    base.Target.Unit.Descriptor.AddBuff(BlueprintRoot.Instance.SystemMechanics.DisarmMainHandBuff, this.Context, TimeSpanExtension.Seconds(6));
                }
                else if (maybeWeapon2 != null && !maybeWeapon2.Blueprint.IsUnarmed && !maybeWeapon2.Blueprint.IsNatural)
                {
                    this.Target.Unit.Descriptor.AddBuff(BlueprintRoot.Instance.SystemMechanics.DisarmOffHandBuff, this.Context, TimeSpanExtension.Seconds(6));
                }
            }
        }

        public class SpendPanache : ContextAction
        {
            private static BlueprintAbilityResource resource = panache_resource;
            public int amount;

            public override string GetCaption()
            {
                return "Spend Panache";
            }

            public override void RunAction()
            {
                var owner = this.Context.MaybeOwner;
                if (owner == null)
                {
                    return;
                };
                owner.Descriptor.Resources.Spend(resource, amount);
            }
        }

        [ComponentName("Delivery ability with a weapon hit")]
        [AllowedOn(typeof(BlueprintAbility))]
        public class AbilityDeliverHitWithMeleeWeapon : AbilityDeliverEffect
        {
            public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper target)
            {
                UnitEntityData caster = context.MaybeCaster;
                if (caster == null)
                {
                    UberDebug.LogError("Caster is missing", Array.Empty<object>());
                    yield break;
                }

                RulebookEventContext rulebookContext = context.RulebookContext;
                RuleAttackWithWeapon attackWithWeapon = (rulebookContext != null) ? rulebookContext.AllEvents.LastOfType<RuleAttackWithWeapon>() : null;
                RuleAttackWithWeapon ruleAttackWithWeapon = attackWithWeapon;
                RuleAttackRoll attackRoll = (ruleAttackWithWeapon != null) ? ruleAttackWithWeapon.AttackRoll : null;
                attackRoll = (attackRoll ?? context.TriggerRule<RuleAttackRoll>(new RuleAttackRoll(caster, target.Unit, caster.GetFirstWeapon(), 0)));
                if (attackWithWeapon == null)
                {
                    attackRoll.ConsumeMirrorImageIfNecessary();
                }
                yield return new AbilityDeliveryTarget(target)
                {
                    AttackRoll = attackRoll
                };
                yield break;
            }
        }

        [ComponentName("Check Target is not Immune to Precision Damage")]
        [AllowedOn(typeof(BlueprintComponent))]
        public class AbilityTargetNotImmuneToPrecision : BlueprintComponent, IAbilityTargetChecker
        {
            public bool CanTarget(UnitEntityData caster, TargetWrapper target)
            {
                UnitEntityData unit = target.Unit;
                if (unit == null)
                {
                    return false;
                }


                if (target.Unit.Descriptor.Progression.Features.Enumerable.Any(f => f.Blueprint.GetComponent<AddImmunityToPrecisionDamage>() != null) || target.Unit.Descriptor.Buffs.Enumerable.Any(f => f.Blueprint.GetComponent<AddImmunityToPrecisionDamage>() != null))
                {
                    return false;
                }

                return true;
            }
        }

        public class AbilityDeliverAttackWithWeaponOnHit : AbilityDeliverEffect
        {
            public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper target)
            {
                if (target.Unit == null)
                {
                    UberDebug.LogError("Target unit is missing", Array.Empty<object>());
                    yield break;
                }
                UnitAttack cmd = new UnitAttack(target.Unit)
                {
                    IsSingleAttack = true
                };
                cmd.Init(context.Caster);
                cmd.Start();
                AttackHandInfo attackHandInfo = cmd.AllAttacks.FirstOrDefault<AttackHandInfo>();
                ItemEntityWeapon weapon = (attackHandInfo != null) ? attackHandInfo.Weapon : null;
                if (weapon == null)
                {
                    UberDebug.LogError("Has no weapon for attack", Array.Empty<object>());
                    cmd.Interrupt();
                    yield break;
                }
                bool hitHandled = false;
                bool isMelee = weapon.Blueprint.IsMelee;
                for (; ; )
                {
                    if (cmd.IsFinished)
                    {
                        RuleAttackWithWeapon lastAttackRule = cmd.LastAttackRule;
                        if (((lastAttackRule != null) ? lastAttackRule.Projectile : null) == null || cmd.LastAttackRule.Projectile.IsHit || cmd.LastAttackRule.Projectile.Cleared || cmd.LastAttackRule.Projectile.Destroyed)
                        {
                            break;
                        }
                    }
                    bool wasActed = cmd.IsActed;
                    if (!cmd.IsFinished)
                    {
                        cmd.Tick();
                    }
                    RuleAttackWithWeapon lastAttackRule2 = cmd.LastAttackRule;
                    if (!wasActed && cmd.IsActed && isMelee)
                    {
                        hitHandled = true;
                        if (lastAttackRule2.AttackRoll.IsHit)
                        {
                            yield return new AbilityDeliveryTarget(target);
                        }
                    }
                    yield return null;
                }
                if (!hitHandled && !isMelee)
                {
                    RuleAttackWithWeapon lastAttackRule3 = cmd.LastAttackRule;
                    bool? flag3 = (lastAttackRule3 != null) ? new bool?(lastAttackRule3.AttackRoll.IsHit) : null;
                    if (flag3 != null && flag3.Value)
                    {
                        yield return new AbilityDeliveryTarget(target);
                    }
                }
                yield break;
            }
        }

        [ComponentName("Add bonus bleed damage on swashbuckler weapons")]
        [AllowedOn(typeof(BlueprintUnitFact))]
        public class ApplySwashbucklerBleedOnHit : RuleInitiatorLogicComponent<RuleAttackRoll>
        {
            private BlueprintAbilityResource resource = panache_resource;
            public ActionList Action;
            public int need_resource;

            public override void OnEventAboutToTrigger(RuleAttackRoll evt)
            {
            }

            public override void OnEventDidTrigger(RuleAttackRoll evt)
            {
                if (evt.Initiator.Descriptor.Resources.GetResourceAmount(resource) < need_resource)
                {
                    Main.logger.Log("Not enough resource - had " + evt.Target.Descriptor.Resources.GetResourceAmount(resource) + " and needed " + need_resource);
                    return;
                }

                if (!isLightOrOneHandedPiercingWeapon(evt.Weapon.Blueprint))
                {
                    return;
                }

                if (evt.ImmuneToSneakAttack)
                {
                    return;
                }

                if (!evt.IsHit)
                {
                    return;
                }

                evt.Initiator.Descriptor.Resources.Spend(resource, need_resource);

                IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
                if (factContextOwner != null)
                {
                    factContextOwner.RunActionInContext(this.Action, evt.Target);
                }
            }
        }

        [ComponentName("Immune to Disarm against Swashbuckler Weapons")]
        public class SwashbucklerWeaponDisarmImmune : RuleTargetLogicComponent<RuleCombatManeuver>
        {
            private BlueprintAbilityResource resource = panache_resource;
            private int amount = 1;

            public override void OnEventAboutToTrigger(RuleCombatManeuver evt)
            {
                if (!(evt.Type == CombatManeuver.Disarm))
                {
                    return;
                }

                if (evt.Target.Descriptor.Resources.GetResourceAmount(resource) < amount)
                {
                    return;
                }

                ItemEntityWeapon maybeWeapon = evt.Target.Body.PrimaryHand.MaybeWeapon;
                ItemEntityWeapon maybeWeapon2 = evt.Target.Body.SecondaryHand.MaybeWeapon;
                if ((maybeWeapon != null && !maybeWeapon.Blueprint.IsUnarmed && !maybeWeapon.Blueprint.IsNatural) && isLightOrOneHandedPiercingWeapon(maybeWeapon.Blueprint))
                {
                    evt.AutoFailure = true;
                }
                else if ((maybeWeapon2 != null && !maybeWeapon2.Blueprint.IsUnarmed && !maybeWeapon2.Blueprint.IsNatural) && isLightOrOneHandedPiercingWeapon(maybeWeapon.Blueprint))
                {
                    evt.AutoFailure = true;
                }
            }

            public override void OnEventDidTrigger(RuleCombatManeuver evt)
            {
            }
        }

        [ComponentName("Resolve Attack against Touch AC")]
        [AllowedOn(typeof(BlueprintBuff))]
        public class AttackTargetsTouchAC : RuleInitiatorLogicComponent<RuleAttackRoll>
        {
            public override void OnEventAboutToTrigger(RuleAttackRoll evt)
            {
                evt.AttackType = AttackType.Touch;
            }

            public override void OnEventDidTrigger(RuleAttackRoll evt)
            {
            }
        }

        [ComponentName("Ignore all DR")]
        [AllowedOn(typeof(BlueprintBuff))]
        public class IgnoreAllDR : RuleInitiatorLogicComponent<RuleDealDamage>
        {
            public override void OnEventAboutToTrigger(RuleDealDamage evt)
            {
                evt.IgnoreDamageReduction = true;
            }

            public override void OnEventDidTrigger(RuleDealDamage evt)
            {
            }
        }

        [ComponentName("Go to 1 HP when Incoming Damage Would Reduce to 0")]
        [AllowedOn(typeof(BlueprintBuff))]
        public class ReduceIncomingKillingBlow : RuleTargetLogicComponent<RuleDealDamage>
        {
            private static BlueprintAbilityResource resource = panache_resource;

            public override void OnEventAboutToTrigger(RuleDealDamage evt)
            {
            }

            public override void OnEventDidTrigger(RuleDealDamage evt)
            {
                if (evt.Target.Damage < 0)
                {
                    Main.logger.Log(evt.Target.Damage + " is less than zero.");
                    return;
                }

                if (evt.Target.Descriptor.Resources.GetResourceAmount(resource) < 1)
                {
                    Main.logger.Log("Not enough Panache.");
                    return;
                }

                int max_need_reduce = 1 - evt.Target.HPLeft;
                int reduce_damage = Math.Min(max_need_reduce, evt.Target.Damage);
                if (reduce_damage <= 0)
                {
                    return;
                }

                var consume_amount = evt.Target.Descriptor.Resources.GetResourceAmount(resource);
                evt.Target.Descriptor.Resources.Spend(resource, consume_amount);

                evt.Target.Damage -= reduce_damage;
            }
        }
    }
}
