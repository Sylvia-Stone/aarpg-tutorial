using AarpgTutorial.Common.States;
using AarpgTutorial.Enemies.Scripts;
using Godot;

namespace AarpgTutorial.Enemies.States;

/// <summary>
/// Abstract base for all enemy states. Provides typed access to the owning
/// <see cref="Enemy"/> and <see cref="EnemyStateMachine"/> via computed properties
/// so concrete states never need to cache or cast them manually.
/// </summary>
public abstract partial class EnemyState : State<Enemy>
{
    #region Accessors

    protected Enemy Enemy => StateMachine.Actor;
    protected EnemyStateMachine StateMachine => GetParent<EnemyStateMachine>();

    #endregion

    #region Lifecycle Methods

    /// <inheritdoc/>
    public override EnemyState? HandleInput(InputEvent e) => null;

    /// <inheritdoc/>
    public override EnemyState? Physics(double delta) => null;

    /// <inheritdoc/>
    public override EnemyState? Process(double delta) => null;

    #endregion
}
