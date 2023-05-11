using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using StrategyList = System.Collections.Generic
    .List<System.Tuple<float, System.Func<System.Random, string>>>;


namespace GalaxyLib
{
    public static class StarName
    {
        public static string Generate(Random random)
        {
            return random.WeightedChoice(strategies)(random);
        }

        public static IEnumerable<string> Generate(Random random, int count)
        {
            HashSet<string> choices = new HashSet<string>();
            while (choices.Count < count) {
                var newChoice = Generate(random);
                if (choices.Add(newChoice)) yield return newChoice;
            }
        }

        public static Tuple<float, Func<Random, string>> Weighted
            (float f, Func<Random, string> s) => Tuple.Create(f, s);


        private static readonly StrategyList strategies = new StrategyList
        {
            Weighted(1.0f, r => "Star"),
        };
    }
}
