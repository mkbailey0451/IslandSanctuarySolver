using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IslandSanctuarySolver
{
    /// <summary>
    /// An enumeration of all resources found on the island, in isleventory order. Updated manually as needed.
    /// We really can't put this in a config file, because enums aren't possible to create at runtime, and doing it this way gives us several important optimizations.
    /// </summary>
    public enum Resource
    {
        PalmLeaf,
        Branch,
        Stone,
        Clam,
        Laver,
        Coral,
        Islewort,
        Sand,
        Vine,
        Sap,
        Apple,
        Log,
        PalmLog,
        CopperOre,
        Limestone,
        RockSalt,
        Clay,
        Tinsand,
        Sugarcane,
        CottonBoll,
        Hemp,
        Islefish,
        Squid,
        Jellyfish,
        IronOre,
        Quartz,
        Leucogranite,
        Isleblooms,
        Resin,
        Coconut,
        BeehiveChip,
        WoodOpal,
        Coal,
        Glimshroom,
        EffervescentWater,
        Shale,
        Marble,
        MythrilOre,
        Spectrine,
        DuriumSand,
        YellowCopperOre, //don't ask me what on Etheirys this is
        GoldOre,
        HawksEyeSand,
        CrystalFormation,

        Alyssum,
        Garnet,
        SpruceLog,
        Hammerhead,
        SilverOre,
        CaveShrimp,

        //produce
        Popoto,
        Cabbage,
        Isleberry,
        Pumpkin,
        Onion,
        Tomato,
        Wheat,
        Corn,
        Parsnip,
        Radish,
        Paprika,
        Leek,
        RunnerBean,
        Beet,
        Eggplant,
        Zucchini,
        Watermelon,
        SweetPopoto,
        Broccoli,
        BuffaloBean,

        //leavings
        Fleece,
        Claw,
        Fur,
        Feather,
        Egg,
        Carapace,
        Fang,
        Horn,
        Milk,
    }

    /// <summary>
    /// Categories of Recipes, used to determine efficiency bonuses.
    /// </summary>
    public enum Category
    {
        PreservedFood,
        Attire,
        Foodstuffs,
        Confections,
        Sundries,
        Furnishings,
        Arms,
        Concoctions,
        Ingredients,
        Accessories,
        Metalworks,
        Woodworks,
        Textiles,
        CreatureCreations,
        MarineMerchandise,
        UnburiedTreasures,
    }

    public static class Program
    {
        public static Resource[] ResourceValues = (Resource[])Enum.GetValues(typeof(Resource));

        /// <summary>
        /// "Givens" are resources gathered together automatically. For example, gathering Marble always also yields Limestone and Stone.
        /// These are used to determine gathering difficulty - it's no more difficult to gather 5 marble and 5 limestone than to gather 5 marble.
        /// </summary>
        public static Resource[][] Givens = new Resource[][]
        {
            //order is important for these - go from most basic to least
            new Resource[]{ Resource.PalmLeaf, Resource.PalmLog },
            new Resource[]{ Resource.Coconut, Resource.PalmLeaf },

            new Resource[]{ Resource.Apple, Resource.Vine },
            new Resource[]{ Resource.BeehiveChip, Resource.Apple },

            new Resource[]{ Resource.Sugarcane, Resource.Vine },

            new Resource[]{ Resource.Branch, Resource.Log },
            new Resource[]{ Resource.Resin, Resource.Branch },

            new Resource[]{ Resource.Sap, Resource.Log },
            new Resource[]{ Resource.WoodOpal, Resource.Sap },

            new Resource[]{ Resource.Hemp, Resource.Islewort },

            new Resource[]{ Resource.CottonBoll, Resource.Islewort },

            new Resource[]{ Resource.Marble, Resource.Limestone },
            new Resource[]{ Resource.Limestone, Resource.Stone },

            new Resource[]{ Resource.MythrilOre, Resource.CopperOre },
            new Resource[]{ Resource.CopperOre, Resource.Stone },

            new Resource[]{ Resource.Quartz, Resource.Stone },

            new Resource[]{ Resource.DuriumSand, Resource.IronOre },
            new Resource[]{ Resource.IronOre, Resource.Stone },

            new Resource[]{ Resource.Leucogranite, Resource.Stone },

            new Resource[]{ Resource.Clay, Resource.Sand },

            new Resource[]{ Resource.Tinsand, Resource.Sand },

            new Resource[]{ Resource.Clam, Resource.Islefish },

            new Resource[]{ Resource.Squid, Resource.Laver },

            new Resource[]{ Resource.Jellyfish, Resource.Coral },

            new Resource[]{ Resource.Shale, Resource.Coal },
            new Resource[]{ Resource.Coal, Resource.Stone },

            new Resource[]{ Resource.EffervescentWater, Resource.Spectrine },
            new Resource[]{ Resource.Spectrine, Resource.Stone },
            
            new Resource[]{ Resource.YellowCopperOre, Resource.GoldOre },
            new Resource[]{ Resource.GoldOre, Resource.Stone },

            new Resource[]{ Resource.HawksEyeSand, Resource.CrystalFormation },

            //isleblooms have no givens

            //TODO: only complete as of 6.35(?)
        };

        /// <summary>
        /// An array of gathering "costs" for each resource, with indices equal to the enum values.
        /// A gathering "cost" is an estimation of how annoying it is to gather a given material.
        /// </summary>
        public static long[] GatheringCost = GetAllGatheringCosts();
        //TODO: Maybe move gathering costs to a config file - not needed unless/until public release, but people do have Opinions on this stuff

        private static long[] GetAllGatheringCosts()
        {
            long[] output = new long[ResourceValues.Length];
            for(int x = 0; x < output.Length; x++)
            {
                output[x] = GetGatheringCost(x);
            }
            return output;
        }

        private static long GetGatheringCost(int resource)
        {
            //TODO: Adjust weights, add resources
            switch (resource)
            {
                case (int)Resource.PalmLeaf: return 225;
                case (int)Resource.Branch: return 225;
                case (int)Resource.Stone: return 200;
                case (int)Resource.Clam: return 950;
                case (int)Resource.Laver: return 900;
                case (int)Resource.Coral: return 925;
                case (int)Resource.Islewort: return 200;
                case (int)Resource.Sand: return 200;
                case (int)Resource.Vine: return 100;
                case (int)Resource.Sap: return 225;
                case (int)Resource.Apple: return 215;
                case (int)Resource.Log: return 225;
                case (int)Resource.PalmLog: return 225;
                case (int)Resource.CopperOre: return 500;
                case (int)Resource.Limestone: return 200;
                case (int)Resource.RockSalt: return 213;
                case (int)Resource.Clay: return 200;
                case (int)Resource.Tinsand: return 225;
                case (int)Resource.Sugarcane: return 100;
                case (int)Resource.CottonBoll: return 200;
                case (int)Resource.Hemp: return 225;
                case (int)Resource.Islefish: return 950;
                case (int)Resource.Squid: return 900;
                case (int)Resource.Jellyfish: return 925;
                case (int)Resource.IronOre: return 280;
                case (int)Resource.Quartz: return 250;
                case (int)Resource.Leucogranite: return 250;
                case (int)Resource.Isleblooms: return 510;
                case (int)Resource.Resin: return 225;
                case (int)Resource.Coconut: return 225;
                case (int)Resource.BeehiveChip: return 215;
                case (int)Resource.WoodOpal: return 225;
                case (int)Resource.Coal: return 1000;
                case (int)Resource.Glimshroom: return 1200;
                case (int)Resource.EffervescentWater: return 1000;
                case (int)Resource.Shale: return 1000;
                case (int)Resource.Marble: return 200;
                case (int)Resource.MythrilOre: return 500;
                case (int)Resource.Spectrine: return 1000;
                case (int)Resource.DuriumSand: return 280;
                case (int)Resource.YellowCopperOre: return 350;
                case (int)Resource.GoldOre: return 350;
                case (int)Resource.HawksEyeSand: return 350;
                case (int)Resource.CrystalFormation: return 350;

                default: return 1000000000;
            }
        }

        //TODO Traveling Salesman
        public static Dictionary<Resource, string> GatheringSpots = new Dictionary<Resource, string>
        {
            { Resource.PalmLeaf, "" },
            { Resource.Branch, "" },
            { Resource.Stone, "" },
            { Resource.Clam, "" },
            { Resource.Laver, "" },
            { Resource.Coral, "" },
            { Resource.Islewort, "" },
            { Resource.Sand, "" },
            { Resource.Vine, "18.0, 17.9, 0.6" },
            { Resource.Sap, "" },
            { Resource.Apple, "" },
            { Resource.Log, "" },
            { Resource.PalmLog, "" },
            { Resource.CopperOre, "" },
            { Resource.Limestone, "" },
            { Resource.RockSalt, "" },
            { Resource.Clay, "" },
            { Resource.Tinsand, "" },
            { Resource.Sugarcane, "18.0, 17.9, 0.6" },
            { Resource.CottonBoll, "" },
            { Resource.Hemp, "" },
            { Resource.Islefish, "" },
            { Resource.Squid, "" },
            { Resource.Jellyfish, "" },
            { Resource.IronOre, "" },
            { Resource.Quartz, "" },
            { Resource.Leucogranite, "" },
            { Resource.Isleblooms, "" },
            { Resource.Resin, "" },
            { Resource.Coconut, "" },
            { Resource.BeehiveChip, "" },
            { Resource.WoodOpal, "" },
            { Resource.Alyssum, null },
            { Resource.Garnet, null },
            { Resource.SpruceLog, null },
            { Resource.Hammerhead, null },
            { Resource.SilverOre, null },
        };

        //TODO: Commonly gathered together resources (tinsand and limestone, for example)?
        

        

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
