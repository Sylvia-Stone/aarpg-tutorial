using AarpgTutorial.Common.States;
using Godot;

namespace AarpgTutorial.PlayerCharacter.States;

/// <summary>
/// Abstract base for all player states. Provides typed access to the owning
/// <see cref="PlayerCharacter.Player"/> and <see cref="PlayerStateMachine"/> via computed properties
/// so concrete states never need to cache or cast them manually.
/// </summary>
public abstract partial class PlayerState : State<Player>
{
    #region Accessors

    protected Player Player => StateMachine.Actor;
    protected PlayerStateMachine StateMachine => GetParent<PlayerStateMachine>();

    #endregion

    #region Lifecycle Methods

    /// <inheritdoc/>
    public override PlayerState? HandleInput(InputEvent e) => null;

    /// <inheritdoc/>
    public override PlayerState? Physics(double delta) => null;

    /// <inheritdoc/>
    public override PlayerState? Process(double delta) => null;

    #endregion
}
