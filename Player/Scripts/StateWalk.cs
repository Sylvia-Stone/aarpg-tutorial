using Godot;

namespace AARPGtutorial.Player.Scripts;

public partial class StateWalk : State
{
    [Export] public float MoveSpeed { get; set; } = 100;

    public override void Enter()
    {
        Player.UpdateAnimation(PlayerState.Walk);
    }

    public override State Process(double delta)
    {
        if (Input.IsActionJustPressed("Attack")) return StateMachine.GetState<StateAttack>();
        if (Player.Direction == Vector2.Zero) return StateMachine.GetState<StateIdle>();

        Player.Velocity = Player.Direction * MoveSpeed;

        if (Player.SetDirection())
        {
            Player.UpdateAnimation(PlayerState.Walk);
        }
        return null;
    }
}
