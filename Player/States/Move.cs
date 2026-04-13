using AarpgTutorial.Common;
using AarpgTutorial.Common.Enums;
using AarpgTutorial.Player.Scripts;
using Godot;

namespace AarpgTutorial.Player.States;

public partial class Move : PlayerState
{
    #region Exports

    [Export]
    public float MoveSpeed { get; set; } = 100;

    #endregion

    #region Lifecycle

    public override void Enter()
    {
        PlayerCharacter.UpdateAnimation(StateType.Walk);
    }

    public override PlayerState? Process(double delta)
    {
        if (Input.IsActionJustPressed(InputActions.Attack)) return StateMachine.GetState<Attack>();
        if (PlayerCharacter.Direction == Vector2.Zero) return StateMachine.GetState<Idle>();

        PlayerCharacter.Velocity = PlayerCharacter.Direction * MoveSpeed;

        if (PlayerCharacter.SetDirection(PlayerCharacter.Direction))
            PlayerCharacter.UpdateAnimation(StateType.Walk);

        return null;
    }

    #endregion
}
