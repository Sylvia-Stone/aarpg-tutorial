using Godot;

namespace AARPGtutorial.Player.Scripts;

public partial class PlayerInteractionsManager : Node2D
{
    [Export] private Player _player;

    public override void _Ready()
    {
        _player.DirectionChanged += UpdateDirection;
    }

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
}