using CallOfTheWild;
using CallOfTheWild.NewMechanics;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using static CallOfTheWild.Helpers;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;

namespace Derring_Do
{
    class RostlandBravo
    {
        static LibraryScriptableObject library = Main.library;

        static public BlueprintArchetype rostland_bravo;

        static public BlueprintFeature rostland_bravo_proficiencies;

        static public BlueprintFeature aldori_swashbuckler;

        static public BlueprintFeature inevitable_victory_deed;

        static public BlueprintFeature sweeping_wind_feint;

        static public BlueprintFeature dragons_rage_deed;

        static public BlueprintFeature terror_of_the_great_wyrm_deed;
        static public BlueprintBuff terror_of_the_great_wyrm_buff;

        static public void create()
        {
            Main.DebugLog("Creating Rostland Bravo archetype");
            rostland_bravo = Helpers.Create<BlueprintArchetype>(a =>
            {
                a.name = "RostlandBravoSwashbucklerArchetype";
                a.LocalizedName = Helpers.CreateString($"{a.name}.Name", "Rostland Bravo");
                a.LocalizedDescription = Helpers.CreateString($"{a.name}.Description", "The Free City of Restov is host to numerous dueling schools, from the renowned Aldori Academy to tiny training grounds in blademasters’ homes. Students of these schools are notoriously competitive, and street-corner duels at dawn and dusk are a constant of Restov life. In most cases, while these “lesser schools” do not teach official Aldori techniques, their methods mesh well with that signature style. Unsurprisingly, many students eventually train in the Aldori style, whether because they aspire to join the swordlords’ ranks or simply for the challenge of mastering the legendary weapon. While some favor more technical approaches, others study flashier maneuvers, wielding the curved blade with artful flair. Disdainfully called “bravos” by classically trained rivals, students of this approach have claimed the label with pride. The Rostland bravos’ most advanced techniques bear dragon-themed names as a snub to traditionalist Aldori swordlords, who have never forgotten their crushing defeat by Choral the Conqueror’s dragons at the Valley of Fire.");
            });
            SetField(rostland_bravo, "m_ParentClass", Swashbuckler.swashbuckler_class);
            library.AddAsset(rostland_bravo, "8a2a3b210e5a4fc1b1429365515695fd");

            rostland_bravo.ReplaceStartingEquipment = true;
            rostland_bravo.StartingItems = new BlueprintItem[]
            {
                library.Get<BlueprintItem>("afbe88d27a0eb544583e00fa78ffb2c7"), //StuddedStandard
                library.Get<BlueprintItem>("4697667a33a6774489e5265a955675a5"), //StandardDuelingSword
                library.Get<BlueprintItem>("bc93a78d71bef084fa155e529660ed0d"), //PotionOfShieldOfFaith
                library.Get<BlueprintItem>("d52566ae8cbe8dc4dae977ef51c27d91"), //PotionOfCureLightWounds
            };

            rostland_bravo.ReplaceClassSkills = true;
            rostland_bravo.ClassSkills = new StatType[] { StatType.SkillMobility, StatType.SkillKnowledgeWorld, StatType.SkillPerception, StatType.SkillPersuasion };

            //Create Features
            createRostlandBravoProficiencies();
            createAldoriSwashbuckler();
            createInevitableVictory();
            createSweepingWindFeint();
            createDragonsRage();
            createTerrorOfTheGreatWyrm();

            rostland_bravo.RemoveFeatures = new LevelEntry[] { Helpers.LevelEntry(1, Swashbuckler.swashbuckler_proficiencies),
                                                               Helpers.LevelEntry(3, Swashbuckler.menacing_swordplay_deed),
                                                               Helpers.LevelEntry(7, Swashbuckler.superior_feint_deed),
                                                               Helpers.LevelEntry(11, Swashbuckler.bleeding_wound_deed),
                                                               Helpers.LevelEntry(15, Swashbuckler.swashbucklers_edge_deed),
                                                             };

            rostland_bravo.AddFeatures = new LevelEntry[] { Helpers.LevelEntry(1, rostland_bravo_proficiencies, aldori_swashbuckler),
                                                            Helpers.LevelEntry(3, inevitable_victory_deed),
                                                            Helpers.LevelEntry(7, sweeping_wind_feint),
                                                            Helpers.LevelEntry(11, dragons_rage_deed),
                                                            Helpers.LevelEntry(15, terror_of_the_great_wyrm_deed),
                                                          };
            Swashbuckler.swashbuckler_progression.UIDeterminatorsGroup = Swashbuckler.swashbuckler_progression.UIDeterminatorsGroup.AddToArray(rostland_bravo_proficiencies, aldori_swashbuckler);
        }

