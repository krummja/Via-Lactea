using System;
using Godot;


public partial class Main : Node
{
	[Export]
	public bool Debug = true;

	[Export]
	public string Version = "1.0.0a";

	public enum States
	{
		BOOT,
		SPLASH,
		LOAD,
		MAIN,
		RUN,
		PAUSE,
		EXIT,
	}

	public Director Director;

	public class BootState : State<States>
	{
		private Main main;

		public BootState(Main main) : base(States.BOOT)
		{
			this.main = main;
		}

		public override void Enter()
		{
			base.Enter();

			// Console printout for the current build version.
			GD.Print($"Booted Via Lactea v{this.main.Version}");

			// Transition to the SPLASH state to start the splash sequence.
			this.main.StateMachine.SetCurrentState(States.SPLASH);
		}
	}

	public class SplashState : State<States>
	{
		private Main main;

		private SceneTreeTimer splashTimer;

		public SplashState(Main main) : base(States.SPLASH)
		{
			this.main = main;
		}

		public override void Enter()
		{
			base.Enter();

			// Load the SplashScreen.
			SplashScreen splash = this.main.Director.LoadImmediate(
				"splash", Director.MountPoint.OVERLAY
			) as SplashScreen;

			// Register a callback to transition state when all splash
			// animations have finished.
			splash.OnSplashComplete += () => {
				this.main.StateMachine.SetCurrentState(States.LOAD);
			};
		}
	}

	public class LoadState : State<States>
	{
		private Main main;
		private bool loading = false;
		private bool canContinue = false;

		public LoadState(Main main) : base(States.LOAD)
		{
			this.main = main;
		}

		public override void Enter()
		{
			base.Enter();
			LoadingScreen loadingScreen = this.main.Director.LoadImmediate(
				"loading", Director.MountPoint.OVERLAY
			) as LoadingScreen;

			this.main.Director.OnLoadComplete += () => {
				GD.Print("Load Complete!");
				loading = false;
				canContinue = true;
			};

			loadingScreen.OnLoadingReady += () => {
				loading = true;
				this.main.Director.StartLoad(
					"main_menu",
					Director.MountPoint.OVERLAY
				);
			};
		}

		public override void Input(InputEvent @event)
		{
			if (!canContinue) return;

			if (@event is InputEventKey eventKey) {
				this.main.StateMachine.SetCurrentState(States.MAIN);
			}
		}
	}

	public class MainState : State<States>
	{
		private Main main;

		public MainState(Main main) : base(States.MAIN)
		{
			this.main = main;
		}
	}

	public class RunState : State<States>
	{
		private Main main;

		public RunState(Main main) : base(States.RUN)
		{
			this.main = main;
		}
	}

	public class PauseState : State<States>
	{
		private Main main;

		public PauseState(Main main) : base(States.PAUSE)
		{
			this.main = main;
		}
	}

	public class ExitState : State<States>
	{
		private Main main;

		public ExitState(Main main) : base(States.EXIT)
		{
			this.main = main;
		}
	}

	public FiniteStateMachine<States> StateMachine;

    public override void _Ready()
    {
		Director = GetNode<Director>("Director");
		StateMachine = new FiniteStateMachine<States>();

		StateMachine.Add(new BootState(this));
		StateMachine.Add(new LoadState(this));
		StateMachine.Add(new SplashState(this));
		StateMachine.Add(new MainState(this));
		StateMachine.Add(new RunState(this));
		StateMachine.Add(new PauseState(this));
		StateMachine.Add(new ExitState(this));

		StateMachine.SetCurrentState(States.BOOT);
    }

	public override void _Process(double delta)
	{
		StateMachine.Process(delta);
	}

    public override void _Input(InputEvent @event)
    {
        StateMachine.Input(@event);
    }
}
