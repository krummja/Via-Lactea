using Godot;
using Godot.Collections;


public partial class Director : Node
{
    [Signal]
    public delegate void OnLoadCompleteEventHandler();

    [Export]
    public Array<PackedScene> ScenePaths = new Array<PackedScene>();

    public enum MountPoint
    {
        WORLD,
        OVERLAY,
    }

    public Node3D World;
    public Control Overlay;

    public Array Progress;

    private Main main;
    private PackedScene loadedScene;
    private MountPoint mountPoint;
    private string scenePath;
    private bool isLoading = false;

    public override void _Ready()
    {
        this.main = GetParent<Main>();

        World = this.main.GetNode<Node3D>("World");
        Overlay = this.main.GetNode<Control>("Overlay");
    }

    public override void _Process(double delta)
    {
        ResourceLoader.ThreadLoadStatus sceneLoadStatus = ResourceLoader
            .LoadThreadedGetStatus(scenePath, Progress);

        while (isLoading) {
            switch (sceneLoadStatus)
            {
                case ResourceLoader.ThreadLoadStatus.InProgress:
                    break;

                case ResourceLoader.ThreadLoadStatus.Loaded:
                    loadedScene = ResourceLoader
                        .LoadThreadedGet(scenePath) as PackedScene;
                    Mount(mountPoint);
                    EmitSignal(SignalName.OnLoadComplete);
                    isLoading = false;
                    break;

                case ResourceLoader.ThreadLoadStatus.Failed:
                    GD.PrintErr("Resource loading failed");
                    return;

                case ResourceLoader.ThreadLoadStatus.InvalidResource:
                    GD.PrintErr("Invalid resource.");
                    return;

                default:
                    GD.PrintErr("State not found");
                    return;
            }
        }
    }

    public Node LoadImmediate(string sceneName, MountPoint mountPoint)
    {
        SetScenePath(sceneName, mountPoint);
        loadedScene = (PackedScene) ResourceLoader.Load(scenePath);
        Node mounted = Mount(mountPoint);
        return mounted;
    }

    public void StartLoad(string sceneName, MountPoint mountPoint)
    {
        SetScenePath(sceneName, mountPoint);
        ResourceLoader.LoadThreadedRequest(scenePath);
        Progress = new Array();
        isLoading = true;
        this.mountPoint = mountPoint;
    }

    private void SetScenePath(string sceneName, MountPoint mountPoint)
    {
        switch (mountPoint) {
            case MountPoint.WORLD:
                scenePath = "res://scenes/world/";
                break;
            case MountPoint.OVERLAY:
                scenePath = "res://scenes/overlay/";
                break;
        }

        scenePath += $"{sceneName}/{sceneName}.tscn";
    }

    private Node Mount(MountPoint mountPoint)
    {
        if (loadedScene == null) return null;

        Node child = loadedScene.Instantiate();

        switch (mountPoint) {
            case MountPoint.WORLD:
                this.main.GetNode<Node3D>("World").AddChild(child);
                break;
            case MountPoint.OVERLAY:
                this.main.GetNode<Control>("Overlay").AddChild(child);
                break;
        }

        return child;
    }
}
