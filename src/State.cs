using Godot;
using System.Collections.Generic;


public partial class State<T>
{
    public delegate void DelegateNoArg();
    public DelegateNoArg OnEnter;
    public DelegateNoArg OnExit;
    public DelegateNoArg OnProcess;
    public DelegateNoArg OnInput;

    public string Name { get; set; }

    public T ID { get; private set; }

    public State(T id)
    {
        ID = id;
    }

    public State(T id, string name) : this(id)
    {
        Name = name;
    }

    public State(
        T id,
        DelegateNoArg onEnter,
        DelegateNoArg onExit = null,
        DelegateNoArg onProcess = null,
        DelegateNoArg onInput = null
    ) : this(id) {
        OnEnter = onEnter;
        OnExit = onExit;
        OnProcess = onProcess;
        OnInput = onInput;
    }

    public State(
        T id,
        string name,
        DelegateNoArg onEnter,
        DelegateNoArg onExit = null,
        DelegateNoArg onProcess = null,
        DelegateNoArg onInput = null
    ) : this(id, name) {
        OnEnter = onEnter;
        OnExit = onExit;
        OnProcess = onProcess;
        OnInput = onInput;
    }

    virtual public void Enter()
    {
        OnEnter?.Invoke();
    }

    virtual public void Exit()
    {
        OnExit?.Invoke();
    }

    virtual public void Process(double delta)
    {
        OnProcess?.Invoke();
    }

    virtual public void Input(InputEvent @event)
    {
        OnInput?.Invoke();
    }
}


public partial class FiniteStateMachine<T>
{
    protected Dictionary<T, State<T>> States;

    public State<T> CurrentState
    {
        get;
        protected set;
    }

    public FiniteStateMachine()
    {
        States = new Dictionary<T, State<T>>();
    }

    public void Add(State<T> state)
    {
        States.Add(state.ID, state);
    }

    public void Add(T stateID, State<T> state)
    {
        States.Add(stateID, state);
    }

    public State<T> GetState(T stateID)
    {
        if (States.ContainsKey(stateID)) {
            return States[stateID];
        }
        return null;
    }

    public void SetCurrentState(T stateID)
    {
        State<T> state = States[stateID];
        SetCurrentState(state);
    }

    public void SetCurrentState(State<T> state)
    {
        if (CurrentState == state) return;
        if (CurrentState != null) CurrentState.Exit();

        CurrentState = state;

        if (CurrentState != null) CurrentState.Enter();
    }

    public void Process(double delta)
    {
        if (CurrentState != null) CurrentState.Process(delta);
    }

    public void Input(InputEvent @event)
    {
        if (CurrentState != null) CurrentState.Input(@event);
    }
}
