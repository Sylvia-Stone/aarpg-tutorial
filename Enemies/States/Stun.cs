using AarpgTutorial.Common;
using AarpgTutorial.Common.Enums;
using AarpgTutorial.Common.HurtBox;
using AarpgTutorial.Enemies.Scripts;
using Godot;

namespace AarpgTutorial.Enemies.States;

/// <summary>
/// Enemy stun state. Applies a brief knockback in the direction away from the damage source,
/// grants invulnerability for the duration, then transitions after the animation finishes.
/// </summary>
public partial class Stun : EnemyState
{
    #region Exports

    [Export]
    public StateType AnimationStateType = StateType.Stun;

    [Export]
    public double DecelerateSpeed = 10.0;

    [Export]
    public double KnockBackSpeed = 200.0;

    [ExportCategory("AI")]
    [Export]
    public EnemyState? NextState;

    #endregion

    #region Fields

    private bool _animationFinished;
    private Vector2 _damagePosition;
    private Vector2 _direction;

    #endregion

    #region Lifecycle Methods

    /// <summary>
    /// Applies knockback away from the stored damage position, marks the enemy invulnerable,
    /// and plays the stun animation.
    /// </summary>
    public override void Enter()
    {
        Enemy.IsInvulnerable = true;
        _animationFinished = false;

        _direction = Enemy.GlobalPosition.DirectionTo(_damagePosition);

        Enemy.SetDirection(_direction);
        Enemy.Velocity = _direction * -(float)KnockBackSpeed;

        Enemy.UpdateAnimation(AnimationStateType);
        Enemy.AnimationPlayer.AnimationFinished += OnAnimationFinished;
    }

    /// <summary>
    /// Restores vulnerability and unsubscribes from the animation finished signal.
    /// </summary>
    public override void Exit()
    {
        Enemy.IsInvulnerable = false;
        Enemy.AnimationPlayer.AnimationFinished -= OnAnimationFinished;
    }

    /// <summary>
    /// Subscribes to <see cref="Enemy.EnemyDamaged"/> so this state can intercept
    /// damage events and queue itself regardless of the currently active state.
    /// </summary>
    public override void Init(Enemy enemy)
    {
        enemy.EnemyDamaged += OnEnemyDamaged;
    }

    /// <summary>
    /// Decelerates the knockback velocity each frame.
    /// Transitions to <c>_nextState</c> once the stun animation has finished.
    /// </summary>
    public override EnemyState? Process(double delta)
    {
        if (_animationFinished) return NextState;
        Enemy.Velocity -= Enemy.Velocity * (float)DecelerateSpeed * (float)delta;
        return null;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Flags the animation as complete so <see cref="Process"/> can transition out next tick.
    /// </summary>
    private void OnAnimationFinished(StringName animation)
    {
        _animationFinished = true;
    }

    /// <summary>
    /// Stores the damage source position for knock-back direction, then transitions into this state.
    /// </summary>
    private void OnEnemyDamaged(HurtBox hurtBox)
    {
        _damagePosition = hurtBox.GlobalPosition;
        StateMachine.ChangeState(this);
    }

    #endregion
}
