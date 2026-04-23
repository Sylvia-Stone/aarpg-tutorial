using AarpgTutorial.Common;
using AarpgTutorial.Common.Constants;
using AarpgTutorial.Common.Enums;
using Godot;

namespace AarpgTutorial.PlayerCharacter.States;

/// <summary>
/// Player idle state. Zeroes velocity and waits for movement or attack input.
/// </summary>
public partial class Idle : PlayerState
{
    #region Lifecycle

    /// <summary>Plays the idle animation for the current facing direction.</summary>
    public override void Enter()
    {
        Player.UpdateAnimation(StateType.Idle);
    }

    /// <summary>
    /// Transitions to <see cref="Attack"/> on attack input, or <see cref="Move"/> when
    /// the player starts moving. Zeroes velocity while idle.
    /// </summary>
    public override PlayerState? Process(double delta)
    {
        if (Input.IsActionJustPressed(InputActions.Attack)) return StateMachine.GetState<Attack>();
        if (Player.Direction != Vector2.Zero) return StateMachine.GetState<Move>();
        Player.Velocity = Vector2.Zero;
        return null;
    }

    #endregion
}
