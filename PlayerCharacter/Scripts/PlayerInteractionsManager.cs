using Godot;

namespace AarpgTutorial.PlayerCharacter.Scripts;

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

    #region Lifecycle

    public override void _Ready()
    {
        Player.DirectionChanged += UpdateDirection;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Rotates the interaction area to face the player's current cardinal direction,
    /// so hit detection stays aligned with the player's facing angle.
    /// </summary>
    private void UpdateDirection(Vector2 newDirection)
    {
        RotationDegrees = newDirection switch
        {
            var direction when direction == Vector2.Down => 0,
            var direction when direction == Vector2.Up => 180,
            var direction when direction == Vector2.Left => 90,
            var direction when direction == Vector2.Right => -90,
            _ => 0
        };
    }

    #endregion
}
