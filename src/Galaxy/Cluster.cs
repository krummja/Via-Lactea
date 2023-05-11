using System;
using System.Collections.Generic;
using Godot;


namespace GalaxyLib
{
    public class Cluster : GalaxySpec
    {
        private readonly GalaxySpec basis;
        private readonly float countMean;
        private readonly float countDeviation;
        private readonly float deviationX;
        private readonly float deviationY;
        private readonly float deviationZ;

        public Cluster(
            GalaxySpec basis,
            float countMean = 0.0000025f,
            float countDeviation = 0.000001f,
            float deviationX = 0.0000025f,
            float deviationY = 0.0000025f,
            float deviationZ = 0.0000025f
        ) {
            this.basis = basis;
            this.countMean = countMean;
            this.countDeviation = countDeviation;
            this.deviationX = deviationX;
            this.deviationY = deviationY;
            this.deviationZ = deviationZ;
        }

        protected internal override IEnumerable<Star> Generate(Random random)
        {
            var count = Math.Max(
                0,
                random.NormallyDistributedSingle(
                    countDeviation,
                    countMean
                )
            );

            if (count <= 0) yield break;

            for (int i = 0; i < count; i++) {
                Vector3 center = new Vector3(

                );

                foreach (var star in basis.Generate(random)) {
                    yield return star.Offset(center);
                }
            }
        }
    }
}
