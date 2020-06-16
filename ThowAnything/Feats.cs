using CallOfTheWild;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using System.Linq;
using static CallOfTheWild.Helpers;

namespace Derring_Do
{
    class Feats
    {
        static LibraryScriptableObject library => Main.library;

        static WeaponCategory[] throwable_weapon_categories = new WeaponCategory[] { WeaponCategory.Bomb, WeaponCategory.Dagger, WeaponCategory.Dart, WeaponCategory.Javelin, WeaponCategory.Shuriken, WeaponCategory.Starknife, WeaponCategory.ThrowingAxe };

        static public BlueprintFeatureSelection closequarters_thrower;

        public static void create()
        {
            createPointBlankMaster();
            createCloseQuartersThrower();
        }

        public static void createPointBlankMaster()
        {
            var point_blank_master = library.Get<BlueprintParametrizedFeature>("05a3b543b0a0a0346a5061e90f293f0b");
            var weapon_spec_dagger = library.Get<BlueprintFeature>("578a6db7e82d48d40b2c570b99b8abfb");
            var weapon_spec_starknife = library.Get<BlueprintFeature>("31470b17e8446ae4ea0dacd6c5817d86");

            var point_blank_shortbow = library.Get<BlueprintFeature>("2b9de25e23424f24390fa7a96ab10ebc");

            var point_blank_dagger = library.CopyAndAdd<BlueprintFeature>("2b9de25e23424f24390fa7a96ab10ebc", "PointBlankMasterDagger", "767ca730cb1f4901a8d073da32d99f5d");
            point_blank_dagger.SetName("Point-Blank Master (Dagger)");
            point_blank_dagger.ReplaceComponent<PrerequisiteFeature>(Create<PrerequisiteFeature>(p => p.Feature = weapon_spec_dagger));
            point_blank_dagger.ReplaceComponent<PointBlankMaster>(Create<PointBlankMaster>(p => p.Category = WeaponCategory.Dagger));

            var point_blank_starknife = library.CopyAndAdd<BlueprintFeature>("2b9de25e23424f24390fa7a96ab10ebc", "PointBlankMasteStarknife", "bd45589795064a82ab65c1d5e78f4ec6");
            point_blank_starknife.SetName("Point-Blank Master (Starknife)");
            point_blank_starknife.ReplaceComponent<PrerequisiteFeature>(Create<PrerequisiteFeature>(p => p.Feature = weapon_spec_starknife));
            point_blank_starknife.ReplaceComponent<PointBlankMaster>(Create<PointBlankMaster>(p => p.Category = WeaponCategory.Starknife));
        }

        public static void createCloseQuartersThrower()
        {
            var dodge = library.Get<BlueprintFeature>("97e216dbb46ae3c4faef90cf6bbe6fd5");
            var weapon_focus = library.Get<BlueprintParametrizedFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e");
            var seed_guid = "f66948aff7414962b99483d1a3c33733";

            closequarters_thrower = CreateFeatureSelection("CloseQuartersThrowerFeature",
                                                           "Close-Quarters Thrower",
                                                           "Choose a type of thrown weapon. You do not provoke attacks of opportunity for making ranged attacks using the selected weapon. If you are an alchemist, and you select this feat and choose alchemist bombs, you do not provoke attacks of opportunity for the process of drawing components of, creating, and throwing a bomb.",
                                                           "b6508aa910f14933af1aea3b82dfaf29",
                                                           null,
                                                           FeatureGroup.Feat,
                                                           Helpers.PrerequisiteStatValue(StatType.Dexterity, 13),
                                                           Helpers.PrerequisiteFeature(dodge)
                                                           );

            foreach (var category in throwable_weapon_categories)
            {
                var guid = library.GetAllBlueprints().OfType<BlueprintWeaponType>().Where(b => b.Category == category).First().AssetGuid;

                var feature = CreateFeature(category.ToString() + "CloseQuartersThrowerFeatureSelection",
                                            "Close-Quarters Thrower " + "(" + category.ToString() + ")",
                                            closequarters_thrower.Description,
                                            MergeIds(guid, seed_guid),
                                            closequarters_thrower.Icon,
                                            FeatureGroup.None,
                                            Create<PointBlankMaster>(p => p.Category = category),
                                            Common.createPrerequisiteParametrizedFeatureWeapon(weapon_focus, category)
                                            );

                closequarters_thrower.AllFeatures = closequarters_thrower.AllFeatures.AddToArray(feature);
            }

            closequarters_thrower.Groups = closequarters_thrower.Groups.AddToArray(FeatureGroup.CombatFeat);
            library.AddCombatFeats(closequarters_thrower);
        }
    }
}
