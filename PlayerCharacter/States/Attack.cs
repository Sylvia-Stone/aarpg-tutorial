using AarpgTutorial.Common.Enums;
using AarpgTutorial.Common.Utilities;
using Godot;

namespace AarpgTutorial.PlayerCharacter.States;

/// <summary>
/// Player attack state. Plays the attack animation and sound, enables the hurt box with a short
/// delay for hit timing, then decelerates the player and transitions out once the animation ends.
/// </summary>
public partial class Attack : PlayerState
{
    #region Exports

    [Export]
    public AnimationPlayer AttackAnimationPlayer = null!;

    [Export]
    public AudioStream AttackSound = null!;

    [Export]
    public AudioStreamPlayer2D AudioStreamPlayer2D = null!;

    [Export(PropertyHint.Range, "1,20,.5")]
    public double DecelerationRate;

    [Export]
    public Area2D HurtBox = null!;

    [Export]
    public AnimationPlayer PlayerAnimationPlayer = null!;

    #endregion

    #region Fields

    private bool _attacking;

    #endregion

    #region Lifecycle Methods

    public override void _Ready()
    {
        AttackAnimationPlayer.Require();
        AttackSound.Require();
        AudioStreamPlayer2D.Require();
        HurtBox.Require();
        PlayerAnimationPlayer.Require();
    }

    /// <summary>
    /// Starts the attack and sprite animations, plays a randomized-pitch sound,
    /// and enables the hurt box after a short delay to align with the animation.
    /// </summary>
    public override void Enter()
    {
        Player.UpdateAnimation(StateType.Attack);
        AttackAnimationPlayer.Play($"{StateType.Attack}{Player.GetAnimDirection()}");
        PlayerAnimationPlayer.AnimationFinished += EndAttack;

        AudioStreamPlayer2D.Stream = AttackSound;
        AudioStreamPlayer2D.PitchScale = (float)GD.RandRange(.9, 1.1);
        AudioStreamPlayer2D.Play();

        _attacking = true;
        GetTree().CreateTimer(.075).Timeout += () => HurtBox.Monitoring = true;
    }

    /// <summary>
    /// Unsubscribes from the animation signal and disables the hurt box.
    /// </summary>
    public override void Exit()
    {
        PlayerAnimationPlayer.AnimationFinished -= EndAttack;
        _attacking = false;
        HurtBox.Monitoring = false;
    }

    /// <summary>
    /// Decelerates the player each frame. Once the animation has ended,
    /// transitions to <see cref="Idle"/> or <see cref="Move"/> based on current input.
    /// </summary>
    public override PlayerState? Process(double delta)
    {
        Player.Velocity -= Player.Velocity * (float)DecelerationRate * (float)delta;

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
