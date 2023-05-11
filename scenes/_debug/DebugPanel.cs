using Godot;


public partial class DebugPanel : Control
{
    [Export]
    public Node3D PlayerRoot;

    [Export]
    public Field PositionField;

    [Export]
    public Field VelocityField;

    public override void _Ready()
    {
        PositionField.UpdateValue($"{PlayerRoot.GlobalPosition}");
    }

    public override void _Process(double delta)
    {
        PositionField.UpdateValue($"{PlayerRoot.GlobalPosition}");
    }
}
