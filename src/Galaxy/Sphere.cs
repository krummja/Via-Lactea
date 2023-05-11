using System;
using System.Collections.Generic;
using Godot;


namespace GalaxyLib
{
    public class Sphere : GalaxySpec
    {
        private readonly float size;
        private readonly float densityMean;
        private readonly float densityDeviation;
        private readonly float deviationX;
        private readonly float deviationY;
        private readonly float deviationZ;
        private readonly float starSizeDeviation;

        public Sphere(
            float size,
            float densityMean = 0.0000025f,
            float densityDeviation = 0.000001f,
            float deviationX = 0.0000025f,
            float deviationY = 0.0000025f,
            float deviationZ = 0.0000025f,
            float starSizeDeviation = 0.15f
        ) {
            this.size = size;
            this.densityMean = densityMean;
            this.densityDeviation = densityDeviation;
            this.deviationX = deviationX;
            this.deviationY = deviationY;
            this.deviationZ = deviationZ;
            this.starSizeDeviation = starSizeDeviation;
        }

        protected internal override IEnumerable<Star> Generate(Random random)
        {
            float density = Math.Max(
                0,
                random.NormallyDistributedSingle(
                    densityDeviation,
                    densityMean
                )
            );

            float countMax = Math.Max(0, (int) (size * size * size * density));

            if (countMax <= 0) yield break;

            int count = random.Next((int) countMax);

            for (int i = 0; i < count; i++) {
                Vector3 pos = new Vector3(
                    random.NormallyDistributedSingle(deviationX * size, 0),
                    random.NormallyDistributedSingle(deviationY * size, 0),
                    random.NormallyDistributedSingle(deviationZ * size, 0)
                );

                float d = pos.Length() / size;
                float m = d * 2000 + (1 - d) * 15000;
                float t = random.NormallyDistributedSingle(4000, m, 1000, 40000);
                float s = random.NormallyDistributedSingle(starSizeDeviation, 1.0f);

                yield return new Star(pos, StarName.Generate(random), s, t);
            }
        }
    }
}
