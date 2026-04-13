using AarpgTutorial.Common;
using AarpgTutorial.Common.Enums;
using AarpgTutorial.Player.Scripts;
using Godot;

namespace AarpgTutorial.Player.States;

public partial class Idle : PlayerState
{
    #region Lifecycle

    public override void Enter()
    {
        PlayerCharacter.UpdateAnimation(StateType.Idle);
    }

    public override PlayerState? Process(double delta)
    {
        if (Input.IsActionJustPressed(InputActions.Attack)) return StateMachine.GetState<Attack>();
        if (PlayerCharacter.Direction != Vector2.Zero) return StateMachine.GetState<Move>();
        PlayerCharacter.Velocity = Vector2.Zero;
        return null;
    }

    #endregion
}
