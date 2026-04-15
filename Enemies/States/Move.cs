using AarpgTutorial.Common;
using AarpgTutorial.Common.Enums;
using Godot;

namespace AarpgTutorial.Enemies.States;

/// <summary>
/// Enemy wander state. The enemy walks in a random cardinal direction for a random
/// number of animation cycles before transitioning to the next state.
/// </summary>
public partial class Move : EnemyState
{
    #region Exports

    [Export]
    private StateType _animationStateType = StateType.Walk;
    [Export]
    private double _wanderSpeed = 20.0;

    [ExportCategory("AI")]
    [Export]
    private double _stateAnimationDuration = .7;
    [Export]
    private int _stateCyclesMin = 1;
    [Export]
    private int _stateCyclesMax = 3;
    [Export]
    private EnemyState? _nextState;

    #endregion

    #region Fields

    private double _timer;
    private Vector2 _direction;

    #endregion

    #region Lifecycle

    /// <summary>
    /// Picks a random cardinal direction and a random wander duration, then sets velocity.
    /// </summary>
    public override void Enter()
    {
        _timer = GD.RandRange(_stateCyclesMin, _stateCyclesMax) * _stateAnimationDuration;
        _direction = Actor.CardinalDirections[GD.Randi() % Actor.CardinalDirections.Length];
        Enemy.Velocity = _direction * (float)_wanderSpeed;
        Enemy.SetDirection(_direction);
        Enemy.UpdateAnimation(_animationStateType);
    }

    /// <summary>
    /// Counts down the wander timer. Transitions to <c>_nextState</c> when it expires.
    /// </summary>
    public override EnemyState? Process(double delta)
    {
        _timer -= delta;
        return _timer < 0 ? _nextState : null;
    }

    #endregion
}
