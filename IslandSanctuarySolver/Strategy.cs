using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandSanctuarySolver
{
    /// <summary>
    /// Represents a complete list of Presets, thus forming a complete strategy for a full week of Island Sanctuary workshop building.
    /// </summary>
    public static class Strategy
    {
        #region Public members

        #region Very bad coding practice necessary to achieve reasonable runtime performance

        //We have four versions of this function, one for each possible workshop quantity. 
        //This is bad coding practice. It is also much, much faster (in CPU time) than allocating millions of objects or trying to do this in a clever or recursive way.

        /// <summary>
        /// Progressively updates the input form with the best Strategy found so far, given the input stocks (given as an integer array parallel to ResourceValues).
        /// Works for 3 Workshops.
        /// </summary>
        public static void FindBestStrategy3(MainForm form, int[] stocks)
        {
            //Total candidates checked so far
            long candidates = 0;

            //Number of candidates that were better than the last candidate we already showed the user
            long bestCandidates = -1;
            
            //First preset to combine into a Strategy
            Preset preset1;
            
            Preset[] allPresets = Preset.AllPresets;

            //Best strategy cost so far
            long champCost = long.MaxValue;

            //We take an interesting approach here.
            //No inner loop variable can meet or exceed its outer loop's value. This means that early Presets are checked first,
            //and later ones are only introduced when every combination of earlier Presets has been tried.
            //Since Presets are sorted by their likely cost, this means that solutions that are more likely to be ideal are checked earliest.
            //As such, the user is more likely to receive *good* results quickly, even as the program checks for the *ideal* result in the entire search space.
            for (int x = 2; x < allPresets.Length; x++)
            {
                preset1 = allPresets[x];

                //For 3 or more workshops, we multithread the processing to let multiple cores process the data. This, predictably, improves performace by a lot on most gaming PCs.
                Parallel.For(1, x, (y, state) =>
                {
                    //do not allow presets that share recipes, so we spread out demand and resource usage
                    if (allPresets[x].SharedRecipes(allPresets[y]))
                    {
                        //since this is a function with a static value for y, we add allPresets.Length to reflect the fact that we won't be traversing z,
                        //then return (which in this context is equaivalent to "continue" in the y-loop in a conventional nested for loop structure)
                        candidates += allPresets.Length;
                        return;
                    }

                    Preset preset2 = allPresets[y];

                    Preset preset3 = null;

                    //the "mini-champ" is the best Strategy found within this method. It has to beat the main champion (or at least the stale value we pick up here),
                    //and then when we're finished finding the "mini-champ" we see if it beats the current champion. This reduces the amount of time we have to spend
                    //in a lock{} block. (It can mean we find too many "better" strategies due to concurrency, but that's a minimal loss.)
                    long miniChampCost = champCost;
                    for (int z = 0; z < y; z++)
                    {
                        //do not allow presets that share recipes, so we spread out demand and resource usage
                        if (allPresets[z].SharedRecipes(allPresets[y]) || allPresets[z].SharedRecipes(allPresets[x]))
                        {
                            continue;
                        }

                        preset3 = allPresets[z];

                        long newCost = GetCost(stocks, preset1, preset2, preset3);
                        if (newCost < miniChampCost)
                        {
                            miniChampCost = newCost;
                        }
                    }

                    //it is not crucial that this number be exactly correct, so we don't lock{} here - this is a substantial performance boost, so it's worth the inaccuracy in this case
                    candidates += allPresets.Length;

                    //it is also not crucial to update the candidate count exactly as often as this, so we also don't lock{} here
                    if (miniChampCost >= champCost)
                    {
                        if (y % 100 == 0)
                        {
                            form.UpdateCandidateLabel(candidates, bestCandidates);
                        }
                        //we don't want to bother locking if we didn't find anything possibly useful
                        return;
                    }

                    //this, however, *must* be accurate
                    lock (champLock)
                    {
                        //concurrency-safe check in case another thread found something even better
                        if (miniChampCost < champCost)
                        {
                            bestCandidates++;
                            champCost = miniChampCost;
                            form.UpdateStrategyTextBox(GetStrategyText(miniChampCost, preset1, preset2, preset3));
                            form.UpdateCandidateLabel(candidates, bestCandidates);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Progressively updates the input form with the best Strategy found so far, given the input stocks (given as an integer array parallel to ResourceValues).
        /// Works for 1 Workshop.
        /// </summary>
        public static void FindBestStrategy1(MainForm form, int[] stocks)
        {
            //Candidates that were better than the last candidate we already showed the user
            long bestCandidates = -1;
            
            Preset[] allPresets = Preset.AllPresets;
            long champCost = long.MaxValue;

            for (int x = 0; x < allPresets.Length; x++)
            {
                long newCost = GetCost(stocks, allPresets[x]);
                if (newCost < champCost)
                {
                    //Best strategy so far
                    champCost = newCost;
                    bestCandidates++;
                    form.UpdateStrategyTextBox(GetStrategyText(newCost, allPresets[x]));
                    form.UpdateCandidateLabel(x + 1, bestCandidates);
                }
                else if (x % 100 == 0)
                {
                    form.UpdateCandidateLabel(x + 1, bestCandidates);
                }
            }
        }

        /// <summary>
        /// Progressively updates the input form with the best Strategy found so far, given the input stocks (given as an integer array parallel to ResourceValues).
        /// Works for 2 Workshops.
        /// </summary>
        public static void FindBestStrategy2(MainForm form, int[] stocks)
        {
            //Total candidates checked so far
            long candidates = 0;
            //Candidates that were better than the last candidate we already showed the user
            int bestCandidates = -1;

            //First preset to combine into a Strategy
            Preset preset1;
            
            Preset[] allPresets = Preset.AllPresets;
            long champCost = long.MaxValue;

            //We take an interesting approach here.
            //No inner loop variable can meet or exceed its outer loop's value. This means that early Presets are checked first,
            //and later ones are only introduced when every combination of earlier Presets has been tried.
            //Since Presets are sorted by their likely cost, this means that solutions that are more likely to be ideal are checked earliest.
            //As such, the user is more likely to receive *good* results quickly, even as the program checks for the *ideal* result in the entire search space.
            for (int x = 1; x < allPresets.Length; x++)
            {
                preset1 = allPresets[x];

                for (int y = 0; y < x; y++)
                {
                    candidates++;
                    //do not allow presets that share recipes, so we spread out demand and resource usage
                    if (allPresets[x].SharedRecipes(allPresets[y]))
                    {
                        continue;
                    }

                    Preset preset2 = allPresets[y];

                    long newCost = GetCost(stocks, preset1, preset2);

                    if (newCost < champCost)
                    {
                        bestCandidates++;
                        //Best strategy so far
                        champCost = newCost;
                        form.UpdateStrategyTextBox(GetStrategyText(newCost, preset1, preset2));
                        form.UpdateCandidateLabel(candidates, bestCandidates);
                    }
                    else if (y % 100 == 0)
                    {
                        form.UpdateCandidateLabel(candidates, bestCandidates);
                    }
                }
            }
        }

        /// <summary>
        /// Progressively updates the input form with the best Strategy found so far, given the input stocks (given as an integer array parallel to ResourceValues).
        /// Works for 4 Workshops.
        /// </summary>
        public static void FindBestStrategy4(MainForm form, int[] stocks)
        {
            //Total candidates checked so far
            long candidates = 0;
            //Candidates that were better than the last candidate we already showed the user
            int bestCandidates = -1;

            //First preset to combine into a Strategy
            Preset preset1;
            
            Preset[] allPresets = Preset.AllPresets;

            //Best strategy cost so far
            long champCost = long.MaxValue;

            //We take an interesting approach here.
            //No inner loop variable can meet or exceed its outer loop's value. This means that early Presets are checked first,
            //and later ones are only introduced when every combination of earlier Presets has been tried.
            //Since Presets are sorted by their likely cost, this means that solutions that are more likely to be ideal are checked earliest.
            //As such, the user is more likely to receive *good* results quickly, even as the program checks for the *ideal* result in the entire search space.
            for (int x = 3; x < allPresets.Length; x++)
            {
                preset1 = allPresets[x];

                //For 3 or more workshops, we multithread the processing to let multiple cores process the data. This, predictably, improves performace by a lot on most gaming PCs.
                Parallel.For(2, x, (y, state) =>
                {
                    //do not allow presets that share recipes, so we spread out demand and resource usage
                    if (allPresets[x].SharedRecipes(allPresets[y]))
                    {
                        //since this is a function with a static value for y, we add the square of allPresets.Length to reflect the fact that we won't be traversing z or i,
                        //then return (which in this context is equaivalent to "continue" in the y-loop in a conventional nested for loop structure)
                        candidates += allPresets.Length * allPresets.Length;
                        return;
                    }

                    Preset preset2 = allPresets[y];

                    Parallel.For(1, y, (z, state2) =>
                    {
                        if (allPresets[z].SharedRecipes(allPresets[y]) || allPresets[z].SharedRecipes(allPresets[x]))
                        {
                            candidates += allPresets.Length;
                            return;
                        }

                        Preset preset3 = allPresets[z];
                        Preset preset4 = null;

                        //the "mini-champ" is the best Strategy found within this method. It has to beat the main champion (or at least the stale value we pick up here),
                        //and then when we're finished finding the "mini-champ" we see if it beats the current champion. This reduces the amount of time we have to spend
                        //in a lock{} block.
                        long miniChampCost = champCost;

                        for (int i = 0; i < z; i++)
                        {
                            if (allPresets[i].SharedRecipes(allPresets[z]) || allPresets[i].SharedRecipes(allPresets[y]) || allPresets[i].SharedRecipes(allPresets[x]))
                            {
                                continue;
                            }
                            preset4 = allPresets[i];
                            long newCost = GetCost(stocks, preset1, preset2, preset3, preset4);
                            if (newCost < miniChampCost)
                            {
                                miniChampCost = newCost;
                            }
                        }

                        //it is not crucial that this number be exactly correct, so we don't lock{} here - this is a substantial performance boost, so it's worth the inaccuracy in this case
                        candidates += allPresets.Length;

                        //it is also not crucial to update the candidate count exactly as often as this, so we also don't lock{} here
                        if (miniChampCost >= champCost)
                        {
                            if (z % 100 == 0)
                            {
                                form.UpdateCandidateLabel(candidates, bestCandidates);
                            }
                            return;
                        }

                        //this, however, *must* be accurate
                        lock (champLock)
                        {
                            if (miniChampCost < champCost)
                            {
                                bestCandidates++;
                                champCost = miniChampCost;
                                form.UpdateStrategyTextBox(GetStrategyText(miniChampCost, preset1, preset2, preset3, preset4));
                                form.UpdateCandidateLabel(candidates, bestCandidates);
                            }
                        }
                    });
                });
            }
        }
        #endregion

        public static string GetStrategyText(long cost, params Preset[] presets)
        {
            string output = "";
            int shop = 1;

            foreach (Preset preset in presets)
            {
                output += "Workshop " + shop + ":" + Environment.NewLine + preset.ToString() + Environment.NewLine;
                shop++;
            }

            output += "Cost: " + cost;

            return output;
        }
        #endregion

        #region Private members

        /// <summary>
        /// Given the input presets, calculates the total resources used by all of them and outputs them into an array parallel to the ResourceValues array.
        /// This is a static method rather than something we do as part of the constructor because creating a Strategy every time we
        /// want to evaluate this cost is prohibitively time-consuming. This does mean we calculate it twice for each new winning 
        /// Strategy, but the cost of doing so is lower by many orders of magnitude.
        /// </summary>
        private static int[] CalcResources(Preset[] presets)
        {
            int[] output = new int[Program.ResourceValues.Length];

            //The resource cost of a Strategy is, of course, the sum of the costs of its Presets
            foreach (Preset preset in presets)
            {
                for(int x = 0; x < output.Length; x++)
                {
                    output[x] += preset.Resources[x];
                }
            }
            return output;
        }

        private static long GetCost(int[] stocks, params Preset[] presets)
        {
            int[] quantities = CalcResources(presets);

            //spend stocks to satisfy all resources needed (if possible)
            for (int x = 0; x < quantities.Length; x++)
            {
                quantities[x] -= stocks[x];
                if(quantities[x] < 0)
                {
                    quantities[x] = 0;
                }
            }

            //apply givens (items automatically gathered when other items are gathered)
            for(int x = 0; x < Program.Givens.Length; x++)
            {
                Resource[] given = Program.Givens[x];
                quantities[(int)given[1]] -= quantities[(int)given[0]];
            }

            //calculate cost given modified quantities
            long cost = 0;
            for(int x = 0; x < quantities.Length; x++)
            {
                if (quantities[x] <= 0)
                {
                    continue;
                }
                cost += quantities[x] * Program.GatheringCost[x];
            }

            //handle integer underflow
            if(cost < 0)
            {
                return long.MaxValue;
            }

            //do not return zero yet - there's a tiebreaker
            if(cost > 0)
            {
                return cost;
            }

            //zero-cost tiebreaker - prefer large stocks
            int minStock = int.MinValue;
            int[] stratResources = CalcResources(presets);
            for (int x = 0; x < stratResources.Length; x++)
            {
                if(stratResources[x] == 0)
                {
                    continue;
                }
                //we're doing this backwards so we don't have to multiply by -1 later
                int stock = stratResources[x] - stocks[x];
                if(stock > minStock)
                {
                    minStock = stock;
                }
            }

            return minStock;
        }

        /// <summary>
        /// Just a lock object.
        /// </summary>
        private const string champLock = "Y'shtola Rhul";

        #endregion
    }
}
