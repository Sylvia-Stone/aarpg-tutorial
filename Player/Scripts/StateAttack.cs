using System;
using Godot;

namespace AARPGtutorial.Player.Scripts;

public partial class StateAttack : State
{
    [Export] private AudioStream _attackSound;
    [Export(PropertyHint.Range, "1,20,.5")] double _decelerationRate;
    
    private bool _attacking = false;
    private AnimationPlayer _playerAnimationPlayer;
    private AnimationPlayer _attackAnimationPlayer;
    private AudioStreamPlayer2D _audioStreamPlayer2D;


    public override void _Ready()
    {
        _playerAnimationPlayer = GetNode<AnimationPlayer>("../../AnimationPlayer");
        _attackAnimationPlayer = GetNode<AnimationPlayer>("../../Sprite2D/AttackEffectSprite/AnimationPlayer");
        _audioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("../../Audio/AudioStreamPlayer2D");
    }

    public override void Enter()
    {
        Player.UpdateAnimation(PlayerState.Attack);
        _attackAnimationPlayer.Play($"{PlayerState.Attack}{Player.GetAnimDirection()}");
        _playerAnimationPlayer.AnimationFinished += EndAttack;

        _audioStreamPlayer2D.Stream = _attackSound;
        _audioStreamPlayer2D.PitchScale = (float)GD.RandRange(.9, 1.1);
        _audioStreamPlayer2D.Play();
        
        _attacking = true;
    }

    public override State Process(double delta)
    {
        Player.Velocity -= Player.Velocity * (float)_decelerationRate * (float)delta;

        if (_attacking) return null;
        return Player.Direction == Vector2.Zero
            ? StateMachine.GetState<StateIdle>()
            : StateMachine.GetState<StateWalk>();
    }

    private void EndAttack(StringName animName)
    {
        _attacking = false;
        _playerAnimationPlayer.AnimationFinished -= EndAttack;
    }
}
