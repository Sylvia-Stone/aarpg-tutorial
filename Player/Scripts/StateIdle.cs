using Godot;

namespace AARPGtutorial.Player.Scripts;

public partial class StateIdle : State
{

    private State _walkState;

    public override void _Ready()
    {
        _walkState = GetNode<State>("../Walk");
    }
    public override void Enter()
    {
        Player.UpdateAnimation(PlayerState.Idle);
    }

    public override void Exit()
    {

    }

    public override State Process(double delta)
    {
        if (Player.Direction != Vector2.Zero)
        {
            return _walkState;
        }
        Player.Velocity = Vector2.Zero;
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