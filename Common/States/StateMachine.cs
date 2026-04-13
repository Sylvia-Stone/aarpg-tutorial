using System.Collections.Generic;
using System.Linq;
using Godot;

namespace AarpgTutorial.Common.States;

public abstract partial class StateMachine<TActor, TState> : Node
    where TState : State
{
    #region Fields

    protected readonly List<TState> States = [];
    protected TState? CurrentState;
    protected TState? PreviousState;

    #endregion

    #region Lifecycle

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Disabled;
        foreach (var child in GetChildren())
            if (child is TState state)
                States.Add(state);
    }

    public override void _Process(double delta)
    {
        if (CurrentState?.Process(delta) is TState next)
            ChangeState(next);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (CurrentState?.Physics(delta) is TState next)
            ChangeState(next);
    }

    #endregion

    #region Public Methods

    public TState? GetState<T>() where T : TState => States.OfType<T>().FirstOrDefault();

    public void ChangeState(TState? newState)
    {
        if (newState == null || newState == CurrentState) return;
        CurrentState?.Exit();
        PreviousState = CurrentState;
        CurrentState = newState;
        CurrentState.Enter();
    }

    public abstract void Initialize(TActor actor);

    #endregion
}
