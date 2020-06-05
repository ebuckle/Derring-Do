using CallOfTheWild;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derring_Do
{
    public class SwashbucklerFeats
    {
        static LibraryScriptableObject library => Main.library;

        static BlueprintFeature extra_panache;

        static public void createFeats()
        {
            var panache = Swashbuckler.panache;
            var panache_resource = Swashbuckler.panache_resource;

            extra_panache = Helpers.CreateFeature("ExtraPanacheFeature",
                                                  "Extra Panache",
                                                  "You gain two more panache points at the start of each day, and your maximum panache increases by two.\n"
                                                  + "If you have levels in the swashbuckler class, you can take this feat multiple times. Its effects stack.",
                                                  "75d9556fb68f460a9171e72eeea81115",
                                                  panache.Icon,
                                                  FeatureGroup.Feat,
                                                  Helpers.Create<IncreaseResourceAmount>(i => { i.Resource = panache_resource; i.Value = 2; }),
                                                  Helpers.PrerequisiteFeature(panache)
                                                  );
            extra_panache.Ranks = 10;

            //TODO More feats

            library.AddFeats(extra_panache);
        }
    }
}
