using Godot;

namespace AarpgTutorial.Common.States;

/// <summary>
/// Base class for a single state in a <see cref="StateMachine{TActor,TState}"/>.
/// Subclass via an intermediate abstract (e.g. <c>EnemyState</c>) to implement per-state behavior.
/// </summary>
public abstract partial class State<TActor> : Node
    where TActor : class
{
    #region Lifecycle Methods

    /// <summary>Called when the state machine transitions into this state.</summary>
    public virtual void Enter() { }

    /// <summary>Called when the state machine transitions out of this state.</summary>
    public virtual void Exit() { }

    /// <summary>
    /// Input event hook. Return a state to transition to, or <c>null</c> to remain in this state.
    /// Only called from state machines that override <c>_UnhandledInput</c>.
    /// </summary>
    public virtual State<TActor>? HandleInput(InputEvent e) => null;

    /// <summary>
    /// One-time setup called during <see cref="StateMachine{TActor,TState}.Initialize"/> before
    /// any state is entered. Use this to subscribe to signals on <paramref name="actor"/> rather
    /// than in <see cref="Enter"/>, so subscriptions survive state transitions.
    /// </summary>
    public virtual void Init(TActor actor) { }

    /// <summary>
    /// Per-physics-frame logic hook. Return a state to transition to, or <c>null</c> to remain in this state.
    /// </summary>
    public virtual State<TActor>? Physics(double delta) => null;

    /// <summary>
    /// Per-frame logic hook. Return a state to transition to, or <c>null</c> to remain in this state.
    /// </summary>
    public virtual State<TActor>? Process(double delta) => null;

    #endregion
}
