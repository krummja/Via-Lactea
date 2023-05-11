using Godot;


public partial class SplashScreen : Control
{
    [Signal]
    public delegate void OnSplashCompleteEventHandler();

    [Export]
    public AnimationPlayer LogoAnimator;

    [Export]
    public AnimationPlayer TransitionAnimator;

    [Export]
    public Label Debug;

    public Main Main;

    public override void _Ready()
    {
        Main = GetNode<Main>("/root/Main");
        if (Main.Debug) Debug.Show();

        LogoAnimator.AnimationFinished += (name) => {
            if (name == "simulacrum") {
                TransitionAnimator.Play("fade_in");
            }
        };

        TransitionAnimator.AnimationFinished += (name) => {
            if (name == "fade_in") {
                EmitSignal(SignalName.OnSplashComplete);
            }
        };

        LogoAnimator.Play("simulacrum");
    }

    public override void _Input(InputEvent @event)
    {
        if (!Main.Debug) return;

        if (@event is InputEventKey eventKey) {
            if (eventKey.Keycode == Key.Space) {
                LogoAnimator.Stop();
                LogoAnimator.Play("simulacrum");
            }
        }
    }
}
