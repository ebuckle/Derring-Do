using CallOfTheWild;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CallOfTheWild.Helpers;

namespace Derring_Do
{
    public class InspiredBlade
    {
        static LibraryScriptableObject library = Main.library;

        static public BlueprintArchetype inspired_blade;

        static public BlueprintFeature inspired_panache;

        static public BlueprintFeature inspired_finesse;

        static public BlueprintFeature inspired_strike_deed;

        static public BlueprintFeature rapier_training;

        static public BlueprintFeature rapier_weapon_mastery;

        static public void create()
        {
            inspired_blade = Helpers.Create<BlueprintArchetype>(a =>
            {
                a.name = "InspiredBladeArchetype";
                a.LocalizedName = Helpers.CreateString($"{a.name}.Name", "Inspired Blade");
                a.LocalizedDescription = Helpers.CreateString($"{a.name}.Description", "An inspired blade is both a force of personality and a sage of swordplay dedicated to the perfection of combat with the rapier. They use the science and geometry with swordplay to beautiful and deadly effect.");
            });
            SetField(inspired_blade, "m_ParentClass", Swashbuckler.swashbuckler_class);
            library.AddAsset(inspired_blade, "9dbb6ac330874b358b2b041287f99d18");

            //Create Features

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

            Swashbuckler.swashbuckler_class.Archetypes = Swashbuckler.swashbuckler_class.Archetypes.AddToArray(inspired_blade);
        }
    }
}
