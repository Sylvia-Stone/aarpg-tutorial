using AarpgTutorial.Common.Enums;
using Godot;

namespace AarpgTutorial.Enemies.States;

public partial class Idle : EnemyState
{
    [Export]
    private StateType _animationStateType = StateType.Idle;

    [ExportCategory("AI")]
    [Export]
    private double _stateDurationMin = .5;
    [Export]
    private double _stateDurationMax = 1.5;
    [Export]
    private EnemyState _nextState;

    private double _timer;

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
}
