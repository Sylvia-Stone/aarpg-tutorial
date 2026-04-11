using Godot;

namespace AARPGtutorial.Player.Scripts;

public partial class StateAttack : State
{
    [Export] private AudioStream _attackSound;
    [Export(PropertyHint.Range, "1,20,.5")] double _decelerationRate;
    [Export] private AnimationPlayer _playerAnimationPlayer;
    [Export] private AnimationPlayer _attackAnimationPlayer;
    [Export] private AudioStreamPlayer2D _audioStreamPlayer2D;
    [Export] private Area2D _hurtBox;

    private bool _attacking = false;

    public override async void Enter()
    {
        Player.UpdateAnimation(PlayerState.Attack);
        _attackAnimationPlayer.Play($"{PlayerState.Attack}{Player.GetAnimDirection()}");
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

    public override State Process(double delta)
    {
        Player.Velocity -= Player.Velocity * (float)_decelerationRate * (float)delta;

        if (_attacking) return null;
        return Player.Direction == Vector2.Zero
            ? StateMachine.GetState<StateIdle>()
            : StateMachine.GetState<StateWalk>();
    }

    private void EndAttack(StringName _)
    {
        _attacking = false;
    }
}