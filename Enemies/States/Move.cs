using AarpgTutorial.Common.Enums;
using Godot;

namespace AarpgTutorial.Enemies.States;

public partial class Move : EnemyState
{
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
    private EnemyState _nextState;

    private double _timer;
    private Vector2 _direction;

    public override void Enter()
    {
        _timer = GD.RandRange(_stateCyclesMin, _stateCyclesMax) * _stateAnimationDuration;
        var directions = new[] { Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right };
        _direction = directions[GD.Randi() % directions.Length];
        Enemy.Velocity = _direction * (float)_wanderSpeed;
        Enemy.SetDirection(_direction);
        Enemy.UpdateAnimation(_animationStateType);
    }

    public override EnemyState? Process(double delta)
    {
        _timer -= delta;
        return _timer < 0 ? _nextState : null;
    }
}
