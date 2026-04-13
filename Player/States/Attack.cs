using AarpgTutorial.Common.Enums;
using Godot;

namespace AarpgTutorial.Player.States;

public partial class Attack : PlayerState
{
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

    private bool _attacking;

    public override async void Enter()
    {
        PlayerCharacter.UpdateAnimation(StateType.Attack);
        _attackAnimationPlayer.Play($"{StateType.Attack}{PlayerCharacter.GetAnimDirection()}");
        _playerAnimationPlayer.AnimationFinished += EndAttack;

        _audioStreamPlayer2D.Stream = _attackSound;
        _audioStreamPlayer2D.PitchScale = (float)GD.RandRange(.9, 1.1);
        _audioStreamPlayer2D.Play();

        _attacking = true;
        await ToSignal(GetTree().CreateTimer(.075), SceneTreeTimer.SignalName.Timeout);
        _hurtBox.Monitoring = true;
    }

    public override void Exit()
    {
        _playerAnimationPlayer.AnimationFinished -= EndAttack;
        _attacking = false;
        _hurtBox.Monitoring = false;
    }

    public override PlayerState? Process(double delta)
    {
        PlayerCharacter.Velocity -= PlayerCharacter.Velocity * (float)_decelerationRate * (float)delta;

        if (_attacking) return null;
        return PlayerCharacter.Direction == Vector2.Zero
            ? StateMachine.GetState<Idle>()
            : StateMachine.GetState<Move>();
    }

    private void EndAttack(StringName _)
    {
        _attacking = false;
    }
}
