using System.Collections.Generic;
using System.Linq;
using Godot;

namespace AarpgTutorial.Common.States;

/// <summary>
/// Generic finite state machine. Owns a set of <typeparamref name="TState"/> child nodes,
/// routes Godot process/physics/input callbacks to the active state, and handles transitions.
/// Call <see cref="Initialize"/> from the actor's <c>_Ready</c> to activate.
/// </summary>
public abstract partial class StateMachine<TActor, TState> : Node
    where TActor : class
    where TState : State<TActor>
{
    #region Fields

    public TActor Actor { get; private set; } = null!;

    protected readonly List<TState> States = [];
    protected TState? CurrentState;
    protected TState? PreviousState;

    #endregion

    #region Lifecycle

    /// <summary>
    /// Disables processing until <see cref="Initialize"/> is called,
    /// preventing states from ticking before the actor is wired up.
    /// </summary>
    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Disabled;
    }

    /// <summary>
    /// Delegates the process tick to the active state and changes state if one is returned.
    /// </summary>
    public override void _Process(double delta)
    {
        if (CurrentState?.Process(delta) is TState next)
            ChangeState(next);
    }

    /// <summary>
    /// Delegates the physics tick to the active state and changes state if one is returned.
    /// </summary>
    public override void _PhysicsProcess(double delta)
    {
        if (CurrentState?.Physics(delta) is TState next)
            ChangeState(next);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Returns the first child state of type <typeparamref name="T"/>, or <c>null</c> if not found.
    /// </summary>
    public TState? GetState<T>() where T : TState => States.OfType<T>().FirstOrDefault();

    /// <summary>
    /// Exits the current state, records it as the previous state, and enters <paramref name="newState"/>.
    /// No-ops if <paramref name="newState"/> is <c>null</c> or is already the active state.
    /// </summary>
    public void ChangeState(TState? newState)
    {
        if (newState == null || newState == CurrentState) return;
        CurrentState?.Exit();
        PreviousState = CurrentState;
        CurrentState = newState;
        CurrentState.Enter();
    }

    /// <summary>
    /// Wires the state machine to its <paramref name="actor"/>, discovers child state nodes,
    /// calls <see cref="State{TActor}.Init"/> on each, then enters the first state and enables processing.
    /// Must be called once from the actor's <c>_Ready</c>.
    /// </summary>
    public void Initialize(TActor actor)
    {
        Actor = actor;

        foreach (var child in GetChildren())
        {
            if (child is TState state)
            {
                States.Add(state);
                state.Init(actor);
            }
        }

        if (States.FirstOrDefault() is { } initial)
        {
            ChangeState(initial);
            ProcessMode = ProcessModeEnum.Inherit;
        }
    }

    #endregion

}
