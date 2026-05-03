using AarpgTutorial.Common.Constants;
using AarpgTutorial.Common.Enums;
using AarpgTutorial.Common.Managers;
using Godot;

namespace AarpgTutorial.PlayerCharacter.States;

/// <summary>Player idle state. Waits for input and zeroes velocity.</summary>
public partial class Idle : PlayerState
{
    #region Lifecycle Methods

    /// <summary>Plays the idle animation for the current facing direction.</summary>
    public override void Enter()
    {
        Player.UpdateAnimation(StateType.Idle);
    }

    /// <summary>Handles input while idle. Transitions to Attack or Move, fires InteractPressed on interact, and zeroes velocity.</summary>
    public override PlayerState? Process(double delta)
    {
        if (Input.IsActionJustPressed(InputActions.Attack)) return StateMachine.GetState<Attack>();
        if (Player.Direction != Vector2.Zero) return StateMachine.GetState<Move>();
        if (Input.IsActionJustPressed(InputActions.Interact)) PlayerManager.Instance.RaisePlayerInteracted();

        Player.Velocity = Vector2.Zero;
        return null;
    }

    #endregion
}
