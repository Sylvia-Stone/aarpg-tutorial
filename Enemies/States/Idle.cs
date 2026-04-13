using AarpgTutorial.Common.Enums;
using Godot;

namespace AarpgTutorial.Enemies.States;

public partial class Idle : EnemyState
{
    #region Exports

    [Export]
    private StateType _animationStateType = StateType.Idle;

    [ExportCategory("AI")]
    [Export]
    private double _stateDurationMin = .5;
    [Export]
    private double _stateDurationMax = 1.5;
    [Export]
    private EnemyState? _nextState;

    #endregion

    #region Fields

    private double _timer;

    #endregion

    #region Lifecycle

    public override void Enter()
    {
        Enemy.Velocity = Vector2.Zero;
        _timer = GD.RandRange(_stateDurationMin, _stateDurationMax);
        Enemy.UpdateAnimation(_animationStateType);
    }

    public override EnemyState? Process(double delta)
    {
        _timer -= delta;
        return _timer <= 0 ? _nextState : null;
    }

    #endregion
}
