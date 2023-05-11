using System;
using System.Collections.Generic;
using Godot;

namespace GalaxyLib
{
    public class Spiral : GalaxySpec
    {
        public int Size { get; set; }
        public int Spacing { get; set; }

        public int MinimumArms { get; set; }
        public int MaximumArms { get; set; }

        public float ClusterCountDeviation { get; set; }
        public float ClusterCenterDeviation { get; set; }

        public float MinArmClusterScale { get; set; }
        public float ArmClusterScaleDeviation { get; set; }
        public float MaxArmClusterScale { get; set; }

        public float Swirl { get; set; }

        public float CenterClusterScale { get; set; }
        public float CenterClusterDensityMean { get; set; }
        public float CenterClusterDensityDeviation { get; set; }
        public float CenterClusterSizeDeviation { get; set; }

        public float CenterClusterPositionDeviation { get; set; }
        public float CenterClusterCountDeviation { get; set; }
        public float CenterClusterCountMean { get; set; }

        public float CentralVoidSizeMean { get; set; }
        public float CentralVoidSizeDeviation { get; set; }

        public float StarSizeDeviation { get; set; }

        public int CountStars { get; private set; }

        public Spiral()
        {
            Size = 750;
            Spacing = 5;

            MinimumArms = 3;
            MaximumArms = 7;

            ClusterCountDeviation = 0.35f;
            ClusterCenterDeviation = 0.2f;

            MinArmClusterScale = 0.02f;
            ArmClusterScaleDeviation = 0.02f;
            MaxArmClusterScale = 0.1f;

            Swirl = (float) Math.PI * 4;

            CenterClusterScale = 0.19f;
            CenterClusterDensityMean = 0.00005f;
            CenterClusterDensityDeviation = 0.000005f;
            CenterClusterSizeDeviation = 0.00125f;

            CenterClusterPositionDeviation = 20;
            CenterClusterCountDeviation = 3;
            CenterClusterCountMean = 0.075f;

            CentralVoidSizeMean = 25;
            CentralVoidSizeDeviation = 7;
        }

        protected internal override IEnumerable<Star> Generate(Random random)
        {
            GD.Print("Generating");

            float centralVoidSize = random.NormallyDistributedSingle(
                CentralVoidSizeDeviation,
                CentralVoidSizeMean
            );
            if (centralVoidSize < 0) centralVoidSize = 0;

            float centralVoidSizeSqr = centralVoidSize * centralVoidSize;

            GD.Print("Generating Galactic Spiral Arms");

            foreach (Star star in GenerateArms(random)) {
                if (star.Position.LengthSquared() > centralVoidSizeSqr) {
                    CountStars++;
                    yield return star;
                }
            }

            GD.Print("Generating Galactic Core");

            foreach (Star star in GenerateCenter(random)) {
                if (star.Position.LengthSquared() > centralVoidSizeSqr) {
                    CountStars++;
                    yield return star;
                }
            }

            GD.Print("Generating Background Stars");

            foreach (Star star in GenerateBackgroundStars(random)) {
                if (star.Position.LengthSquared() > centralVoidSizeSqr) {
                    CountStars++;
                    yield return star;
                }
            }

            GD.Print(CountStars);
        }

        private IEnumerable<Star> GenerateBackgroundStars(Random random)
        {
            Sphere sphere = new Sphere(
                Size,
                densityMean: 0.000001f,
                densityDeviation: 0.0000001f,
                deviationX: 0.35f,
                deviationY: 0.125f,
                deviationZ: 0.35f,
                starSizeDeviation: 0.15f
            );

            return sphere.Generate(random);
        }

        private IEnumerable<Star> GenerateCenter(Random random)
        {
            var sphere = new Sphere(
                size: Size * CenterClusterScale,
                densityMean: CenterClusterDensityMean,
                densityDeviation: CenterClusterDensityDeviation,
                deviationX: CenterClusterScale,
                deviationY: CenterClusterScale,
                deviationZ: CenterClusterScale,
                starSizeDeviation: StarSizeDeviation
            );

            Cluster cluster = new Cluster(
                sphere,
                CenterClusterCountMean,
                CenterClusterCountDeviation,
                Size * CenterClusterPositionDeviation,
                Size * CenterClusterPositionDeviation,
                Size * CenterClusterPositionDeviation
            );

            foreach (Star star in cluster.Generate(random)) {
                yield return star.Swirl(Vector3.Up, Swirl * 5);
            }
        }

        private IEnumerable<Star> GenerateArms(Random random)
        {
            int arms = random.Next(MinimumArms, MaximumArms);

            float armAngle = (float) ((Math.PI * 2) / arms);

            int maxClusters = (Size / Spacing) / arms;

            for (int arm = 0; arm < arms; arm++) {
                int clusters = (int) Math.Round(
                    random.NormallyDistributedSingle(
                        maxClusters * ClusterCountDeviation,
                        maxClusters
                    )
                );

                for (int i = 0; i < clusters; i++) {
                    float angle = random.NormallyDistributedSingle(
                        0.5f * armAngle * ClusterCenterDeviation, 0
                    ) + armAngle * arm;

                    float dist = Math.Abs(
                        random.NormallyDistributedSingle(Size * 0.4f, 0)
                    );

                    Vector3 center = new Vector3(0, 0, dist);
                    center = center.Rotated(new Vector3(0, 1, 0), angle);

                    var clsScaleDev = ArmClusterScaleDeviation * Size;
                    var clsScaleMin = MinArmClusterScale * Size;
                    var clsScaleMax = MaxArmClusterScale * Size;
                    var cSize = random.NormallyDistributedSingle(
                        clsScaleDev,
                        clsScaleMin * 0.5f + clsScaleMax * 0.5f,
                        clsScaleMin,
                        clsScaleMax
                    );

                    var stars = new Sphere(
                        cSize,
                        densityMean: 0.00025f,
                        deviationX: 1,
                        deviationY: 1,
                        deviationZ: 1,
                        starSizeDeviation: 0.15f
                    ).Generate(random);

                    foreach (GalaxyLib.Star star in stars) {
                        yield return star.Offset(center).Swirl(Vector3.Up, Swirl);
                    }
                }
            }
        }
    }
}
