using CallOfTheWild;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            rostland_bravo.RemoveFeatures = new LevelEntry[] { Helpers.LevelEntry(1, Swashbuckler.swashbuckler_proficiencies),
                                                               Helpers.LevelEntry(3, Swashbuckler.menacing_swordplay_deed),
                                                               Helpers.LevelEntry(7, Swashbuckler.superior_feint_deed),
                                                               Helpers.LevelEntry(11, Swashbuckler.bleeding_wound_deed),
                                                               Helpers.LevelEntry(15, Swashbuckler.swashbucklers_edge_deed),
                                                             };

            rostland_bravo.AddFeatures = new LevelEntry[] { Helpers.LevelEntry(1, rostland_bravo_proficiencies),
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
            var dazzling_display_feature = library.Get<BlueprintFeature>("bcbd674ec70ff6f4894bb5f07b6f4095");
            var dazzling_diplay_ability = library.Get<BlueprintAbility>("5f3126d4120b2b244a95cb2ec23d69fb");
            var inevitable_victory_ability = CreateAbility("InevitableVictorySwashbucklerAbility",
                                                           "Inevitable Victory",
                                                           "The Rostland bravo’s technique is all about flair; a display of her skill is enough to make any Restov brawler reconsider picking a fight. At 3rd level, the Rostland bravo gains Dazzling Display as a bonus feat. She can activate its effect only while wielding an Aldori dueling sword, and she must spend 1 panache point to do so.",
                                                           "de27de8ad462406daf934325b02a1bcd",
                                                           null, //TODO icon
                                                           AbilityType.Extraordinary,
                                                           CommandType.Swift,
                                                           dazzling_diplay_ability.Range,
                                                           dazzling_diplay_ability.LocalizedDuration,
                                                           dazzling_diplay_ability.LocalizedSavingThrow
                                                           );
            inevitable_victory_ability.ComponentsArray = dazzling_diplay_ability.ComponentsArray.AddToArray(Create<AbilityCasterDuelingSwordCheck>(), Create<AbilityResourceLogic>(a => { a.Amount = 1; a.IsSpendResource = true; a.RequiredResource = Swashbuckler.panache_resource; }));
        }

        static void createSweepingWindFeint()
        {

        }
    }
}
