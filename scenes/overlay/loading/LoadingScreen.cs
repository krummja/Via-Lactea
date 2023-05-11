using Godot;


public partial class LoadingScreen : Control
{
    [Signal]
    public delegate void OnLoadingReadyEventHandler();

    [Export]
    public AnimationPlayer LoadingAnimator;

    [Export]
    public AnimationPlayer TransitionAnimator;

    [Export]
    public Sprite2D Loading;

    [Export]
    public Label AnyKey;

    public override void _Ready()
    {
        Main main = GetNode<Main>("/root/Main");

        main.StateMachine.GetState(Main.States.LOAD).OnExit += () => {
            GetParent().RemoveChild(this);
            QueueFree();
        };

        Director director = GetNode<Director>("/root/Main/Director");
        director.OnLoadComplete += () => {
            LoadingAnimator.Stop();
            Loading.Hide();
            LoadingAnimator.Play("any_key");
            AnyKey.Show();
        };

        TransitionAnimator.AnimationFinished += (name) => {
            EmitSignal(SignalName.OnLoadingReady);
        };

        TransitionAnimator.Play("fade_out");

        LoadingAnimator.AnimationFinished += (name) => {
            if (name == "loading") {
                LoadingAnimator.Play("loading");
            }

            if (name == "any_key") {
                LoadingAnimator.Play("any_key");
            }
        };

        LoadingAnimator.Play("loading");
    }
}
