using AarpgTutorial.Common;
using AarpgTutorial.Common.Constants;
using AarpgTutorial.Common.Enums;
using AarpgTutorial.Common.Managers;
using Godot;

namespace AarpgTutorial.PlayerCharacter.States;

/// <summary>Player movement state. Moves the player and updates the facing animation as direction changes.</summary>
public partial class Move : PlayerState
{
    #region Exports

    [Export]
    public float MoveSpeed { get; set; } = 100;

    #endregion

    #region Lifecycle Methods

    /// <summary>Plays the walk animation for the current facing direction.</summary>
    public override void Enter()
    {
        Player.UpdateAnimation(StateType.Walk);
    }

    /// <summary>Applies velocity and handles input. Transitions to Attack or Idle, fires InteractPressed on interact, and re-plays walk if facing changes.</summary>
    public override PlayerState? Process(double delta)
    {
        if (Input.IsActionJustPressed(InputActions.Attack)) return StateMachine.GetState<Attack>();
        if (Player.Direction == Vector2.Zero) return StateMachine.GetState<Idle>();
        if (Input.IsActionJustPressed(InputActions.Interact)) PlayerManager.Instance.RaisePlayerInteracted();

        Player.Velocity = Player.Direction * MoveSpeed;

        if (Player.SetDirection(Player.Direction))
            Player.UpdateAnimation(StateType.Walk);

        return null;
    }

    #endregion
}
