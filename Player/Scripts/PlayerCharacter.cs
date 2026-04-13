using AarpgTutorial.Common;
using AarpgTutorial.Player.States;
using Godot;

namespace AarpgTutorial.Player.Scripts;

public partial class PlayerCharacter : Actor
{
    [Export]
    private PlayerStateMachine _stateMachine;

    public override void _Ready()
    {
        PlayerManager.Instance.PlayerCharacter = this;
        _stateMachine.Initialize(this);
    }

    public override void _PhysicsProcess(double delta)
    {
        Direction = new Vector2(
            Input.GetAxis(InputActions.Left, InputActions.Right),
            Input.GetAxis(InputActions.Up, InputActions.Down)
        ).Normalized();
        base._PhysicsProcess(delta);
    }
}
