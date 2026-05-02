using AarpgTutorial.Common;
using AarpgTutorial.Common.Enums;
using AarpgTutorial.Common.Utilities;
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
    public StateType AnimationStateType = StateType.Walk;

    [Export]
    public double WanderSpeed = 20.0;

    [ExportCategory("AI")]
    [Export]
    public EnemyState? NextState;

    [Export]
    public double StateAnimationDuration = .7;

    [Export]
    public int StateCyclesMax = 3;

    [Export]
    public int StateCyclesMin = 1;

    #endregion

    #region Fields

    private Vector2 _direction;
    private double _timer;

    #endregion

    #region Lifecycle Methods

    public override void _Ready()
    {
        NextState.Require();
    }
    
    /// <summary>
    /// Picks a random cardinal direction and a random wander duration, then sets velocity.
    /// </summary>
    public override void Enter()
    {
        _timer = GD.RandRange(StateCyclesMin, StateCyclesMax) * StateAnimationDuration;
        _direction = Actor.CardinalDirections[GD.Randi() % Actor.CardinalDirections.Length];
        Enemy.Velocity = _direction * (float)WanderSpeed;
        Enemy.SetDirection(_direction);
        Enemy.UpdateAnimation(AnimationStateType);
    }

    /// <summary>
    /// Counts down the wander timer. Transitions to <c>_nextState</c> when it expires.
    /// </summary>
    public override EnemyState? Process(double delta)
    {
        _timer -= delta;
        return _timer < 0 ? NextState : null;
    }

    #endregion
}
