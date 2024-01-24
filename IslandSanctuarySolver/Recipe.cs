using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandSanctuarySolver
{
    /// <summary>
    /// Represents a recipe that can be created in an Island Sanctuary workshop.
    /// Can be combined with other Recipes to create a Preset.
    /// </summary>
    public class Recipe
    {
        #region Public members

        /// <summary>
        /// In-game name of the Recipe.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Hours taken for a mammet to complete the Recipe.
        /// </summary>
        public int Hours { get; set; }

        /// <summary>
        /// Type and quantity of materials used in the Recipe.
        /// This can use a library object because it's not that speed-critical.
        /// </summary>
        public Dictionary<Resource, int> Materials { get; set; }

        /// <summary>
        /// Efficiency bonus categories to which the Recipe belongs.
        /// </summary>
        public HashSet<Category> Categories { get; set; }

        public override bool Equals(object other)
        {
            if(other is Recipe recipe)
            {
                return Name == recipe.Name;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        /// <summary>
        /// The minimum hours used by any Recipe. Used to avoid magic numbers in source code.
        /// </summary>
        public static int MinHours
        {
            get
            {
                if (_minHours == int.MaxValue)
                {
                    foreach (Recipe recipe in AllRecipes)
                    {
                        if (recipe.Hours < _minHours)
                        {
                            _minHours = recipe.Hours;
                        }
                    }
                }

                return _minHours;
            }
        }
        private static int _minHours = int.MaxValue;
        
        /// <summary>
        /// A hardcoded list of all possible Recipes.
        /// This has to be updated whenever a new game version adds Recipes, but so would any kind of input file, so there's little to no benefit to moving this to a config file.
        /// If this project ever gets released to the public, there might be some benefit.
        /// </summary>
        public static List<Recipe> AllRecipes = new List<Recipe>
        {
            new Recipe
            {
                Name = "Isleworks Potion",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Concoctions },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.PalmLeaf, 2 },
                    { Resource.Islewort, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Firesand",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Concoctions, Category.UnburiedTreasures },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Sand, 2 },
                    { Resource.Limestone, 1 },
                    { Resource.Islewort, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Grilled Clam",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Foodstuffs, Category.MarineMerchandise },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Clam, 2 },
                    { Resource.Laver, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Necklace",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Accessories, Category.Woodworks },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Branch, 3 },
                    { Resource.Vine, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Sauerkraut",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.PreservedFood },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Cabbage, 1 },
                    { Resource.RockSalt, 3 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Baked Pumpkin",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Foodstuffs },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Pumpkin, 1 },
                    { Resource.Sap, 3 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Culinary Knife",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Sundries, Category.CreatureCreations },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Claw, 1 },
                    { Resource.PalmLog, 3 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Brush",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Sundries, Category.Woodworks },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Fur , 1 },
                    { Resource.PalmLog , 3 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Boiled Egg",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Foodstuffs, Category.CreatureCreations },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Egg, 1 },
                    { Resource.Laver, 3 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Earrings",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Accessories, Category.CreatureCreations },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Fang, 1 },
                    { Resource.Vine, 3 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Butter",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Ingredients, Category.CreatureCreations },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Milk, 1 },
                    { Resource.RockSalt, 3 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Parsnip Salad",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Foodstuffs },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Parsnip, 2 },
                    { Resource.Islewort, 1 },
                    { Resource.Sap, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Rope",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Sundries, Category.Textiles },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Hemp, 2 },
                    { Resource.Islewort, 1 },
                    { Resource.Vine, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Squid Ink",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Ingredients, Category.MarineMerchandise },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Squid, 2 },
                    { Resource.RockSalt, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Tomato Relish",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Ingredients },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Tomato, 2 },
                    { Resource.Sugarcane, 1 },
                    { Resource.Islewort, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Corn Flakes",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.PreservedFood },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Corn, 2 },
                    { Resource.Sugarcane, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Coconut Juice",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Confections, Category.Concoctions },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Coconut, 2 },
                    { Resource.Sugarcane, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Honey",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Confections, Category.Ingredients },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.BeehiveChip, 2 },
                    { Resource.Sap, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Powdered Paprika",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Ingredients, Category.Concoctions },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Paprika, 2 },
                    { Resource.Islewort, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Isloaf",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Foodstuffs, Category.Concoctions },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Wheat, 2 },
                    { Resource.Islefish, 1},
                    { Resource.RockSalt, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Popoto Salad",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Foodstuffs },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Popoto, 2 },
                    { Resource.Apple, 1 },
                    { Resource.Islewort, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Dressing",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Ingredients },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Onion, 2 },
                    { Resource.Sap, 1 },
                    { Resource.Laver, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Natron",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Sundries, Category.Concoctions },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.EffervescentWater, 2 },
                    { Resource.RockSalt, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Runner Bean Saute",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Foodstuffs },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.RunnerBean, 2 },
                    { Resource.RockSalt, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Brass Serving Dish",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Sundries, Category.Metalworks },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.YellowCopperOre, 2 },
                    { Resource.CopperOre, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Fruit Punch",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Confections },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Watermelon, 1 },
                    { Resource.Isleberry, 1 },
                    { Resource.Apple, 1 },
                    { Resource.Coconut, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Buffalo Bean Salad",
                Hours = 4,
                Categories = new HashSet<Category>{ Category.Foodstuffs, Category.CreatureCreations },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.BuffaloBean, 2 },
                    { Resource.Milk, 2 },
                }
            },

            new Recipe
            {
                Name = "Isleworks Wooden Chair",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Furnishings, Category.Woodworks },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Log, 4 },
                    { Resource.Vine, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Coral Ring",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Accessories, Category.MarineMerchandise },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Coral, 3 },
                    { Resource.Vine, 3 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Barbut",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Attire, Category.Metalworks },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.CopperOre, 3 },
                    { Resource.Sand, 3 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Macuahuitl",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Arms, Category.Woodworks },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.PalmLog, 3 },
                    { Resource.Stone, 3 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Tunic",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Attire, Category.Textiles },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Fleece, 2 },
                    { Resource.Vine, 4 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Hora",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Arms, Category.CreatureCreations },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Carapace, 2},
                    { Resource.Stone, 4},
                }
            },
            new Recipe
            {
                Name = "Isleworks Brick Counter",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Furnishings, Category.UnburiedTreasures },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Clay, 2 },
                    { Resource.Limestone, 2 },
                    { Resource.PalmLog, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Sweet Popoto",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Confections },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Popoto, 2},
                    { Resource.Milk, 1 },
                    { Resource.Sap, 3 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Caramels",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Confections },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Sugarcane, 4 },
                    { Resource.Milk, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Ribbon",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Accessories, Category.Textiles },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.CottonBoll, 2 },
                    { Resource.CopperOre, 2 },
                    { Resource.Vine, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Cavalier's Hat",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Attire, Category.Textiles },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Feather, 2 },
                    { Resource.CottonBoll, 2 },
                    { Resource.Hemp, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Horn",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Sundries, Category.CreatureCreations },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Horn, 2 },
                    { Resource.Clay, 2 },
                    { Resource.Hemp, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Salt Cod",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.PreservedFood, Category.MarineMerchandise },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Islefish, 4 },
                    { Resource.RockSalt, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Essential Draught",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Concoctions, Category.MarineMerchandise },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Jellyfish, 2 },
                    { Resource.PalmLeaf, 2 },
                    { Resource.Laver, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleberry Jam",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Ingredients },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Isleberry, 3 },
                    { Resource.Sugarcane, 2 },
                    { Resource.Sap, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Onion Soup",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Foodstuffs },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Onion, 3 },
                    { Resource.RockSalt, 2 },
                    { Resource.Islewort, 1 },
                }
            },
            new Recipe
            {
                Name = "Islefish Pie",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Confections, Category.MarineMerchandise },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Wheat, 3 },
                    { Resource.Islefish, 2 },
                    { Resource.Sugarcane, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Vegetable Juice",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Concoctions },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Cabbage, 3 },
                    { Resource.Islewort, 2 },
                    { Resource.Laver, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Pumpkin Pudding",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Confections },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Pumpkin, 3 },
                    { Resource.Egg, 1 },
                    { Resource.Milk, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Sheepfluff Rug",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Furnishings, Category.CreatureCreations },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Fleece, 3 },
                    { Resource.CottonBoll, 2 },
                    { Resource.Hemp, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Garden Scythe",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Sundries, Category.Metalworks },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Claw, 3 },
                    { Resource.IronOre, 2 },
                    { Resource.PalmLog, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Dried Flowers",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Sundries, Category.Furnishings },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Isleblooms, 3 },
                    { Resource.Coconut, 2 },
                    { Resource.Sap, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Cawl Cennin",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Concoctions, Category.CreatureCreations },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Leek, 3 },
                    { Resource.Milk, 1 },
                    { Resource.Laver, 3 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Stove",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Furnishings, Category.Metalworks },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Coal, 2 },
                    { Resource.IronOre, 3 },
                    { Resource.Leucogranite, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Fossil Display",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.CreatureCreations, Category.UnburiedTreasures },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Shale, 3 },
                    { Resource.PalmLog, 2 },
                    { Resource.CottonBoll, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Spectacles",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Attire, Category.Sundries },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.MythrilOre, 3 },
                    { Resource.Quartz, 2 },
                    { Resource.CopperOre, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Beet Soup",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Foodstuffs },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Beet, 3 },
                    { Resource.Popoto, 1 },
                    { Resource.Milk, 1 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Imam Bayildi",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Foodstuffs },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Eggplant, 2 },
                    { Resource.Onion, 2 },
                    { Resource.Tomato, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Grinding Wheel",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Sundries },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.HawksEyeSand, 2 },
                    { Resource.MythrilOre, 2 },
                    { Resource.Sand, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Durium Tathlums",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Arms, Category.Metalworks },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.DuriumSand, 2 },
                    { Resource.IronOre, 2 },
                    { Resource.Quartz, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Peperoncino",
                Hours = 6,
                Categories = new HashSet<Category>{ Category.Foodstuffs },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Broccoli, 2 },
                    { Resource.Wheat, 1 },
                    { Resource.RockSalt, 3 },
                }
            },

            new Recipe
            {
                Name = "Bronze Sheep",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Furnishings, Category.Metalworks },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Tinsand, 3 },
                    { Resource.CopperOre, 3 },
                    { Resource.Log, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Growth Formula",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Concoctions },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Alyssum, 2 },
                    { Resource.Islewort, 3 },
                    { Resource.Branch, 3 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Garnet Rapier",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Arms, Category.UnburiedTreasures },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Garnet, 2 },
                    { Resource.CopperOre, 3 },
                    { Resource.Tinsand, 3 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Spruce Round Shield",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Attire, Category.Woodworks },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.SpruceLog, 2 },
                    { Resource.CopperOre, 3 },
                    { Resource.Stone, 3 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Shark Oil",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Sundries, Category.MarineMerchandise },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Hammerhead, 2 },
                    { Resource.Laver, 3 },
                    { Resource.Sap, 3 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Silver Ear Cuffs",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Accessories, Category.Metalworks },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.SilverOre, 2 },
                    { Resource.Tinsand, 3 },
                    { Resource.Coral, 3 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Pickled Radish",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.PreservedFood },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Radish, 4 },
                    { Resource.Apple, 2 },
                    { Resource.Sugarcane, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Iron Axe",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Arms, Category.Metalworks },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.IronOre, 3},
                    { Resource.Log, 3 },
                    { Resource.Sand, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Quartz Ring",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Accessories, Category.UnburiedTreasures },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Quartz, 3 },
                    { Resource.IronOre, 3 },
                    { Resource.Stone, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Porcelain Vase",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Sundries, Category.UnburiedTreasures },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Leucogranite, 3 },
                    { Resource.Quartz, 3 },
                    { Resource.Clay, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Bed",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Furnishings, Category.Textiles },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Fur, 4 },
                    { Resource.CottonBoll, 2 },
                    { Resource.Log, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Scale Fingers",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Attire, Category.CreatureCreations },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Carapace, 4 },
                    { Resource.IronOre, 2 },
                    { Resource.CottonBoll, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Crook",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Arms, Category.Woodworks },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Fang, 4 },
                    { Resource.Quartz, 2 },
                    { Resource.Log, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Coral Sword",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Arms, Category.MarineMerchandise },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Coral, 3 },
                    { Resource.Resin, 3 },
                    { Resource.PalmLog, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Seashine Opal",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.UnburiedTreasures },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.WoodOpal, 4 },
                    { Resource.Sand, 4 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Lantern",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Sundries },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Glimshroom, 3 },
                    { Resource.Quartz, 3 },
                    { Resource.CopperOre, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Bouillabaisse",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Foodstuffs, Category.MarineMerchandise },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.CaveShrimp, 2 },
                    { Resource.Clam, 2 },
                    { Resource.Squid, 2 },
                    { Resource.Tomato, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Bathtub",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Furnishings, Category.UnburiedTreasures },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Marble, 4 },
                    { Resource.Leucogranite, 2 },
                    { Resource.Clay, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Cooling Glass",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.UnburiedTreasures },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Spectrine, 4 },
                    { Resource.Sand, 4 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Pickled Zucchini",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.PreservedFood },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.Zucchini, 4 },
                    { Resource.Laver, 2 },
                    { Resource.Sugarcane, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Gold Hairpin",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Accessories, Category.Metalworks },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.GoldOre, 4 },
                    { Resource.YellowCopperOre, 2 },
                    { Resource.Quartz, 2 },
                }
            },
            new Recipe
            {
                Name = "Mammet of the Cycle Award",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Furnishings },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.CrystalFormation, 2 },
                    { Resource.Spectrine, 2 },
                    { Resource.MythrilOre, 2 },
                    { Resource.Marble, 2 },
                }
            },
            new Recipe
            {
                Name = "Isleworks Sweet Popoto Pie",
                Hours = 8,
                Categories = new HashSet<Category>{ Category.Foodstuffs, Category.Confections },
                Materials = new Dictionary<Resource, int>
                {
                    { Resource.SweetPopoto, 3 },
                    { Resource.Wheat, 1 },
                    { Resource.Egg, 1 },
                    { Resource.Sugarcane, 3 },
                }
            },
        };

        #endregion
    }
}
