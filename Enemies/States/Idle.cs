using AarpgTutorial.Common.Enums;
using Godot;

namespace AarpgTutorial.Enemies.States;

/// <summary>
/// Enemy idle state. The enemy stands still for a random duration within
/// [<c>_stateDurationMin</c>, <c>_stateDurationMax</c>] before transitioning.
/// </summary>
public partial class Idle : EnemyState
{
    #region Exports

    [Export]
    private StateType _animationStateType = StateType.Idle;
    [Export]
    private EnemyState? _nextState;
    [Export]
    private double _stateDurationMax = 1.5;
    [Export]
    private double _stateDurationMin = .5;

    #endregion

    #region Fields

    private double _timer;

    #endregion

    #region Lifecycle

    /// <summary>
    /// Stops the enemy and picks a random duration to remain idle.
    /// </summary>
    public override void Enter()
    {
        Enemy.Velocity = Vector2.Zero;
        _timer = GD.RandRange(_stateDurationMin, _stateDurationMax);
        Enemy.UpdateAnimation(_animationStateType);
    }

    /// <summary>
    /// Counts down the idle timer. Transitions to <c>_nextState</c> when it expires.
    /// </summary>
    public override EnemyState? Process(double delta)
    {
        _timer -= delta;
        return _timer <= 0 ? _nextState : null;
    }

    #endregion
}
