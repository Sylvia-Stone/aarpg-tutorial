using AarpgTutorial.Common.Enums;
using AarpgTutorial.Common.HurtBox;
using AarpgTutorial.Common.Utilities;
using AarpgTutorial.PlayerCharacter.Scripts;
using Godot;

namespace AarpgTutorial.PlayerCharacter.States;

public partial class Stun : PlayerState
{
    #region Exports

    [Export]
    public StateType AnimationStateType = StateType.Stun;
    [Export]
    public double KnockBackSpeed = 200.0;
    [Export]
    public double DecelerateSpeed = 10.0;
    [Export]
    public PlayerState IdleState = null!;
    [Export]
    public double InvulnerableDuration = 1.0;

    #endregion
    
    #region Fields
    
    private Vector2 _direction;
    private HurtBox? _hurtBox;
    private PlayerState? _nextState;
    
    #endregion

    #region Lifecycle

    public override void _Ready()
    {
        IdleState.Require();
    }

    /// <summary>
    /// Subscribes to <see cref="Player.PlayerDamaged"/> so this state can intercept
    /// damage events and queue a transition to itself regardless of the active state.
    /// </summary>
    public override void Init(Player actor)
    {
        actor.PlayerDamaged += OnPlayerDamaged;
    }

    public override void Enter()
    {
        Player.AnimationPlayer.AnimationFinished += OnAnimationFinished;

        _direction = Player.GlobalPosition.DirectionTo(_hurtBox!.GlobalPosition);
        Player.Velocity = _direction * (float)KnockBackSpeed * -1;
        Player.SetDirection(_direction);
        
        Player.UpdateAnimation(AnimationStateType);
        Player.MakeInvulnerable(InvulnerableDuration);
        Player.EffectAnimationPlayer.Play(nameof(AnimationType.Damaged));
    }

    public override void Exit()
    {
        _nextState = null;
        Player.AnimationPlayer.AnimationFinished -= OnAnimationFinished;
    }

    public override PlayerState? Process(double delta)
    {
        Player.Velocity -= Player.Velocity * (float)DecelerateSpeed * (float)delta;
        return _nextState;
    }

    #endregion
    
    #region Private Methods

    /// <summary>
    /// Queues the transition to idle once the stun animation completes.
    /// The actual state change happens on the next process tick via <see cref="Process"/>.
    /// </summary>
    private void OnAnimationFinished(StringName stringName)
    {
        _nextState = IdleState;
    }
    
    /// <summary>
    /// Stores the offending <paramref name="hurtBox"/> so <see cref="Enter"/> can
    /// calculate knock-back direction, then immediately transitions into the stun state.
    /// </summary>
    private void OnPlayerDamaged(HurtBox hurtBox)
    {
        _hurtBox = hurtBox;
        StateMachine.ChangeState(this);
    }
    
    #endregion
}
