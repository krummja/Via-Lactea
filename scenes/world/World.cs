using Godot;


public partial class World : Node3D
{
    [Export]
    public Node3D PlayerRoot;

    public Vector3 CurrentPosition => PlayerRoot.GlobalPosition;

    public override void _Ready()
    {

    }

    public override void _Process(double delta)
    {

    }
}
