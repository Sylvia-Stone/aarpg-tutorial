using AarpgTutorial.Common.Enums;
using AarpgTutorial.Common.HurtBox;
using AarpgTutorial.Enemies.Scripts;
using Godot;

namespace AarpgTutorial.Enemies.States;

/// <summary>
/// Enemy death state. Applies a final knockback away from the damage source,
/// plays the destroy animation, then removes the enemy from the scene.
/// </summary>
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

    #region Fields

    private Vector2 _damagePosition;
    private Vector2 _direction;

    #endregion

    #region Lifecycle

    /// <summary>
    /// Subscribes to <see cref="Enemy.EnemyDestroyed"/> so this state can intercept
    /// the kill event and queue itself regardless of the currently active state.
    /// </summary>
    public override void Init(Enemy enemy)
    {
        enemy.EnemyDestroyed += OnEnemyDestroyed;
    }

    /// <summary>
    /// Marks the enemy invulnerable, applies knockback away from the damage source,
    /// and starts the destroy animation.
    /// </summary>
    public override void Enter()
    {
        Enemy.IsInvulnerable = true;

        _direction = Enemy.GlobalPosition.DirectionTo(_damagePosition);

        Enemy.SetDirection(_direction);
        Enemy.Velocity = _direction * -(float)_knockBackSpeed;

        Enemy.UpdateAnimation(_animationStateType);
        Enemy.AnimationPlayer.AnimationFinished += OnAnimationFinished;
    }

    /// <summary>
    /// Decelerates the knockback velocity each frame. This state has no exit transition;
    /// the enemy is removed at the end of the animation.
    /// </summary>
    public override EnemyState? Process(double delta)
    {
        Enemy.Velocity -= Enemy.Velocity * (float)_decelerateSpeed * (float)delta;
        return null;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Stores the kill-shot position for knock-back direction, then transitions into this state.
    /// </summary>
    private void OnEnemyDestroyed(HurtBox hurtBox)
    {
        _damagePosition = hurtBox.GlobalPosition;
        StateMachine.ChangeState(this);
    }

    /// <summary>
    /// Called when the destroy animation finishes. Removes the enemy from the scene.
    /// </summary>
    private void OnAnimationFinished(StringName animation)
    {
        Enemy.QueueFree();
    }

    #endregion
}
