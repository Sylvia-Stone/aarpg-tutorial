using AarpgTutorial.Common.Utilities;
using Godot;

namespace AarpgTutorial.PlayerCharacter.Managers;

/// <summary>
/// Rotates the player's interaction area (attacks, pickups, etc.) to match the
/// player's cardinal facing direction so hit detection stays aligned.
/// </summary>
public partial class PlayerInteractionsManager : Node2D
{
    #region Exports

    [Export]
    public Player Player = null!;

    #endregion

    #region Lifecycle Methods

    public override void _Ready()
    {
        Player.Require();
        Player.DirectionChanged += UpdateDirection;
    }

    #endregion

    #region Private Methods

    /// <summary>Rotates the interaction area to match the player's current cardinal facing direction.</summary>
    private void UpdateDirection(Vector2 newDirection)
    {
        RotationDegrees = newDirection.ToRotationDegrees();
    }

    #endregion
}
