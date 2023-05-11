using Godot;
using System;
using System.Threading.Tasks;


public partial class Galaxy : Node3D
{
    [ExportCategory("Template")]
    [Export]
    public PackedScene StarTemplate;

    [ExportCategory("Generator Settings")]
    [Export]
    public int Size = 750;
    [Export]
    public int Spacing = 5;
    [Export]
    public int MinimumArms = 3;
    [Export]
    public int MaximumArms = 7;
    [Export]
    public float ClusterCountDeviation = 0.35f;
    [Export]
    public float ClusterCenterDeviation = 0.2f;
    [Export]
    public float MinArmClusterScale = 0.02f;
    [Export]
    public float ArmClusterScaleDeviation = 0.02f;
    [Export]
    public float MaxArmClusterScale = 0.1f;
    [Export]
    public float Swirl = (float) Math.PI * 4;
    [Export]
    public float CenterClusterScale = 0.19f;
    [Export(PropertyHint.Range, "0.1,1.0")]
    public float CenterClusterDensityMean = 0.5f;
    [Export(PropertyHint.Range, "0.1,1.0")]
    public float CenterClusterDensityDeviation = 0.5f;
    [Export(PropertyHint.Range, "0.1,1.0")]
    public float CenterClusterSizeDeviation = 0.125f;
    [Export]
    public float CenterClusterPositionDeviation = 20;
    [Export]
    public float CenterClusterCountDeviation = 3;
    [Export]
    public float CenterClusterCountMean = 0.075f;
    [Export]
    public float CentralVoidSizeMean = 25;
    [Export]
    public float CentralVoidSizeDeviation = 7;
    [Export(PropertyHint.Range, "0.05, 0.95")]
    public float StarSizeDeviation = 0.15f;

    private GalaxyLib.Galaxy galaxy;

    private bool generated = false;

    public override void _Ready()
    {
        Task.Run(async () => {
            var rand = new System.Random();
            var spiral = new GalaxyLib.Spiral();

            // 0.5
            // 0.00005f

            spiral.Size = Size;
            spiral.Spacing = Spacing;
            spiral.MinimumArms = MinimumArms;
            spiral.MaximumArms = MaximumArms;
            spiral.ClusterCountDeviation = ClusterCountDeviation;
            spiral.ClusterCenterDeviation = ClusterCenterDeviation;
            spiral.MinArmClusterScale = MinArmClusterScale;
            spiral.ArmClusterScaleDeviation = ArmClusterScaleDeviation;
            spiral.MaxArmClusterScale = MaxArmClusterScale;
            spiral.Swirl = Swirl;
            spiral.CenterClusterScale = CenterClusterScale;
            spiral.CenterClusterDensityMean = CenterClusterDensityMean * 0.0001f;
            spiral.CenterClusterDensityDeviation = CenterClusterDensityDeviation * 0.00001f;
            spiral.CenterClusterSizeDeviation = CenterClusterSizeDeviation * 0.01f;
            spiral.CenterClusterPositionDeviation = CenterClusterPositionDeviation;
            spiral.CenterClusterCountDeviation = CenterClusterCountDeviation;
            spiral.CenterClusterCountMean = CenterClusterCountMean;
            spiral.CentralVoidSizeMean = CentralVoidSizeMean;
            spiral.CentralVoidSizeDeviation = CentralVoidSizeDeviation;
            spiral.StarSizeDeviation = StarSizeDeviation;

            this.galaxy = await GalaxyLib.Galaxy.Generate(spiral, rand);
            this.generated = this.galaxy.Generated;
        });
    }

    public override void _Process(double delta)
    {
        RandomNumberGenerator rand = new RandomNumberGenerator();

        if (generated) {
            foreach (GalaxyLib.Star star in this.galaxy.Stars) {
                Node3D starObj = StarTemplate.Instantiate<Node3D>();
                starObj.Position = star.Position;

                MeshInstance3D mesh = starObj.GetChild<MeshInstance3D>(0);
                ShaderMaterial mat = mesh.MaterialOverride as ShaderMaterial;
                mat.SetShaderParameter("star_color", star.Color);

                float vertScale = rand.RandfRange(0.5f, 4.0f);
                mat.SetShaderParameter("vert_scale", vertScale);

                float corona = rand.RandfRange(0.05f, 0.2f);
                mat.SetShaderParameter("corona", corona);

                float diffHardness = rand.RandfRange(1.0f, 5.0f);
                mat.SetShaderParameter("diffusion_hardness", diffHardness);

                mesh.MaterialOverride = mat;

                AddChild(starObj);
            }

            generated = false;
        }
    }
}
