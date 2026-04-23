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
    public StateType AnimationStateType = StateType.Idle;

    [Export]
    public EnemyState? NextState;

    [Export]
    public double StateDurationMax = 1.5;

    [Export]
    public double StateDurationMin = .5;

    #endregion

    #region Fields

    private double _timer;

    #endregion

    #region Lifecycle Methods

    /// <summary>
    /// Stops the enemy and picks a random duration to remain idle.
    /// </summary>
    public override void Enter()
    {
        Enemy.Velocity = Vector2.Zero;
        _timer = GD.RandRange(StateDurationMin, StateDurationMax);
        Enemy.UpdateAnimation(AnimationStateType);
    }

    /// <summary>
    /// Counts down the idle timer. Transitions to <c>_nextState</c> when it expires.
    /// </summary>
    public override EnemyState? Process(double delta)
    {
        _timer -= delta;
        return _timer <= 0 ? NextState : null;
    }

    #endregion
}
