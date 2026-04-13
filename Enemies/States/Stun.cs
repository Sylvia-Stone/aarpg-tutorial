using AarpgTutorial.Common;
using AarpgTutorial.Common.Enums;
using Godot;

namespace AarpgTutorial.Enemies.States;

public partial class Stun : EnemyState
{
    #region Exports

    [Export]
    private StateType _animationStateType = StateType.Stun;
    [Export]
    private double _knockBackSpeed = 200.0;
    [Export]
    private double _decelerateSpeed = 10.0;

    [ExportCategory("AI")]
    [Export]
    private EnemyState? _nextState;

    #endregion

    #region Fields

    private Vector2 _direction;
    private bool _animationFinished;

    #endregion

    #region Lifecycle

    public override void Init()
    {
        Enemy.EnemyDamaged += OnEnemyDamaged;
    }

    public override void Enter()
    {
        Enemy.IsInvulnerable = true;
        _animationFinished = false;

        _direction = Enemy.GlobalPosition.DirectionTo(Enemy.PlayerCharacter.GlobalPosition);

        Enemy.SetDirection(_direction);
        Enemy.Velocity = _direction * -(float)_knockBackSpeed;

        Enemy.UpdateAnimation(_animationStateType);
        Enemy.AnimationPlayer.AnimationFinished += OnAnimationFinished;
    }

    public override void Exit()
    {
        Enemy.IsInvulnerable = false;
        Enemy.AnimationPlayer.AnimationFinished -= OnAnimationFinished;
    }

    public override EnemyState? Process(double delta)
    {
        if (_animationFinished) return _nextState;
        Enemy.Velocity -= Enemy.Velocity * (float)_decelerateSpeed * (float)delta;
        return null;
    }

    #endregion

    #region Private Methods

    private void OnEnemyDamaged()
    {
        GD.Print("OnEnemyDamaged fired");
        StateMachine.ChangeState(this);
    }

    private void OnAnimationFinished(StringName animation)
    {
        _animationFinished = true;
    }

    #endregion
}
