using CallOfTheWild;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;

namespace Derring_Do
{
    class FavoredClass
    {
        static LibraryScriptableObject library = Main.library;

        static BlueprintFeature bonus_panache;

        static BlueprintFeature bonus_charmed_life;

        static public void create()
        {
            Main.DebugLog("Creating favored class bonuses");
            createBonusPanache();
            createBonusCharmedLife();

            library.AddAsset(bonus_panache, bonus_panache.AssetGuid);
            library.AddAsset(bonus_charmed_life, bonus_charmed_life.AssetGuid);
        }

        static void createBonusPanache()
        {
            bonus_panache = Helpers.CreateFeature("BonusPanacheSwashbucklerFavoredClassFeature",
                                                  "Bonus Panache Pool",
                                                  "Increase the total number of points in the swashbuckler’s panache pool by 1/4.",
                                                  "89b0082da456424380b5c541f63eee41",
                                                  Swashbuckler.panache.Icon,
                                                  FeatureGroup.None,
                                                  Helpers.Create<IncreaseResourceAmount>(i => { i.Resource = Swashbuckler.panache_resource; i.Value = 1; })
                                                  );
            bonus_panache.Ranks = 5;
            bonus_panache.ReapplyOnLevelUp = true;
        }

        static void createBonusCharmedLife()
        {
            bonus_charmed_life = Helpers.CreateFeature("BonusCharmedLifeSwashbucklerFavoredClassFeature",
                                                       "Bonus Charmed Life Uses",
                                                       "Increase the number of times per day the swashbuckler can use charmed life by 1/4.",
                                                       "57b8c544535f40918881cf4000021a40",
                                                       Swashbuckler.charmed_life.Icon,
                                                       FeatureGroup.None,
                                                       Helpers.Create<IncreaseResourceAmount>(i => { i.Resource = Swashbuckler.charmed_life_resource; i.Value = 1; })
                                                       );
            bonus_charmed_life.Ranks = 5;
            bonus_charmed_life.ReapplyOnLevelUp = true;
        }
    }
}
