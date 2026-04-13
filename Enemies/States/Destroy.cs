using AarpgTutorial.Common.Enums;
using AarpgTutorial.Enemies.States;
using Godot;

namespace AARPGtutorial.Enemies.States;

public partial class Destroy : EnemyState
{
    #region Exports

    [Export]
    private StateType _animationStateType = StateType.Destroy;
    [Export]
    private double _knockBackSpeed = 200.0;
    [Export]
    private double _decelerateSpeed = 10.0;

    #endregion

    #region Private Fields

    private Vector2 _direction;

    #endregion

    #region Public Methods

    public override void Init()
    {
        Enemy.EnemyDestroyed += OnEnemyDestroyed;
    }

    public override void Enter()
    {
        Enemy.IsInvulnerable = true;

        _direction = Enemy.GlobalPosition.DirectionTo(Enemy.PlayerCharacter.GlobalPosition);

        Enemy.SetDirection(_direction);
        Enemy.Velocity = _direction * -(float)_knockBackSpeed;

        Enemy.UpdateAnimation(_animationStateType);
        Enemy.AnimationPlayer.AnimationFinished += OnAnimationFinished;
    }

    public override EnemyState? Process(double delta)
    {
        Enemy.Velocity -= Enemy.Velocity * (float)_decelerateSpeed * (float)delta;
        return null;
    }

    #endregion

    #region Private Methods

    private void OnEnemyDestroyed()
    {
        StateMachine.ChangeState(this);
    }

    private void OnAnimationFinished(StringName animation)
    {
        Enemy.QueueFree();
    }

    #endregion
}