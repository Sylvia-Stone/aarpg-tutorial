using AarpgTutorial.Common;
using AarpgTutorial.Common.Constants;
using AarpgTutorial.Common.Enums;
using Godot;

namespace AarpgTutorial.PlayerCharacter.States;

/// <summary>
/// Player movement state. Moves in the player's input direction at <see cref="MoveSpeed"/>
/// and updates the facing animation when the cardinal direction changes.
/// </summary>
public partial class Move : PlayerState
{
    #region Exports

    [Export]
    public float MoveSpeed { get; set; } = 100;

    #endregion

    #region Lifecycle

    /// <summary>Plays the walk animation for the current facing direction.</summary>
    public override void Enter()
    {
        Player.UpdateAnimation(StateType.Walk);
    }

    /// <summary>
    /// Applies velocity from player input. Transitions to <see cref="Attack"/> on attack input,
    /// or back to <see cref="Idle"/> when movement stops. Re-plays the walk animation if the
    /// facing direction changes mid-movement.
    /// </summary>
    public override PlayerState? Process(double delta)
    {
        if (Input.IsActionJustPressed(InputActions.Attack)) return StateMachine.GetState<Attack>();
        if (Player.Direction == Vector2.Zero) return StateMachine.GetState<Idle>();

        Player.Velocity = Player.Direction * MoveSpeed;

        if (Player.SetDirection(Player.Direction))
            Player.UpdateAnimation(StateType.Walk);

        return null;
    }

    #endregion
}
