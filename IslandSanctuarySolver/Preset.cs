using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandSanctuarySolver
{
    /// <summary>
    /// Represents a set of tasks for a single workshop on the Island Sanctuary. n Presets (where n = the number of workshops) form a Strategy.
    /// </summary>
    public class Preset : IComparable
    {
        #region Public members

        /// <summary>
        /// An array representation of all of the resource quantities used in this preset to supply its recipes.
        /// Uses the values of the Program.Resource enum as indices.
        /// Again, this could be memoized, but is not for performance reasons.
        /// </summary>
        public int[] Resources;

        public static Preset[] AllPresets;

        /// <summary>
        /// Returns true if and only if this Preset and the Preset supplied as a parameter share any Recipes.
        /// </summary>
        public bool SharedRecipes(Preset other)
        {
            for (int x = 0; x < _recipesUsed.Length; x++)
            {
                for (int y = 0; y < other._recipesUsed.Length; y++)
                {
                    if (_recipesUsed[x] == other._recipesUsed[y])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Creates a complete array of all valid Presets, given certain constraints designed to maximize the value of each Preset.
        /// </summary>
        /// <param name="stocks">An array representing a complete list of Isleventory stocks.</param>
        public static Preset[] GetAllPresets(int[] stocks)
        {
            List<Preset> output = new List<Preset>();

            foreach (Recipe recipe in Recipe.AllRecipes)
            {
                //always start Presets with a 4-hour Recipe to minimize lost income from efficiency bonuses
                if (recipe.Hours == Recipe.MinHours)
                {
                    //recursively create all valid Presets starting with this Recipe
                    output.AddRange(GetPresets(stocks, recipe));
                }
            }

            //sort Recipes according to their likely usefulness so we get the likely best results early in processing
            output.Sort();

            return output.ToArray();
        }

        /// <summary>
        /// Sorts Presets according to how difficult they are likely to be to use, based on current stocks and gathering costs.
        /// Easy Presets come first.
        /// </summary>
        public int CompareTo(object obj)
        {
            if (!(obj is Preset other))
            {
                return 0;
            }

            //minimize gathering cost (which is always 0 or higher)
            long diff = LikelyCost - other.LikelyCost;

            if (diff < 0)
            {
                return -1;
            }
            if (diff > 0)
            {
                return 1;
            }

            //maximize minimum remaining stock after item creation
            diff = other.RemainingStocks - RemainingStocks;

            if (diff < 0)
            {
                return -1;
            }
            if (diff > 0)
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Displayed on the UI when this Preset is part of a winning Strategy.
        /// </summary>
        public override string ToString()
        {
            string output = "";

            foreach (Recipe recipe in _recipes)
            {
                output += recipe.Name + " (" + recipe.Hours + ")" + Environment.NewLine;
            }

            return output;
        }

        #endregion

        #region Private members

        /// <summary>
        /// A reference to the total stocks in the Isleventory, used to calculate the likely cost of this Recipe.
        /// </summary>
        private readonly int[] _stocks;

        /// <summary>
        /// An array of the integer indices of each recipe used by this Preset.
        /// Not to be confused with _recipes, which is a List of Recipes (and therefore slower to access).
        /// This would normally want to be a memoized property, but this would impact performance, so we work around it in this case.
        /// </summary>
        private int[] _recipesUsed;

        /// <summary>
        /// Initializes _recipesUsed. Called during creation of the Preset list, after the recipes have been determined for a given Preset.
        /// (This is done because a newly-constructed Preset can be incomplete, and used to recursively create complete Presets, 
        /// or it can already exist in the set and we don't want to waste processing time creating it.)
        /// </summary>
        private void SetRecipesUsed()
        {
            _recipesUsed = new int[_recipes.Count];
            for (int x = 0; x < _recipesUsed.Length; x++)
            {
                for(int y = 0; y < Recipe.AllRecipes.Count; y++)
                {
                    if(_recipes[x] == Recipe.AllRecipes[y])
                    {
                        _recipesUsed[x] = y;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Constructs a new Preset, but does not initialize its internal tracking of component Recipes. Call SetRecipesUsed() before using this Preset if this Preset is complete.
        /// (This is done because a newly-constructed Preset can be incomplete, and used to recursively create complete Presets, 
        /// or it can already exist in the set and we don't want to waste processing time creating it.)
        /// </summary>
        /// <param name="stocks">An array representing a complete list of Isleventory stocks.</param>
        /// <param name="recipes">An array of all Recipes in the Preset.</param>
        private Preset(int[] stocks, params Recipe[] recipes)
        {
            _recipes = new List<Recipe>(recipes);
            _stocks = stocks;

            Resources = CalcResources();
        }
        
        /// <summary>
        /// Returns the minimum amount of any used stock that will remain after the Preset is used.
        /// </summary>
        private long RemainingStocks
        {
            get
            {
                if (_remStocks == null)
                {
                    _remStocks = long.MaxValue;
                    for (int x = 0; x < Resources.Length; x++)
                    {
                        if(Resources[x] == 0)
                        {
                            continue;
                        }
                        long stock = _stocks[x] - Resources[x];
                        if(stock < _remStocks)
                        {
                            _remStocks = stock;
                        }
                    }
                }

                return (long)_remStocks;
            }
        }
        private long? _remStocks = null;

        /// <summary>
        /// Returns an approximation of the preset's likely gathering cost, based on the current stock of materials.
        /// (We cannot calcualte this exactly because other Presets may use the same resources.)
        /// </summary>
        private long LikelyCost
        {
            get
            {
                if (_likelyCost == null)
                {
                    _likelyCost = 0;

                    for (int x = 0; x < Resources.Length; x++)
                    {
                        //this is a little weird, but the idea is that "quantity" is the number of resources we're likely (or certain) to actually need
                        //so we subtract the stock from our needed resources, not vice versa
                        long quantity = Resources[x] - _stocks[x];
                        if (quantity > 0)
                        {
                            _likelyCost += quantity * Program.GatheringCost[x];
                            continue;
                        }

                        //for stocks that are running low, we check higher quantities but penalize them less - the idea is that other presets may want the same stuff, but probably not
                        quantity += Resources[x];
                        if (quantity > 0)
                        {
                            _likelyCost += quantity * Program.GatheringCost[x] / 2;
                            continue;
                        }

                        quantity += Resources[x];
                        if (quantity > 0)
                        {
                            _likelyCost += quantity * Program.GatheringCost[x] / 4;
                            continue;
                        }

                        quantity += Resources[x];
                        if (quantity > 0)
                        {
                            _likelyCost += quantity * Program.GatheringCost[x] / 8;
                            continue;
                        }
                    }
                }

                return (long)_likelyCost;
            }
        }
        private long? _likelyCost = null;

        /// <summary>
        /// A hash containing a prime-factorization representation of created presets, used to avoid creating duplicates.
        /// We use this method because it saves time and memory when creating the preset list (especially compared to a HashSet of Presets).
        /// </summary>
        private static readonly HashSet<long> presetHash = new HashSet<long>();

        /// <summary>
        /// Recursive case of GetAllPresets(int[]) and itself. Gets all valid presets beginning with the recipe(s) in inputRecipes, using stocks to determine costs.
        /// </summary>
        /// <param name="stocks">An array representing a complete list of Isleventory stocks.</param>
        /// <param name="inputRecipes">The recipe(s) to include in all returned presets.</param>
        /// <returns></returns>
        private static IEnumerable<Preset> GetPresets(int[] stocks, params Recipe[] inputRecipes)
        {
            //Create a hypothetical preset using the input recipes, so we can analyze the hours used more easily.
            //The object creation here is expensive, but in practice the time lost is minial (as opposed to with Strategies).
            Preset preset = new Preset(stocks, inputRecipes);
            
            //24-hour Presets should be added to the preset list
            if (preset.Hours == 24)
            {
                //but only if they aren't there already
                if(!presetHash.Contains(preset.ResourceHash))
                {
                    preset.SetRecipesUsed();
                    presetHash.Add(preset.ResourceHash);
                    yield return preset;
                }
                yield break;
            }

            //There's no way to complete a preset with more than 20 hours but less than 24, since no recipe is that short
            if (preset.Hours > 24 - Recipe.MinHours)
            {
                yield break;
            }

            //get the last recipe in the input sequence, so we can do things like make sure we get efficiency bonuses
            Recipe lastRecipe = inputRecipes[inputRecipes.Length - 1];

            foreach (Recipe testRecipe in Recipe.AllRecipes)
            {
                //don't repeat recipes - spread out demand for products and resource usage
                if(inputRecipes.Contains(testRecipe))
                {
                    continue;
                }

                //only build recipes that give efficiency bonuses
                if (testRecipe.Categories.Intersect(lastRecipe.Categories).Count() == 0)
                {
                    continue;
                }

                //don't use 4-hour recipes except to cap off a day (and at the start)
                if(preset.Hours > 0 && preset.Hours < (24 - Recipe.MinHours) && testRecipe.Hours == Recipe.MinHours)
                {
                    continue;
                }
                
                //create a new prefix recipe array for recursive purposes, appending the test recipe to the existing input list
                Recipe[] addedRecipes = new Recipe[inputRecipes.Length + 1];
                for (int x = 0; x < inputRecipes.Length; x++)
                {
                    addedRecipes[x] = inputRecipes[x];
                }
                addedRecipes[addedRecipes.Length - 1] = testRecipe;

                //recursively return all presets yielded by our latest input list
                //I wish there were some kind of "yield range" functionality in C#, but I am aware of no such thing
                foreach (Preset returnedPreset in GetPresets(stocks, addedRecipes))
                {
                    yield return returnedPreset;
                }
            }
        }

        /// <summary>
        /// A List of the Recipes used in this preset. Not to be confused with _recipesUsed, which uses array indices to represent the same thing as a speed optimization.
        /// This object is still useful, however, when we are creating and evaluating recipes, so it stays.
        /// </summary>
        private readonly List<Recipe> _recipes;

        /// <summary>
        /// The total number of hours consumed by this Preset. Cannot be memoized because the value changes during preset creation.
        /// Also, its value is always 24 *after* preset creation, so by that point there is no need.
        /// </summary>
        private int Hours
        {
            get
            {
                int hours = 0;
                foreach (Recipe recipe in _recipes)
                {
                    hours += recipe.Hours;
                }
                return hours;
            }
        }

        /// <summary>
        /// Calculates the values of the Resources array.
        /// </summary>
        /// <returns></returns>
        private int[] CalcResources()
        {
            int[] output = new int[Program.ResourceValues.Length];
            foreach (Recipe recipe in _recipes)
            {
                foreach (Resource key in recipe.Materials.Keys)
                {
                    output[(int)key] += recipe.Materials[key];
                }
            }
            return output;
        }

        /// <summary>
        /// Comparing the values of different Presets takes a long time, so instead each one is assigned a hash based on a prime factorization of its resource usage.
        /// This is that hash.
        /// </summary>
        private long ResourceHash
        {
            get
            {
                if(_resourceHash == -1)
                {
                    _resourceHash = 0;
                    for(int x = 0; x < Resources.Length; x++)
                    {
                        //start with the 20th prime to avoid a situation where the resource amount is a prime
                        //since resource amounts are never more than 20 or so, this is a safe buffer
                        _resourceHash += Primes.GetPrime(x + 20) * Resources[x];
                    }
                }

                return _resourceHash;
            }
        }
        private long _resourceHash = -1;

        #endregion
    }
}
