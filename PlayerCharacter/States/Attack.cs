using AarpgTutorial.Common.Enums;
using Godot;

namespace AarpgTutorial.Player.States;

/// <summary>
/// Player attack state. Plays the attack animation and sound, enables the hurt box with a short
/// delay for hit timing, then decelerates the player and transitions out once the animation ends.
/// </summary>
public partial class Attack : PlayerState
{
    #region Exports

    [Export]
    private AudioStream _attackSound;
    [Export(PropertyHint.Range, "1,20,.5")]
    private double _decelerationRate;
    [Export]
    private AnimationPlayer _playerAnimationPlayer;
    [Export]
    private AnimationPlayer _attackAnimationPlayer;
    [Export]
    private AudioStreamPlayer2D _audioStreamPlayer2D;
    [Export]
    private Area2D _hurtBox;

    #endregion

    #region Fields

    private bool _attacking;

    #endregion

    #region Lifecycle

    /// <summary>
    /// Starts the attack and sprite animations, plays a randomized-pitch sound,
    /// and enables the hurt box after a short delay to align with the animation.
    /// </summary>
    public override void Enter()
    {
        Player.UpdateAnimation(StateType.Attack);
        _attackAnimationPlayer.Play($"{StateType.Attack}{Player.GetAnimDirection()}");
        _playerAnimationPlayer.AnimationFinished += EndAttack;

        _audioStreamPlayer2D.Stream = _attackSound;
        _audioStreamPlayer2D.PitchScale = (float)GD.RandRange(.9, 1.1);
        _audioStreamPlayer2D.Play();

        _attacking = true;
        GetTree().CreateTimer(.075).Timeout += () => _hurtBox.Monitoring = true;
    }

    /// <summary>
    /// Unsubscribes from the animation signal and disables the hurt box.
    /// </summary>
    public override void Exit()
    {
        _playerAnimationPlayer.AnimationFinished -= EndAttack;
        _attacking = false;
        _hurtBox.Monitoring = false;
    }

    /// <summary>
    /// Decelerates the player each frame. Once the animation has ended,
    /// transitions to <see cref="Idle"/> or <see cref="Move"/> based on current input.
    /// </summary>
    public override PlayerState? Process(double delta)
    {
        Player.Velocity -= Player.Velocity * (float)_decelerationRate * (float)delta;

        if (_attacking) return null;
        return Player.Direction == Vector2.Zero
            ? StateMachine.GetState<Idle>()
            : StateMachine.GetState<Move>();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Clears <c>_attacking</c> once the attack animation finishes,
    /// allowing <see cref="Process"/> to transition out of this state.
    /// </summary>
    private void EndAttack(StringName _)
    {
        _attacking = false;
    }

    #endregion
}