        static void createRostlandBravoProficiencies()
        {
            rostland_bravo_proficiencies = library.CopyAndAdd<BlueprintFeature>("8f8c2640ffad89349883fc2e5ff2091e", //magus proficiencies
                                                                                "RostlandBravoSwashbucklerProficiencies",
                                                                                "12f3c40bac26460f97ec10e3e5e28035");

            rostland_bravo_proficiencies.RemoveComponents<ArcaneArmorProficiency>();
            rostland_bravo_proficiencies.AddComponent(Common.createAddWeaponProficiencies(WeaponCategory.DuelingSword));
            rostland_bravo_proficiencies.SetName("Rostland Bravo Proficiencies");
            rostland_bravo_proficiencies.SetDescription("The Rostland bravo is not proficient with bucklers.");
        }

        static void createAldoriSwashbuckler()
        {
            var dueling_sword = library.Get<BlueprintWeaponType>("a6f7e3dc443ff114ba68b4648fd33e9f");

            aldori_swashbuckler = CreateFeature("AldoriSwashbucklerSwashbucklerFeature",
                                                "Aldori Swashbuckler",
                                                "A Rostland bravo focuses on the Aldori dueling sword, scorning the bucklers used by duelists of other styles. In addition, the relative safety and creature comforts allowed by life in the sprawling city of Restov reduces her need for athleticism. A Rostland bravo gains Exotic Weapon Proficiency (Aldori dueling sword) as a bonus feat. The Rostland bravo is not proficient with bucklers, and does not gain Athletics as a class skill.",
                                                "77a77a09003d4b398ef3f6dc852fd03e",
                                                dueling_sword.Icon,
                                                FeatureGroup.None
                                                );
        }

        static void createInevitableVictory()
        {
            var stunning_barrier = library.Get<BlueprintAbility>("a5ec7892fb1c2f74598b3a82f3fd679f");
            var dazzling_display_feature = library.Get<BlueprintFeature>("bcbd674ec70ff6f4894bb5f07b6f4095");
            var inevitable_victory_ability = library.CopyAndAdd<BlueprintAbility>("5f3126d4120b2b244a95cb2ec23d69fb", "InevitableVictorySwashbucklerAbility", "12e5c8239c7d4ac2a5bd7813996d358e"); //Dazzling display


            inevitable_victory_ability.SetNameDescriptionIcon("Inevitable Victory",
                                                              "The Rostland bravo’s technique is all about flair; a display of her skill is enough to make any Restov brawler reconsider picking a fight. At 3rd level, the Rostland bravo gains Dazzling Display as a bonus feat. She can activate its effect only while wielding, an Aldori dueling sword, and she must spend 1 panache point to do so.",
                                                              stunning_barrier.Icon
                                                              );
            inevitable_victory_ability.AddComponent(Create<AbilityCasterDuelingSwordCheck>());
            inevitable_victory_ability.AddComponent(Create<AbilityResourceLogic>(a => { a.Amount = 1; a.RequiredResource = Swashbuckler.panache_resource; a.IsSpendResource = true; }));

            inevitable_victory_deed = CreateFeature("InevitableVictorySwashbucklerFeature",
                                                    inevitable_victory_ability.Name,
                                                    inevitable_victory_ability.Description,
                                                    "cc548dc23ebe4423af7b002506d2983e",
                                                    inevitable_victory_ability.Icon,
                                                    FeatureGroup.None,
                                                    Helpers.CreateAddFact(inevitable_victory_ability)
                                                    );

            dazzling_display_feature.AddComponent(Create<FeatureReplacement>(f => f.replacement_feature = inevitable_victory_deed));
        }

