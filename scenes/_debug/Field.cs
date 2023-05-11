using Godot;


public partial class Field : Control
{
    [Export]
    public Label PropertyLabel;

    [Export]
    public Label ValueLabel;

    public void UpdateValue(string value)
    {
        ValueLabel.Text = value;
    }
}
