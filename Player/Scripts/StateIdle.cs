using Godot;

namespace AARPGtutorial.Player.Scripts;

public partial class StateIdle : State
{
    public override void Enter()
    {
        Player.UpdateAnimation(PlayerState.Idle);
    }

    public override State Process(double delta)
    {
        if (Input.IsActionJustPressed(InputActions.Attack)) return StateMachine.GetState<StateAttack>();
        if (Player.Direction != Vector2.Zero) return StateMachine.GetState<StateWalk>();
        Player.Velocity = Vector2.Zero;
        return null;
    }
}