        static void createSweepingWindFeint()
        {
            var see_invis = library.Get<BlueprintAbility>("30e5dc243f937fc4b95d2f8f4e1b7ff3");
            var sweeping_wind_feint_ability = library.CopyAndAdd<BlueprintAbility>("2d5f22af097646e9a9113bcf42976d7f", "SweepingWindFeintSwashbucklerAbility", "297034d66f284a668b4f05b95761f785"); //from properflanking2

            sweeping_wind_feint_ability.SetNameDescriptionIcon("Sweeping Wind Feint",
                                                               "At 7th level, the Rostland bravo masters an exotic feinting style, tossing her blade to the other hand and performing a sweeping attack or upward slash before the opponent reacts. Once per round, she can spend 1 point of panache to attempt a feint as a swift action.",
                                                               see_invis.Icon
                                                               );
            SetField(sweeping_wind_feint_ability, "ActionType", CommandType.Swift);
            SetField(sweeping_wind_feint_ability, "Type", AbilityType.Extraordinary);
            sweeping_wind_feint_ability.AddComponent(Create<AbilityCasterDuelingSwordCheck>());
            sweeping_wind_feint_ability.AddComponent(Create<AbilityResourceLogic>(a => { a.Amount = 1; a.IsSpendResource = true; a.RequiredResource = Swashbuckler.panache_resource; }));

            sweeping_wind_feint = CreateFeature("SweepingWindFeintSwashbucklerFeature",
                                                sweeping_wind_feint_ability.Name,
                                                sweeping_wind_feint_ability.Description,
                                                "8c7eb71834ed492384e86f8086808c9e",
                                                sweeping_wind_feint_ability.Icon,
                                                FeatureGroup.None,
                                                Helpers.CreateAddFact(sweeping_wind_feint_ability)
                                                );
        }

        static void createDragonsRage()
        {
            var dragons_breath = library.Get<BlueprintAbility>("5e826bcdfde7f82468776b55315b2403");

            var buff = CreateBuff("DragonsRageSwashbucklerBuff",
                                  "Dragon’s Rage",
                                  "At 11th level, the Rostland bravo can cast aside restraint in favor of a blindingly fast assault of unpredictable strikes inspired in part by the overwhelming brutality of a dragon in combat. Once per round as part of a full attack, the bravo can spend 1 panache point to make an additional attack with her Aldori dueling sword at her highest attack bonus. If she reduces a creature to 0 or fewer hit points with this additional attack, she regains 2 panache points rather than the normal 1 point she would gain from striking a killing blow.",
                                  "a7cee45d8dee4416be365b000681472d",
                                  dragons_breath.Icon,
                                  null,
                                  Create<CallOfTheWild.NewMechanics.BuffExtraAttackCategorySpecific>(b => b.categories = new WeaponCategory[] { WeaponCategory.DuelingSword }),
                                  Create<RestorePanacheAttackRollTrigger>(a => { a.ReduceHPToZero = true; a.Action = CreateActionList(Swashbuckler.restore_panache); a.CheckWeaponCategory = true; a.Category = WeaponCategory.DuelingSword; a.OnlyOnFullAttack = true; a.OnlyOnFirstAttack = true; })
                                  );

            var apply_buff = Common.createContextActionApplyBuff(buff, Helpers.CreateContextDuration(1), dispellable: false);

            var ability = CreateAbility("DragonsRageSwashbucklerAbility",
                                        buff.Name,
                                        buff.Description,
                                        "ecf13592ed694f41a7a4b1bfc8f06505",
                                        buff.Icon,
                                        AbilityType.Extraordinary,
                                        CommandType.Free,
                                        AbilityRange.Personal,
                                        oneRoundDuration,
                                        "",
                                        CreateRunActions(apply_buff),
                                        Create<AbilityResourceLogic>(a => { a.Amount = 1; a.IsSpendResource = true; a.RequiredResource = Swashbuckler.panache_resource; }),
                                        Create<AbilityCasterDuelingSwordCheck>()
                                        );
            ability.setMiscAbilityParametersSelfOnly();

            dragons_rage_deed = CreateFeature("DragonsRageSwashbucklerFeature",
                                              buff.Name,
                                              buff.Description,
                                              "52034008859547519fe061a60bd443ee",
                                              buff.Icon,
                                              FeatureGroup.None,
                                              Helpers.CreateAddFact(ability)
                                              );
        }

