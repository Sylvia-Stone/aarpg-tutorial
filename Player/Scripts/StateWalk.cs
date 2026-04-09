using Godot;

namespace AARPGtutorial.Player.Scripts;

public partial class StateWalk : State
{
    [Export] public float MoveSpeed { get; set; } = 100;
    private State _idleState;

    public override void _Ready()
    {
        _idleState = GetNode<State>("../Idle");
    }
    
    public override void Enter()
    {
        Player.UpdateAnimation(PlayerState.Walk);
    }

    public override void Exit()
    {

    }

    public override State Process(double delta)
    {
        if (Player.Direction == Vector2.Zero) return _idleState;
        
        Player.Velocity = Player.Direction * MoveSpeed;

        if (Player.SetDirection())
        {
            Player.UpdateAnimation(PlayerState.Walk);
        }
        return null;
    }

    public override State Physics(double delta)
    {
        return null;
    }

    public override State HandleInput(InputEvent e)
    {
        return null;
    }
}