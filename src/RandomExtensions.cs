using System;
using System.Collections.Generic;
using System.Linq;


namespace GalaxyLib
{
    static class RandomExtensions
    {
        public static IEnumerable<float> NormallyDistributedSingles(
            this Random random,
            float standardDeviation,
            float mean
        ) {
            double u1 = random.NextDouble();
            double u2 = random.NextDouble();

            double x1 = Math.Sqrt(-2.0 * Math.Log(u1));
            double x2 = 2.0 *  Math.PI * u2;
            double z1 = x1 * Math.Sin(x2);
            yield return (float) (z1 * standardDeviation + mean);

            double z2 = x1 * Math.Cos(x2);
            yield return (float) (z2 * standardDeviation + mean);
        }

        public static float NormallyDistributedSingle(
            this Random random,
            float standardDeviation,
            float mean
        ) {
            double u1 = random.NextDouble();
            double u2 = random.NextDouble();

            double x1 = Math.Sqrt(-2.0 * Math.Log(u1));
            double x2 = 2.0 *  Math.PI * u2;
            double z1 = x1 * Math.Sin(x2);

            return (float) (z1 * standardDeviation + mean);
        }

        public static IEnumerable<float> NormallyDistributedSingles(
            this Random random,
            float standardDeviation,
            float mean,
            float min,
            float max
        ) {
            while (true)
                yield return random.NormallyDistributedSingle(
                    standardDeviation,
                    mean,
                    min,
                    max
                );
        }

        public static float NormallyDistributedSingle(
            this Random random,
            float standardDeviation,
            float mean,
            float min,
            float max
        ) {
            float nMin = (min - mean) / standardDeviation;
            float nMax = (max - mean) / standardDeviation;
            float nRange = nMax - nMin;
            float nMaxSq = nMax * nMax;
            float nMinSq = nMin * nMin;
            float subFrom = nMinSq;

            if (nMin < 0 && 0 < nMax) subFrom = 0;
            else if (nMax < 0) subFrom = nMaxSq;

            double sigma = 0.0;
            double u;
            float z;

            do {
                z = nRange * (float) random.NextDouble() + nMin;
                sigma = Math.Exp((subFrom - z * z) / 2);
                u = random.NextDouble();
            } while (u > sigma);

            return z * standardDeviation + mean;
        }

        public static T WeightedChoice<T>(
            this Random random,
            List<Tuple<float, T>> choices
        ) {
            var totalWeight = choices.Sum(x => x.Item1);
            var choice = random.NextDouble() * totalWeight;

            return choices.First(c => {
                choice -= c.Item1;
                return (choice <= 0);
            }).Item2;
        }
    }
}