        static void createTerrorOfTheGreatWyrm()
        {
            var dazzling_display = library.Get<BlueprintAbility>("5f3126d4120b2b244a95cb2ec23d69fb");
            var shaken = library.Get<BlueprintBuff>("25ec6cb6ab1845c48a95f9c20b034220");
            var frightened = library.Get<BlueprintBuff>("f08a7239aa961f34c8301518e71d4cdf");
            var demoralize = (dazzling_display.GetComponent<AbilityEffectRunAction>().Actions.Actions[0] as Demoralize);
            var great_wyrm_demoralize = Helpers.Create<TerrorOfTheGreatWyrmDemoralize>(a => { a.Buff = shaken; a.GreaterBuff = frightened; });
            var fear = library.Get<BlueprintAbility>("d2aeac47450c76347aebbc02e4f463e0");
            
            var great_wyrm_demoralize_action = Helpers.CreateAbility("TerrorOfTheGreatWyrmDemoralizeSwashbucklerAbility",
                                                                     "",
                                                                     "",
                                                                     "161e71362f964d758742750374e71232",
                                                                     null,
                                                                     AbilityType.Extraordinary,
                                                                     CommandType.Free,
                                                                     AbilityRange.Personal,
                                                                     "",
                                                                     "",
                                                                     Helpers.CreateRunActions(great_wyrm_demoralize),
                                                                     dazzling_display.GetComponent<AbilityTargetsAround>(),
                                                                     dazzling_display.GetComponent<AbilitySpawnFx>(),
                                                                     Helpers.Create<CallOfTheWild.NewMechanics.AbilityTargetIsCaster>()
                                                                     );
            

            terror_of_the_great_wyrm_buff = CreateBuff("TerrorOfTheGreatWyrmSwashbucklerBuff",
                                                       "Terror of the Great Wyrm",
                                                       "At 15th level, the Rostland bravo can use her inevitable victory deed as part of a full attack or dragon’s rage. If a creature demoralized in this way would be shaken for 3 or more rounds, the Rostland bravo can make the target frightened for 1 round before becoming shaken for the appropriate duration.",
                                                       "f9906d4f50c2479d9a489c6771cc334f",
                                                       fear.Icon,
                                                       null,
                                                       Create<AddInitiatorAttackWithWeaponTrigger>(a => { a.Category = WeaponCategory.DuelingSword; a.OnlyOnFirstAttack = true; a.OnlyOnFullAttack = true; a.Action = CreateActionList(Create<ContextActionCastSpell>(c => c.Spell = great_wyrm_demoralize_action), Create<SpendPanache>(s => { s.resource = Swashbuckler.panache_resource; s.amount = 1; })); })
                                                       );

            var terror_of_the_great_wyrm_ability = CreateActivatableAbility("TerrorOfTheGreatWyrmSwashbucklerActivatableAbility",
                                                                            terror_of_the_great_wyrm_buff.Name,
                                                                            terror_of_the_great_wyrm_buff.Description,
                                                                            "9f0494e84a3a490ca2d2cc412ac88240",
                                                                            terror_of_the_great_wyrm_buff.Icon,
                                                                            terror_of_the_great_wyrm_buff,
                                                                            AbilityActivationType.Immediately,
                                                                            CommandType.Free,
                                                                            null,
                                                                            CallOfTheWild.Helpers.CreateActivatableResourceLogic(Swashbuckler.panache_resource, ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                                                                            );


            terror_of_the_great_wyrm_deed = CreateFeature("TerrorOfTheGreatWyrmSwashbucklerFeature",
                                                          terror_of_the_great_wyrm_buff.Name,
                                                          terror_of_the_great_wyrm_buff.Description,
                                                          "bb9f3383f6ee40919d1cc434f3c387b8",
                                                          terror_of_the_great_wyrm_buff.Icon,
                                                          FeatureGroup.None,
                                                          Helpers.CreateAddFact(terror_of_the_great_wyrm_ability)
                                                          );
        }
    }
}
